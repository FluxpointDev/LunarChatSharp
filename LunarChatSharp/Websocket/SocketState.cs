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
    public SocketServerState? CurrentServer;
    public RestChannel? CurrentChannel;
    public string LunarCommunityId = null!;
    public string LunarDevId = null!;

    public ConcurrentDictionary<string, SocketServerState> Servers = new ConcurrentDictionary<string, SocketServerState>();
    public ConcurrentDictionary<string, RestChannel> Channels = new ConcurrentDictionary<string, RestChannel>();
    public ConcurrentDictionary<string, RestEmoji> Emojis = new ConcurrentDictionary<string, RestEmoji>();
    public ConcurrentDictionary<string, RestRole> Roles = new ConcurrentDictionary<string, RestRole>();
    public ConcurrentDictionary<string, RestRelation> Relations = new ConcurrentDictionary<string, RestRelation>();



}
public class SocketServerState
{
    public SocketServerState(ServerState server, RestMember member)
    {
        Server = server.Server;
        Member = member;
        Channels = server.Channels;
        Roles = server.Roles;
        Apps = server.Apps;
        Emojis = server.Emojis;
    }
    public RestServer Server;
    public RestMember Member;

    public ConcurrentDictionary<string, RestChannel> Channels;
    public ConcurrentDictionary<string, RestRole> Roles;
    public ConcurrentDictionary<string, RestEmoji> Emojis;
    public ConcurrentDictionary<string, RestApp> Apps;

    public Func<RestChannel, Task> OnChannelCreate;
    public Func<RestChannel, Task> OnChannelDelete;
    public Func<RestChannel, Task> OnChannelUpdate;

    public Func<Task> OnPermissionUpdate;

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
