using LunarChatSharp.Rest.Channels;
using LunarChatSharp.Rest.Messages;
using LunarChatSharp.Rest.Users;
using LunarChatSharp.Websocket.Events;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace LunarChatSharp.Websocket;

public class LunarSocketClient
{
    public LunarSocketClient(string url, string auth)
    {
        webSocketUrl = url;
        authId = auth;
        State.WebSocket = this;
    }

    public delegate void MessageEventHandler(MessageCreateEvent message);
    public delegate void ServerJoinEventHandler(ServerCreateEvent server);
    public SocketState State = new SocketState();

    public event MessageEventHandler? OnMessageRecieved;
    public void TriggerMessage(MessageCreateEvent message)
    {
        OnMessageRecieved?.Invoke(message);
    }



    private string webSocketUrl;
    private string authId;
    private bool _firstConnected { get; set; } = true;
    private bool _firstError = true;
    public bool StopWebSocket = false;

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
                //if (Client.Config.WebSocketProxy != null)
                //    WebSocket.Options.Proxy = Client.Config.WebSocketProxy;

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
                        UserId = authId
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
                        State.Channels = data.Channels;
                        State.Relations = data.Relations;
                        try
                        {
                            State.Servers = new ConcurrentDictionary<string, SocketServerState>(data.Servers.ToDictionary(x => x.Id, x => new SocketServerState
                            {
                                Server = x,
                                Channels = new ConcurrentDictionary<string, RestChannel>(State.Channels[x.Id].ToDictionary(x => x.Id, x => x))
                            }));
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        Console.WriteLine("Test");
                        foreach (var i in State.Servers.Values)
                        {
                            State.TriggerAddServer(i.Server);
                        }
                    }
                    break;
                case "message_create":
                    {
                        MessageCreateEvent? data = payload.Deserialize<MessageCreateEvent>(JsonOptions);
                        TriggerMessage(data);

                    }
                    break;
                case "message_update":
                    {
                        MessageUpdateEvent? data = payload.Deserialize<MessageUpdateEvent>(JsonOptions);
                        _ = State.OnMessageEdit?.Invoke(new RestMessage
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
                        _ = State.OnMessageDelete?.Invoke(data.Message);
                    }
                    break;
                case "server_create":
                    {
                        ServerCreateEvent? data = payload.Deserialize<ServerCreateEvent>(JsonOptions);
                        State.Servers.TryAdd(data.Server.Id, new SocketServerState
                        {
                            Server = data.Server,
                            Channels = new ConcurrentDictionary<string, RestChannel>(data.Channels)
                        });
                        var channels = new List<RestChannel>();
                        foreach (var i in data.Channels)
                        {
                            channels.Add(i.Value);
                        }
                        State.Channels.TryAdd(data.Server.Id, channels);
                        State.TriggerAddServer(data.Server);
                    }
                    break;

                case "server_delete":
                    {
                        ServerDeleteEvent? data = payload.Deserialize<ServerDeleteEvent>(JsonOptions);
                        State.TriggerDeleteServer(State.Servers[data.ServerId].Server);
                        State.Servers.TryRemove(data.ServerId, out _);
                        State.Channels.TryRemove(data.ServerId, out _);
                    }
                    break;
                case "channel_create":
                    {
                        ChannelCreateEvent? data = payload.Deserialize<ChannelCreateEvent>(JsonOptions);
                        State.Servers[data.Channel.ServerId].Channels.TryAdd(data.Channel.Id, data.Channel);
                        State.Channels[data.Channel.ServerId].Add(data.Channel);
                        if (State.CurrentServer.Server.Id == data.Channel.ServerId)
                            State.CurrentServer.OnChannelCreate.Invoke(data.Channel);
                    }
                    break;
                case "channel_delete":
                    {
                        ChannelDeleteEvent? data = payload.Deserialize<ChannelDeleteEvent>(JsonOptions);
                        var channel = State.Channels[data.ServerId].FirstOrDefault(x => x.Id == data.ChannelId);
                        State.Servers[data.ServerId].Channels.TryRemove(data.ChannelId, out channel);
                        State.Channels[data.ServerId].Remove(channel);
                        if (State.CurrentServer.Server.Id == channel.ServerId)
                            State.CurrentServer.OnChannelDelete.Invoke(channel);
                    }
                    break;
                case "channel_update":
                    {
                        ChannelUpdateEvent? data = payload.Deserialize<ChannelUpdateEvent>(JsonOptions);
                        var channel = State.Channels[data.ServerId].FirstOrDefault(x => x.Id == data.ChannelId);
                        channel.Name = data.Name;
                        channel.Topic = data.Topic;
                        if (State.CurrentServer.Server.Id == channel.ServerId)
                            State.CurrentServer.OnChannelUpdate.Invoke(channel);
                    }
                    break;
                case "account_relation_create":
                    {
                        RelationCreateEvent? data = payload.Deserialize<RelationCreateEvent>(JsonOptions);
                        State.Relations.Add(data.Relation.UserId, data.Relation);
                        State.OnRelationAdd.Invoke(data.Relation);
                    }
                    break;
                case "account_relation_delete":
                    {
                        RelationDeleteEvent? data = payload.Deserialize<RelationDeleteEvent>(JsonOptions);
                        State.Relations.Remove(data.UserId, out var relation);
                        State.OnRelationRemove.Invoke(relation);
                    }
                    break;
                case "account_relation_update":
                    {
                        RelationUpdateEvent? data = payload.Deserialize<RelationUpdateEvent>(JsonOptions);
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
