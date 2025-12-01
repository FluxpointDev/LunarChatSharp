using LunarChatSharp.Rest.Channels;
using LunarChatSharp.Rest.Messages;
using LunarChatSharp.Rest.Servers;
using LunarChatSharp.Rest.Users;
using System.Collections.Concurrent;

namespace LunarChatSharp.Websocket;

public class SocketState
{
    public bool APIEnabled = true;
    public LunarSocketClient? WebSocket;
    public string? CurrentId;
    public SocketServerState? CurrentServer;
    public RestChannel? CurrentChannel;
    public ConcurrentDictionary<string, SocketServerState> Servers = new ConcurrentDictionary<string, SocketServerState>();
    public ConcurrentDictionary<string, List<RestChannel>> Channels = new ConcurrentDictionary<string, List<RestChannel>>();

    public Dictionary<string, RestRelation> Relations = new Dictionary<string, RestRelation>();

    public delegate void ServerEventHandler(RestServer server);

    public delegate void ChannelEventHandler(RestChannel channel, RestRelation user);
    public delegate void EventHandler();


    public event ServerEventHandler? OnAddServer;
    public event ServerEventHandler? OnRemoveServer;
    public event ServerEventHandler? OnSelectServer;
    public event ChannelEventHandler? OnSelectChannel;

    public Func<RestRelation, Task>? OnRelationAdd;
    public Func<RestRelation, Task>? OnRelationRemove;

    public Func<RestMessage, Task>? OnMessageEdit;
    public Func<RestMessage, Task>? OnMessageDelete;
    public Func<RestUserPresence, Task>? OnPresenceUpdate;
    public void TriggerAddServer(RestServer server)
    {
        OnAddServer?.Invoke(server);
    }

    public void TriggerDeleteServer(RestServer server)
    {
        OnRemoveServer?.Invoke(server);
    }

    public void TriggerSelectServer(RestServer server)
    {
        OnSelectServer?.Invoke(server);
    }

    public void TriggerSelectChannel(RestChannel channel, RestRelation? user)
    {
        OnSelectChannel?.Invoke(channel, user);
    }
}
public class SocketServerState
{
    public Func<RestChannel, Task> OnChannelCreate;
    public Func<RestChannel, Task> OnChannelDelete;
    public Func<RestChannel, Task> OnChannelUpdate;
    public RestServer Server;
    public ConcurrentDictionary<string, RestChannel> Channels = new ConcurrentDictionary<string, RestChannel>();
    public ConcurrentDictionary<string, List<RestMessage>> Messages = new ConcurrentDictionary<string, List<RestMessage>>();
}
