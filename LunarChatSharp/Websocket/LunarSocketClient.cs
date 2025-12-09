using LunarChatSharp.Rest.Channels;
using LunarChatSharp.Rest.Messages;
using LunarChatSharp.Rest.Roles;
using LunarChatSharp.Rest.Users;
using LunarChatSharp.Websocket.Events;
using LunarChatSharp.Websocket.Events.Account;
using LunarChatSharp.Websocket.Events.Channels;
using LunarChatSharp.Websocket.Events.Messages;
using LunarChatSharp.Websocket.Events.Roles;
using LunarChatSharp.Websocket.Events.Servers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace LunarChatSharp.Websocket;

public class LunarSocketClient
{
    internal LunarSocketClient(LunarClient client)
    {
        webSocketUrl = client.Config.ApiUrl.Replace("https://", "wss://") + "gateway";
        Client = client;
    }

    public SocketState State = new SocketState();

    private string webSocketUrl;
    private bool _firstConnected { get; set; } = true;
    private bool _firstError = true;
    public bool StopWebSocket = false;

    internal LunarClient Client;
    internal ClientWebSocket? WebSocket;
    internal CancellationToken CancellationToken = new CancellationToken();
    internal RestUser? CurrentUser;

    private static JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
    };

    public async Task SetupWebsocket()
    {
        StopWebSocket = false;

        while (!CancellationToken.IsCancellationRequested && !StopWebSocket)
        {
            using (WebSocket = new ClientWebSocket())
            {
                if (Client.Config.WebSocketProxy != null)
                    WebSocket.Options.Proxy = Client.Config.WebSocketProxy;

                try
                {
                    Uri uri = new Uri($"{webSocketUrl}?format=json&version=1");

                    //if (!string.IsNullOrEmpty(Client.Config.CfClearance))
                    //{
                    //    WebSocket.Options.Cookies = new System.Net.CookieContainer();
                    //    WebSocket.Options.Cookies.SetCookies(uri, $"cf_clearance={Client.Config.CfClearance}");
                    //}
                    //WebSocket.Options.SetRequestHeader("Origin", "https://lunar.fluxpoint.dev");
                    //WebSocket.Options.SetRequestHeader("User-Agent", "Lunar Client");
                    //WebSocket.Options.SetRequestHeader("Auth-Id", authId);
                    await WebSocket.ConnectAsync(uri, CancellationToken);
                    _ = Send(WebSocket, JsonSerializer.Serialize(new AuthEvent
                    {
                        UserId = Client.Token
                    }, JsonOptions), CancellationToken.None);

                    _firstError = true;
                    await Receive(WebSocket, CancellationToken);
                }
                catch (ArgumentException)
                {
                    Debug.WriteLine("WebSocket Arg Exception");
                }
                catch (WebSocketException we)
                {
                    Debug.WriteLine(we.ToString());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                await Task.Delay(_firstError ? 3000 : 10000, CancellationToken);
                _firstError = false;
            }
        }
    }

    internal Task Send(ClientWebSocket socket, string data, CancellationToken stoppingToken)
        => socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, stoppingToken);

    private async Task Receive(ClientWebSocket socket, CancellationToken cancellationToken)
    {
        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
        while (!cancellationToken.IsCancellationRequested)
        {
            WebSocketReceiveResult result;
            await using (MemoryStream ms = new MemoryStream())
            {
                do
                {
                    result = await socket.ReceiveAsync(buffer, cancellationToken);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                } while (!result.EndOfMessage);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    _ = WebSocketMessage(await reader.ReadToEndAsync());
                }
            }
        }
    }

    private async Task WebSocketMessage(string json)
    {
        JsonNode payload = JsonNode.Parse(json);

        Debug.WriteLine(payload["type"].ToString());
        try
        {
            switch (payload["type"].ToString())
            {
                case "auth":
                    if (_firstConnected)
                    {
                        //Client.InvokeConnected();
                        //Client.InvokeLog("WebSocket Connected!", StoatLogSeverity.Debug);
                    }
                    //else
                    //Client.InvokeLog("WebSocket Reconnected!", StoatLogSeverity.Debug);

                    _firstConnected = false;
                    //await Send(WebSocket, JsonConvert.SerializeObject(new HeartbeatSocketRequest()), CancellationToken);

                    //_ = Task.Run(async () =>
                    //{
                    //    while (!CancellationToken.IsCancellationRequested)
                    //    {
                    //        await Task.Delay(50000, CancellationToken);
                    //        await Send(WebSocket, JsonConvert.SerializeObject(new HeartbeatRequest()), CancellationToken);
                    //    }
                    //}, CancellationToken);
                    break;
                case "ready":
                    {
                        _firstConnected = false;
                        ReadyEvent? data = payload.Deserialize<ReadyEvent>(JsonOptions);
                        if (data == null)
                            return;
                        State.LunarCommunityId = data.LunarCommunityId!;
                        State.LunarDevId = data.LunarDevId!;
                        State.Account = data.Account!;
                        State.Relations = new ConcurrentDictionary<string, RestRelation>(data.Relations!);
                        State.Servers = new ConcurrentDictionary<string, SocketServerState>(data.Servers!.ToDictionary(x => x.Key, x => new SocketServerState(x.Value, data.Members![x.Key])));

                        Dictionary<string, RestEmoji> Emojis = new Dictionary<string, RestEmoji>();
                        Dictionary<string, RestChannel> Channels = new Dictionary<string, RestChannel>();
                        Dictionary<string, RestRole> Roles = new Dictionary<string, RestRole>();
                        foreach (var i in data.Servers!.Values)
                        {
                            foreach (var e in i.Emojis)
                            {
                                Emojis.TryAdd(e.Key, e.Value);
                            }
                            foreach (var c in i.Channels)
                            {
                                Channels.TryAdd(c.Key, c.Value);
                            }
                            foreach (var r in i.Roles)
                            {
                                Roles.TryAdd(r.Key, r.Value);
                            }
                        }
                        State.Emojis = new ConcurrentDictionary<string, RestEmoji>(Emojis);
                        State.Channels = new ConcurrentDictionary<string, RestChannel>(Channels);
                        State.Roles = new ConcurrentDictionary<string, RestRole>(Roles);

                        foreach (var i in State.Servers.Values)
                        {
                            Client.OnAddServer?.Invoke(i.Server);
                        }
                    }
                    break;
                case "message_create":
                    {
                        MessageCreateEvent? data = payload.Deserialize<MessageCreateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Channels.TryGetValue(data.Message.ChannelId, out var channel))
                            return;

                        Client.OnMessageRecieved?.Invoke(channel, data.Message);

                    }
                    break;
                case "message_update":
                    {
                        MessageUpdateEvent? data = payload.Deserialize<MessageUpdateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Channels.TryGetValue(data.ChannelId, out var channel))
                            return;

                        _ = Client.OnMessageEdit?.Invoke(channel, new RestMessage
                        {
                            Author = data.Author,
                            ChannelId = data.ChannelId,
                            Content = data.Content,
                            Id = data.Id,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = data.UpdatedAt
                        });
                    }
                    break;
                case "message_delete":
                    {
                        MessageDeleteEvent? data = payload.Deserialize<MessageDeleteEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Channels.TryGetValue(data.Message.ChannelId, out var channel))
                            return;

                        _ = Client.OnMessageDelete?.Invoke(channel, data.Message);
                    }
                    break;
                case "server_create":
                    {
                        ServerCreateEvent? data = payload.Deserialize<ServerCreateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        State.Servers.TryAdd(data.Server!.Id, new SocketServerState(new ServerState
                        {
                            Server = data.Server,
                            Channels = new ConcurrentDictionary<string, RestChannel>(data.Channels)
                        }, data.Member!));
                        foreach (var c in data.Channels)
                        {
                            State.Channels.TryAdd(c.Key, c.Value);
                        }

                        Client.OnAddServer?.Invoke(data.Server);
                    }
                    break;
                case "server_update":
                    {
                        ServerUpdateEvent? data = payload.Deserialize<ServerUpdateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        if (data.Name != null)
                            server.Server.Name = data.Name;

                        if (data.Description != null)
                            server.Server.Description = data.Description;

                        if (data.DefaultPermissions != null)
                            server.Server.DefaultPermissions = data.DefaultPermissions;

                        if (data.SystemMessages != null)
                            server.Server.SystemMessages = data.SystemMessages;

                        Client.OnServerUpdate?.Invoke(data);
                        if (data.DefaultPermissions != null && State.CurrentServer != null)
                            State.CurrentServer?.OnPermissionUpdate?.Invoke();
                    }
                    break;
                case "server_delete":
                    {
                        ServerDeleteEvent? data = payload.Deserialize<ServerDeleteEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        Client.OnRemoveServer?.Invoke(server.Server);
                        State.Servers.TryRemove(data.ServerId, out _);
                        foreach (var c in server.Channels)
                        {
                            State.Channels.TryRemove(c.Key, out _);
                        }
                        foreach (var r in server.Roles)
                        {
                            State.Roles.TryRemove(r.Key, out _);
                        }
                        foreach (var e in server.Emojis)
                        {
                            State.Emojis.TryRemove(e.Key, out _);
                        }
                        if (State.CurrentServer != null)
                            State.CurrentServer?.OnPermissionUpdate?.Invoke();
                    }
                    break;
                case "role_create":
                    {
                        RoleCreateEvent? data = payload.Deserialize<RoleCreateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        if (!server.Roles.TryAdd(data.Role.Id, data.Role))
                            return;

                        State.Roles.TryAdd(data.Role.Id, data.Role);

                        foreach (var i in data.Positions)
                        {
                            if (server.Roles.TryGetValue(i.Key, out var role))
                                role.Position = i.Value;
                        }

                        Client.OnRoleCreate?.Invoke(server.Server, data.Role);

                        if (State.CurrentServer != null)
                            State.CurrentServer?.OnPermissionUpdate?.Invoke();
                    }
                    break;

                case "role_update":
                    {
                        RoleUpdateEvent? data = payload.Deserialize<RoleUpdateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Roles.TryGetValue(data.RoleId, out var role))
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        if (data.Name != null)
                            role.Name = data.Name;

                        if (data.Color != null)
                            role.Color = data.Color;

                        if (data.Permissions != null)
                            role.Permissions = data.Permissions;

                        Client.OnRoleUpdate?.Invoke(server.Server, role, data);

                        if (data.Permissions != null && State.CurrentServer != null)
                            State.CurrentServer?.OnPermissionUpdate?.Invoke();
                    }
                    break;
                case "role_delete":
                    {
                        RoleDeleteEvent? data = payload.Deserialize<RoleDeleteEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Roles.TryRemove(data.RoleId, out var role))
                            return;

                        if (State.Servers.TryGetValue(data.ServerId, out var server))
                            server.Roles.TryRemove(data.RoleId, out _);

                        foreach (var i in data.Positions)
                        {
                            if (server.Roles.TryGetValue(i.Key, out var getRole))
                                getRole.Position = i.Value;
                        }

                        Client.OnRoleDelete?.Invoke(server.Server, role);

                        if (State.CurrentServer != null)
                            State.CurrentServer?.OnPermissionUpdate?.Invoke();
                    }
                    break;
                case "role_positions":
                    {
                        RolePositionsEvent? data = payload.Deserialize<RolePositionsEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        foreach (var i in data.Positions)
                        {
                            if (server.Roles.TryGetValue(i.Key, out var getRole))
                                getRole.Position = i.Value;
                        }
                    }
                    break;
                case "channel_create":
                    {
                        ChannelCreateEvent? data = payload.Deserialize<ChannelCreateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (data.Channel.Type == Core.Channels.ChannelType.Direct || data.Channel.Type == Core.Channels.ChannelType.Group)
                        {
                            State.Channels.TryAdd(data.Channel.Id, data.Channel);
                        }
                        else
                        {
                            if (!State.Servers.TryGetValue(data.Channel.ServerId, out var server))
                                return;

                            server.Channels.TryAdd(data.Channel.Id, data.Channel);
                            State.Channels.TryAdd(data.Channel.Id, data.Channel);
                            if (State.CurrentServer != null && State.CurrentServer.Server.Id == data.Channel.ServerId)
                                State.CurrentServer.OnChannelCreate?.Invoke(data.Channel);
                        }
                    }
                    break;
                case "channel_delete":
                    {
                        ChannelDeleteEvent? data = payload.Deserialize<ChannelDeleteEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Channels.TryGetValue(data.ChannelId, out var channel))
                            return;

                        if (channel.Type == Core.Channels.ChannelType.Direct || channel.Type == Core.Channels.ChannelType.Group)
                        {
                            State.Channels.TryRemove(channel.Id, out _);
                        }
                        else
                        {
                            if (State.Servers.TryGetValue(channel.ServerId, out var server))
                                server.Channels.TryRemove(channel.Id, out _);

                            State.Channels.TryRemove(channel.Id, out _);
                        }
                        if (State.CurrentServer != null && State.CurrentServer.Server.Id == channel.ServerId)
                            State.CurrentServer.OnChannelDelete?.Invoke(channel);
                    }
                    break;
                case "channel_update":
                    {
                        ChannelUpdateEvent? data = payload.Deserialize<ChannelUpdateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Channels.TryGetValue(data.ChannelId, out var channel))
                            return;

                        channel.Name = data.Name;
                        channel.Topic = data.Topic;
                        if (State.CurrentServer != null && State.CurrentServer.Server.Id == channel.ServerId)
                            State.CurrentServer.OnChannelUpdate?.Invoke(channel);
                    }
                    break;
                case "emoji_create":
                    {
                        EmojiCreateEvent? data = payload.Deserialize<EmojiCreateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        if (!server.Emojis.TryAdd(data.Emoji.Id, data.Emoji))
                            return;

                        State.Emojis.TryAdd(data.Emoji.Id, data.Emoji);

                        Client.OnEmojiCreate?.Invoke(server.Server, data.Emoji);
                    }
                    break;
                case "emoji_update":
                    {
                        EmojiUpdateEvent? data = payload.Deserialize<EmojiUpdateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        if (!server.Emojis.TryGetValue(data.EmojiId, out var emoji))
                            return;

                        emoji.Name = data.Name;

                        Client.OnEmojiUpdate?.Invoke(server.Server, emoji, data);
                    }
                    break;
                case "emoji_delete":
                    {
                        EmojiDeleteEvent? data = payload.Deserialize<EmojiDeleteEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        if (!server.Emojis.TryRemove(data.EmojiId, out var emoji))
                            return;

                        State.Emojis.TryRemove(data.EmojiId, out _);

                        Client.OnEmojiDelete?.Invoke(server.Server, emoji);
                    }
                    break;
                case "app_add":
                    {
                        AppAddEvent? data = payload.Deserialize<AppAddEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        server.Apps.TryAdd(data.App.Id, data.App);

                        Client.OnAppAdd?.Invoke(server.Server, data.App);
                    }
                    break;
                case "app_update":
                    {
                        AppUpdatedEvent? data = payload.Deserialize<AppUpdatedEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        if (!server.Apps.TryRemove(data.AppId, out var app))
                            return;

                        if (data.Changed != null)
                        {
                            app.Name = data.Changed.Name;
                            app.Description = data.Changed.Description;
                            app.IsPublic = data.Changed.IsPublic;
                            app.Website = data.Changed.Website;
                            app.Terms = data.Changed.Terms;
                            app.Privacy = data.Changed.Privacy;
                        }


                        Client.OnAppUpdate?.Invoke(server.Server, app, data);
                    }
                    break;
                case "app_remove":
                    {
                        AppRemoveEvent? data = payload.Deserialize<AppRemoveEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        if (!server.Apps.TryRemove(data.AppId, out var app))
                            return;

                        Client.OnAppRemove?.Invoke(server.Server, app);
                    }
                    break;
                case "account_relation_create":
                    {
                        RelationCreateEvent? data = payload.Deserialize<RelationCreateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (data.Relation == null || !State.Relations.TryAdd(data.Relation.UserId, data.Relation))
                            return;

                        Client.OnRelationAdd?.Invoke(data.Relation);
                    }
                    break;
                case "account_relation_delete":
                    {
                        RelationDeleteEvent? data = payload.Deserialize<RelationDeleteEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Relations.TryRemove(data.UserId, out var relation))
                            return;

                        Client.OnRelationRemove?.Invoke(relation);
                    }
                    break;
                case "account_relation_update":
                    {
                        RelationUpdateEvent? data = payload.Deserialize<RelationUpdateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Relations.TryRemove(data.UserId, out var relation))
                            return;

                        Client.OnRelationUpdate?.Invoke(relation, data);
                    }
                    break;
                case "account_update":
                    {
                        AccountUpdateEvent? data = payload.Deserialize<AccountUpdateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        Client.OnAccountUpdate?.Invoke(data);
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}
