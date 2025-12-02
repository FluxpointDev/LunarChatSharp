using LunarChatSharp.Rest.Channels;
using LunarChatSharp.Rest.Messages;
using LunarChatSharp.Rest.Roles;
using LunarChatSharp.Rest.Servers;
using LunarChatSharp.Rest.Users;
using LunarChatSharp.Websocket.Events;
using LunarChatSharp.Websocket.Events.Account;
using LunarChatSharp.Websocket.Events.Roles;
using LunarChatSharp.Websocket.Events.Servers;
using System.Collections.Concurrent;

namespace LunarChatSharp.Websocket;

public class SocketState
{
    public bool APIEnabled = true;
    public LunarSocketClient? WebSocket;
    public string? CurrentId;
    public RestAccount Account;
    public SocketServerState? CurrentServer;
    public RestChannel? CurrentChannel;
    public ConcurrentDictionary<string, SocketServerState> Servers = new ConcurrentDictionary<string, SocketServerState>();
    public ConcurrentDictionary<string, RestChannel> Channels = new ConcurrentDictionary<string, RestChannel>();
    public ConcurrentDictionary<string, RestEmoji> Emojis = new ConcurrentDictionary<string, RestEmoji>();
    public ConcurrentDictionary<string, RestRole> Roles = new ConcurrentDictionary<string, RestRole>();
    public ConcurrentDictionary<string, RestRelation> Relations = new ConcurrentDictionary<string, RestRelation>();


    public Func<RestMessage, Task>? OnMessageRecieved;
    public Func<RestServer, Task>? OnAddServer;
    public Func<RestServer, Task>? OnRemoveServer;
    public Func<ServerUpdateEvent, Task>? OnServerUpdate;
    public Func<RestServer, Task>? OnSelectServer;
    public Func<RestChannel, RestRelation, Task>? OnSelectChannel;

    public Func<RestRelation, Task>? OnRelationAdd;
    public Func<RestRelation, Task>? OnRelationRemove;

    public Func<RestMessage, Task>? OnMessageEdit;
    public Func<RestMessage, Task>? OnMessageDelete;
    public Func<RestUserPresence, Task>? OnPresenceUpdate;
    public Func<AccountUpdateEvent, Task>? OnAccountUpdate;

    public Func<RestServer, RestRole, Task>? OnRoleCreate;
    public Func<RoleUpdateEvent, RestRole, Task>? OnRoleUpdate;
    public Func<RestRole, Task>? OnRoleDelete;
}
public class SocketServerState
{
    public SocketServerState(ServerState server)
    {
        Server = server.Server;
        Channels = server.Channels;
        Roles = server.Roles;
        Emojis = server.Emojis;
    }
    public RestServer Server { get; set; }

    public ConcurrentDictionary<string, RestChannel> Channels;
    public ConcurrentDictionary<string, RestRole> Roles;
    public ConcurrentDictionary<string, RestEmoji> Emojis;

    public Func<RestChannel, Task> OnChannelCreate;
    public Func<RestChannel, Task> OnChannelDelete;
    public Func<RestChannel, Task> OnChannelUpdate;

    //public ConcurrentDictionary<string, List<RestMessage>> Messages = new ConcurrentDictionary<string, List<RestMessage>>();
}
