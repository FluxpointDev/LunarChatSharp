using LunarChatSharp.Core.Channels;
using LunarChatSharp.Core.Servers;
using LunarChatSharp.Rest.Channels;
using LunarChatSharp.Rest.Messages;
using LunarChatSharp.Rest.Roles;
using LunarChatSharp.Rest.Users;
using LunarChatSharp.Websocket.Events;
using LunarChatSharp.Websocket.Events.Account;
using LunarChatSharp.Websocket.Events.Channels;
using LunarChatSharp.Websocket.Events.Groups;
using LunarChatSharp.Websocket.Events.Members;
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
                        State.PrivateChannels = data.PrivateChannels;

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
                        foreach (var i in State.PrivateChannels)
                        {
                            State.Channels.TryAdd(i.Value.Id, i.Value);
                        }

                        State.Roles = new ConcurrentDictionary<string, RestRole>(Roles);
                        foreach (var i in State.Servers.Values)
                        {
                            Client.OnAddServer?.Invoke(i.Server);
                        }

                        Client.OnReady?.Invoke();
                    }
                    break;

                #region Messages
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

                        _ = Client.OnMessageEdit?.Invoke(channel, data, data.Changed);
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
                #endregion

                #region Servers
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

                        if (data.Changed.Name != null)
                            server.Server.Name = data.Changed.Name;

                        if (data.Changed.Description != null)
                            server.Server.Description = data.Changed.Description.ToNullOrString();

                        if (data.Changed.DefaultPermissions != null)
                            server.Server.DefaultPermissions = data.Changed.DefaultPermissions;

                        if (data.Changed.VanityInvite != null)
                            server.Server.VanityInvite = data.Changed.VanityInvite;

                        if (!string.IsNullOrEmpty(data.Changed.OwnerId))
                            server.Server.OwnerId = data.Changed.OwnerId;

                        if (data.Changed.IsDiscoverable.HasValue)
                        {
                            if (data.Changed.IsDiscoverable.Value)
                                server.Server.Features |= ServerFeature.Discoverable;
                            else
                                server.Server.Features &= ~ServerFeature.Discoverable;
                        }

                        if (data.Changed.SystemMessages != null)
                        {
                            if (data.Changed.SystemMessages.MemberJoined != null)
                                server.Server.SystemMessages.MemberJoined = data.Changed.SystemMessages.MemberJoined.ToNullOrString();

                            if (data.Changed.SystemMessages.MemberLeft != null)
                                server.Server.SystemMessages.MemberLeft = data.Changed.SystemMessages.MemberLeft.ToNullOrString();

                            if (data.Changed.SystemMessages.MemberBanned != null)
                                server.Server.SystemMessages.MemberBanned = data.Changed.SystemMessages.MemberBanned.ToNullOrString();

                            if (data.Changed.SystemMessages.MemberUnbanned != null)
                                server.Server.SystemMessages.MemberUnbanned = data.Changed.SystemMessages.MemberUnbanned.ToNullOrString();

                            if (data.Changed.SystemMessages.MemberKicked != null)
                                server.Server.SystemMessages.MemberKicked = data.Changed.SystemMessages.MemberKicked.ToNullOrString();

                            if (data.Changed.SystemMessages.MemberTimedout != null)
                                server.Server.SystemMessages.MemberTimedout = data.Changed.SystemMessages.MemberTimedout.ToNullOrString();

                            if (data.Changed.SystemMessages.AppAdded != null)
                                server.Server.SystemMessages.AppAdded = data.Changed.SystemMessages.AppAdded.ToNullOrString();

                            if (data.Changed.SystemMessages.AppRemoved != null)
                                server.Server.SystemMessages.AppRemoved = data.Changed.SystemMessages.AppRemoved.ToNullOrString();
                        }

                        Client.OnServerUpdate?.Invoke(server.Server, data);
                        if (State.CurrentServer != null)
                        {
                            if (data.Changed.DefaultPermissions != null)
                                State.CurrentServer?.OnPermissionUpdate?.Invoke();
                            else if (!string.IsNullOrEmpty(data.Changed.OwnerId))
                                State.CurrentServer?.OnPermissionUpdate?.Invoke();
                        }
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
                        server.Server.OwnerId = "0";
                        server.Server.DefaultPermissions = new RestPermissions();
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
                #endregion

                #region Members
                case "member_join":
                    {
                        MemberJoinEvent? data = payload.Deserialize<MemberJoinEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        Client.OnMemberJoin?.Invoke(server.Server, data.Member);
                    }
                    break;
                case "member_left":
                    {
                        MemberLeftEvent? data = payload.Deserialize<MemberLeftEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        Client.OnMemberLeft?.Invoke(server.Server, data.Member);
                    }
                    break;
                case "member_ban":
                    {
                        MemberBanEvent? data = payload.Deserialize<MemberBanEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        Client.OnMemberBan?.Invoke(server.Server, data.Member, data.Ban);
                    }
                    break;
                case "member_unban":
                    {
                        MemberUnbanEvent? data = payload.Deserialize<MemberUnbanEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        Client.OnMemberUnban?.Invoke(server.Server, data.User);
                    }
                    break;
                case "member_kick":
                    {
                        MemberKickEvent? data = payload.Deserialize<MemberKickEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        Client.OnMemberKick?.Invoke(server.Server, data.Member);
                    }
                    break;
                case "member_timeout":
                    {
                        MemberTimeoutEvent? data = payload.Deserialize<MemberTimeoutEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        Client.OnMemberTimeout?.Invoke(server.Server, data.Member);
                    }
                    break;
                case "member_update":
                    {
                        MemberUpdateEvent? data = payload.Deserialize<MemberUpdateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        if (data.Changed.Nickname != null && data.UserId == server.Member.Id)
                            server.Member.Nickname = data.Changed.Nickname.ToNullOrString();

                        if (data.Changed.TimeoutRemove)
                            server.Member.Timeout = null;
                        else if (data.Changed.Timeout.HasValue)
                            server.Member.Timeout = data.Changed.Timeout.Value;

                        Client.OnMemberUpdate?.Invoke(server.Server, data.UserId, data.Changed);
                    }
                    break;
                #endregion

                #region Roles
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
                #endregion

                #region Channels
                case "channel_create":
                    {
                        ChannelCreateEvent? data = payload.Deserialize<ChannelCreateEvent>(JsonOptions);
                        if (data == null || data.Channel == null)
                            return;

                        if (data.Channel.Type == ChannelType.Direct || data.Channel.Type == ChannelType.Group)
                        {
                            State.Channels.TryAdd(data.Channel.Id, data.Channel);
                            State.PrivateChannels.TryAdd(data.Channel.Id, data.Channel);
                            if (data.Channel.Type == ChannelType.Direct)
                                Client.OnDMCreate?.Invoke(data.Channel);
                            else
                                Client.OnGroupCreate?.Invoke(data.Channel);
                        }
                        else
                        {
                            if (!State.Servers.TryGetValue(data.Channel.ServerId, out var server))
                                return;

                            server.Channels.TryAdd(data.Channel.Id, data.Channel);
                            State.Channels.TryAdd(data.Channel.Id, data.Channel);
                            if (State.CurrentServer != null && !string.IsNullOrEmpty(data.Channel.ServerId) && State.CurrentServer.Server.Id == data.Channel.ServerId)
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

                        if (channel.Type == ChannelType.Direct || channel.Type == ChannelType.Group)
                        {
                            State.Channels.TryRemove(channel.Id, out _);
                            State.PrivateChannels.TryRemove(channel.Id, out _);

                            if (channel.Type == ChannelType.Direct)
                                Client.OnDMDelete?.Invoke(channel);
                            else
                                Client.OnGroupDelete?.Invoke(channel);
                        }
                        else
                        {
                            if (State.Servers.TryGetValue(channel.ServerId, out var server))
                                server.Channels.TryRemove(channel.Id, out _);

                            State.Channels.TryRemove(channel.Id, out _);
                        }
                        if (State.CurrentServer != null && !string.IsNullOrEmpty(channel.ServerId) && State.CurrentServer.Server.Id == channel.ServerId)
                            State.CurrentServer.OnChannelDelete?.Invoke(channel);
                    }
                    break;
                case "channel_update":
                    {
                        ChannelUpdateEvent? data = payload.Deserialize<ChannelUpdateEvent>(JsonOptions);
                        if (data == null || data.Changed == null)
                            return;

                        if (!State.Channels.TryGetValue(data.ChannelId, out var channel))
                            return;

                        if (data.Changed != null)
                        {
                            if (data.Changed.Name != null)
                                channel.Name = data.Changed.Name;

                            if (data.Changed.Topic != null)
                                channel.Topic = data.Changed.Topic;
                        }

                        switch (channel.Type)
                        {
                            case ChannelType.Direct:
                                Client.OnDMUpdate?.Invoke(channel, data.Changed);
                                break;
                            case ChannelType.Group:
                                {
                                    if (!string.IsNullOrEmpty(data.Changed.OwnerId))
                                        channel.GroupSettings?.OwnerId = data.Changed.OwnerId;

                                    Client.OnGroupUpdate?.Invoke(channel, data.Changed);
                                }
                                break;
                        }

                        if (State.CurrentServer != null && !string.IsNullOrEmpty(channel.ServerId) && State.CurrentServer.Server.Id == channel.ServerId)
                            State.CurrentServer.OnChannelUpdate?.Invoke(channel, data.Changed);
                    }
                    break;
                case "invite_create":
                    {
                        InviteCreateEvent? data = payload.Deserialize<InviteCreateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        Client.OnInviteCreate?.Invoke(server.Server, data.Invite);
                    }
                    break;
                case "invite_delete":
                    {
                        InviteDeleteEvent? data = payload.Deserialize<InviteDeleteEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Servers.TryGetValue(data.ServerId, out var server))
                            return;

                        Client.OnInviteDelete?.Invoke(server.Server, data.Code);
                    }
                    break;

                #endregion

                #region Groups
                case "group_user_add":
                    {
                        GroupAddUserEvent? data = payload.Deserialize<GroupAddUserEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Channels.TryGetValue(data.ChannelId, out var channel))
                            return;

                        channel.Users.Add(data.User);

                        Client.OnGroupAddUser?.Invoke(channel, data.User);
                    }
                    break;
                case "group_user_remove":
                    {
                        GroupRemoveUserEvent? data = payload.Deserialize<GroupRemoveUserEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Channels.TryGetValue(data.ChannelId, out var channel))
                            return;

                        var item = channel.Users.FirstOrDefault(x => x.Id == data.UserId);
                        if (item != null)
                            channel.Users.Remove(item);

                        Client.OnGroupRemoveUser?.Invoke(channel, data.UserId);
                    }
                    break;
                #endregion
                #region Emojis
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
                #endregion

                #region Apps
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
                            app.Description = data.Changed.Description.ToNullOrString();
                            app.IsPublic = data.Changed.IsPublic;
                            app.Website = data.Changed.Website.ToNullOrString();
                            app.Terms = data.Changed.Terms.ToNullOrString();
                            app.Privacy = data.Changed.Privacy.ToNullOrString();
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
                #endregion

                #region Webhooks
                case "webhook_create":
                    {
                        WebhookCreateEvent? data = payload.Deserialize<WebhookCreateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Channels.TryGetValue(data.Webhook.ChannelId, out var channel))
                            return;

                        Client.OnWebhookCreate?.Invoke(channel, data.Webhook);
                    }
                    break;
                case "webhook_update":
                    {
                        WebhookUpdateEvent? data = payload.Deserialize<WebhookUpdateEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Channels.TryGetValue(data.ChannelId, out var channel))
                            return;

                        Client.OnWebhookUpdate?.Invoke(channel, data.WebhookId, data.Changed);
                    }
                    break;
                case "webhook_delete":
                    {
                        WebhookDeleteEvent? data = payload.Deserialize<WebhookDeleteEvent>(JsonOptions);
                        if (data == null)
                            return;

                        if (!State.Channels.TryGetValue(data.ChannelId, out var channel))
                            return;

                        Client.OnWebhookDelete?.Invoke(channel, data.WebhookId);
                    }
                    break;
                #endregion

                #region Accounts
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

                        if (!State.Relations.TryGetValue(data.UserId, out var relation))
                            return;

                        if (data.Note != null)
                            relation.Note = data.Note.ToNullOrString();

                        Client.OnRelationUpdate?.Invoke(relation, data);
                    }
                    break;
                case "account_update":
                    {
                        AccountUpdateEvent? data = payload.Deserialize<AccountUpdateEvent>(JsonOptions);
                        if (data == null || data.Changed == null)
                            return;

                        //if (data.Changed.AboutMe != null)
                        //{
                        //    data.Changed.AboutMe = data.Changed.AboutMe.Trim();


                        //    State.AboutMe = data.Changed.AboutMe.ToNullOrString();
                        //}

                        if (data.Changed.FriendRequestAccess != null)
                        {
                            if (data.Changed.FriendRequestAccess.Everyone.HasValue)
                                State.Account.FriendRequestAccess.Everyone = data.Changed.FriendRequestAccess.Everyone.Value;

                            if (data.Changed.FriendRequestAccess.MutualServers.HasValue)
                                State.Account.FriendRequestAccess.MutualServers = data.Changed.FriendRequestAccess.MutualServers.Value;

                            if (data.Changed.FriendRequestAccess.MutualFriends.HasValue)
                                State.Account.FriendRequestAccess.MutualFriends = data.Changed.FriendRequestAccess.MutualFriends.Value;
                        }

                        if (data.Changed.DirectMessagesAccess != null)
                        {
                            if (data.Changed.DirectMessagesAccess.Everyone.HasValue)
                                State.Account.DirectMessagesAccess.Everyone = data.Changed.DirectMessagesAccess.Everyone.Value;

                            if (data.Changed.DirectMessagesAccess.MutualServers.HasValue)
                                State.Account.DirectMessagesAccess.MutualServers = data.Changed.DirectMessagesAccess.MutualServers.Value;

                            if (data.Changed.DirectMessagesAccess.MutualFriends.HasValue)
                                State.Account.DirectMessagesAccess.MutualFriends = data.Changed.DirectMessagesAccess.MutualFriends.Value;
                        }

                        Client.OnAccountUpdate?.Invoke(data);
                    }
                    break;
                    #endregion
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}
