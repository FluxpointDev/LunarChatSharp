using LunarChatSharp.Core.Servers;
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
    public string LunarCommunityId = null!;
    public string LunarDevId = null!;

    public ConcurrentDictionary<string, SocketServerState> Servers = new ConcurrentDictionary<string, SocketServerState>();
    public ConcurrentDictionary<string, RestChannel> Channels = new ConcurrentDictionary<string, RestChannel>();
    public ConcurrentDictionary<string, RestEmoji> Emojis = new ConcurrentDictionary<string, RestEmoji>();
    public ConcurrentDictionary<string, RestRole> Roles = new ConcurrentDictionary<string, RestRole>();
    public ConcurrentDictionary<string, RestRelation> Relations = new ConcurrentDictionary<string, RestRelation>();


    public Func<RestMessage, Task>? OnMessageRecieved;
    public Func<RestServer, Task>? OnAddServer;
    public Func<RestServer, Task>? OnRemoveServer;
    public Func<ServerUpdateEvent, Task>? OnServerUpdate;
    public Func<RestServer?, Task>? OnSelectServer;
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
    public SocketServerState(ServerState server, RestMember member)
    {
        Server = server.Server;
        Member = member;
        Channels = server.Channels;
        Roles = server.Roles;
        Emojis = server.Emojis;
    }
    public RestServer Server;
    public RestMember Member;

    public ConcurrentDictionary<string, RestChannel> Channels;
    public ConcurrentDictionary<string, RestRole> Roles;
    public ConcurrentDictionary<string, RestEmoji> Emojis;

    public Func<RestChannel, Task> OnChannelCreate;
    public Func<RestChannel, Task> OnChannelDelete;
    public Func<RestChannel, Task> OnChannelUpdate;

    public bool HasPermission(RestMember member, ServerPermission permission)
    {
        if (Server.DefaultPermissions.ServerPermissions.HasFlag(permission) || member.Id == Server.OwnerId)
            return true;

        if (member.Roles != null)
        {
            foreach (var i in member.Roles)
            {
                if (Roles.TryGetValue(i, out var role) && role.Permissions.ServerPermissions.HasFlag(permission))
                    return true;
            }
        }
        return false;
    }

    public bool HasPermission(RestMember member, ModPermission permission)
    {
        if (Server.DefaultPermissions.ModPermissions.HasFlag(permission) || member.Id == Server.OwnerId)
            return true;

        if (member.Roles != null)
        {
            foreach (var i in member.Roles)
            {
                if (Roles.TryGetValue(i, out var role) && role.Permissions.ModPermissions.HasFlag(permission))
                    return true;
            }
        }
        return false;
    }

    public bool HasPermission(RestMember member, ChannelPermission permission)
    {
        if (Server.DefaultPermissions.ChannelPermissions.HasFlag(permission) || member.Id == Server.OwnerId)
            return true;

        if (member.Roles != null)
        {
            foreach (var i in member.Roles)
            {
                if (Roles.TryGetValue(i, out var role) && role.Permissions.ChannelPermissions.HasFlag(permission))
                    return true;
            }
        }
        return false;
    }

    public bool HasPermission(RestMember member, VoicePermission permission)
    {
        if (Server.DefaultPermissions.VoicePermissions.HasFlag(permission) || member.Id == Server.OwnerId)
            return true;

        if (member.Roles != null)
        {
            foreach (var i in member.Roles)
            {
                if (Roles.TryGetValue(i, out var role) && role.Permissions.VoicePermissions.HasFlag(permission))
                    return true;
            }
        }
        return false;
    }

    //public ConcurrentDictionary<string, List<RestMessage>> Messages = new ConcurrentDictionary<string, List<RestMessage>>();
}
