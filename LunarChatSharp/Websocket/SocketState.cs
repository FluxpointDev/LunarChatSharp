using LunarChatSharp.Core.Servers;
using LunarChatSharp.Rest.Channels;
using LunarChatSharp.Rest.Dev;
using LunarChatSharp.Rest.Messages;
using LunarChatSharp.Rest.Roles;
using LunarChatSharp.Rest.Servers;
using LunarChatSharp.Rest.Users;
using LunarChatSharp.Websocket.Events;
using System.Collections.Concurrent;

namespace LunarChatSharp.Websocket;

public class SocketState
{
    public RestAccount Account;
    public ulong? LunarCommunityId = null;
    public ulong? LunarDevId = null;

    public ConcurrentDictionary<ulong, SocketServerState> Servers = new ConcurrentDictionary<ulong, SocketServerState>();
    public ConcurrentDictionary<ulong, RestChannel> PrivateChannels = new ConcurrentDictionary<ulong, RestChannel>();
    public ConcurrentDictionary<ulong, RestChannel> Channels = new ConcurrentDictionary<ulong, RestChannel>();
    public ConcurrentDictionary<ulong, RestEmoji> Emojis = new ConcurrentDictionary<ulong, RestEmoji>();
    public ConcurrentDictionary<ulong, RestRole> Roles = new ConcurrentDictionary<ulong, RestRole>();
    public ConcurrentDictionary<ulong, RestRelation> Relations = new ConcurrentDictionary<ulong, RestRelation>();



}
public class SocketServerState
{
    public SocketServerState(LunarClient client, ServerState server, RestMember member)
    {
        Server = server.Server;
        Member = member;
        Channels = server.Channels;
        Roles = server.Roles;
        Apps = server.Apps;
        Emojis = server.Emojis;
        client.OnPermissionUpdate += PermissionUpdate;
    }

    private async Task PermissionUpdate(RestServer server)
    {
        if (Server.Id != server.Id)
            return;

        OnPermissionUpdate?.Invoke(server);
    }

    public RestServer Server;
    public RestMember Member;

    public ConcurrentDictionary<ulong, RestChannel> Channels;
    public ConcurrentDictionary<ulong, RestRole> Roles;
    public ConcurrentDictionary<ulong, RestEmoji> Emojis;
    public ConcurrentDictionary<ulong, RestApp> Apps;
    public Func<RestServer, Task>? OnPermissionUpdate;

    public bool CanManageServer(RestMember member)
    {
        bool CanView = HasPermission(member, ServerPermission.ManageServer);
        if (!CanView)
            CanView = HasPermission(member, ServerPermission.ManageExpressions);
        if (!CanView)
            CanView = HasPermission(member, ServerPermission.CreateExpressions);
        if (!CanView)
            CanView = HasPermission(member, ServerPermission.ManageApps);
        if (!CanView)
            CanView = HasPermission(member, ModPermission.BanMembers);
        if (!CanView)
            CanView = HasPermission(member, ModPermission.ViewAuditLogs);
        if (!CanView)
            CanView = HasPermission(member, ModPermission.AssignRoles);
        if (!CanView)
            CanView = HasPermission(member, ModPermission.ManageRoles);
        if (!CanView)
            CanView = HasPermission(member, ModPermission.ManageRolePermissions);
        if (!CanView)
            CanView = HasPermission(member, ModPermission.ManageApprovals);
        if (!CanView)
            CanView = HasPermission(member, ModPermission.ManageAppeals);
        if (!CanView)
            CanView = HasPermission(member, ChannelPermission.ManageWebhooks);
        if (!CanView)
            CanView = HasPermission(member, ChannelPermission.ManageInvites);
        return CanView;
    }

    public bool CanManageChannel(RestMember member)
    {
        bool CanManage = HasPermission(member, ChannelPermission.ManageChannel);
        if (!CanManage)
            CanManage = HasPermission(member, ChannelPermission.ManageChannelPermissions);

        if (!CanManage)
            CanManage = HasPermission(member, ChannelPermission.ManageWebhooks);

        if (!CanManage)
            CanManage = HasPermission(member, ChannelPermission.ManageInvites);

        return CanManage;
    }

    public bool HasPermission(RestMember member, ServerPermission permission)
    {
        if (member.Id == Server.OwnerId || Server.DefaultPermissions.ServerPermissions.HasFlag(ServerPermission.Administrator))
            return true;

        if (Server.DefaultPermissions.ServerPermissions.HasFlag(permission))
            return true;

        if (member.Roles != null)
        {
            foreach (var i in member.Roles)
            {
                if (Roles.TryGetValue(i, out var role))
                {
                    if (role.Permissions.ServerPermissions.HasFlag(ServerPermission.Administrator))
                        return true;

                    if (role.Permissions.ServerPermissions.HasFlag(permission))
                        return true;
                }
            }
        }
        return false;
    }

    public bool HasPermission(RestMember member, ModPermission permission)
    {
        if (member.Id == Server.OwnerId || Server.DefaultPermissions.ServerPermissions.HasFlag(ServerPermission.Administrator))
            return true;

        if (Server.DefaultPermissions.ModPermissions.HasFlag(permission))
            return true;

        if (member.Roles != null)
        {
            foreach (var i in member.Roles)
            {
                if (Roles.TryGetValue(i, out var role))
                {
                    if (role.Permissions.ServerPermissions.HasFlag(ServerPermission.Administrator))
                        return true;

                    if (role.Permissions.ModPermissions.HasFlag(permission))
                        return true;
                }
            }
        }
        return false;
    }

    public bool HasPermission(RestMember member, ChannelPermission permission)
    {
        if (member.Id == Server.OwnerId || Server.DefaultPermissions.ServerPermissions.HasFlag(ServerPermission.Administrator))
            return true;

        if (Server.DefaultPermissions.ChannelPermissions.HasFlag(permission))
            return true;

        if (member.Roles != null)
        {
            foreach (var i in member.Roles)
            {
                if (Roles.TryGetValue(i, out var role))
                {
                    if (role.Permissions.ServerPermissions.HasFlag(ServerPermission.Administrator))
                        return true;

                    if (role.Permissions.ChannelPermissions.HasFlag(permission))
                        return true;
                }
            }
        }
        return false;
    }

    public bool HasPermission(RestMember member, VoicePermission permission)
    {
        if (member.Id == Server.OwnerId || Server.DefaultPermissions.ServerPermissions.HasFlag(ServerPermission.Administrator))
            return true;

        if (Server.DefaultPermissions.VoicePermissions.HasFlag(permission))
            return true;

        if (member.Roles != null)
        {
            foreach (var i in member.Roles)
            {
                if (Roles.TryGetValue(i, out var role))
                {
                    if (role.Permissions.ServerPermissions.HasFlag(ServerPermission.Administrator))
                        return true;

                    if (role.Permissions.VoicePermissions.HasFlag(permission))
                        return true;
                }
            }
        }
        return false;
    }

    //public ConcurrentDictionary<string, List<RestMessage>> Messages = new ConcurrentDictionary<string, List<RestMessage>>();
}
