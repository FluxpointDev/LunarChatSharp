using LunarChatSharp.Core.Servers;
using LunarChatSharp.Rest.Channels;
using LunarChatSharp.Rest.Dev;
using LunarChatSharp.Rest.Messages;
using LunarChatSharp.Rest.Roles;
using LunarChatSharp.Rest.Servers;
using LunarChatSharp.Rest.Users;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events;

public class AuthEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "auth";

    [JsonPropertyName("user_id")]
    public required ulong UserId { get; set; }
}
public class ReadyEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "ready";

    [JsonPropertyName("account")]
    public required RestAccount? Account { get; set; }

    [JsonPropertyName("servers")]
    public required ConcurrentDictionary<ulong, ServerState>? Servers { get; set; }

    [JsonPropertyName("private_channels")]
    public required ConcurrentDictionary<ulong, RestChannel>? PrivateChannels { get; set; }

    [JsonPropertyName("members")]
    public required ConcurrentDictionary<ulong, RestMember>? Members { get; set; }

    [JsonPropertyName("relations")]
    public required Dictionary<ulong, RestRelation>? Relations { get; set; }

    [JsonPropertyName("community_server_id")]
    public required ulong? LunarCommunityId { get; set; }

    [JsonPropertyName("dev_server_id")]
    public required ulong? LunarDevId { get; set; }
}
public class ServerState
{
    [JsonIgnore]
    public ConcurrentDictionary<ulong, RestBan> Bans = new ConcurrentDictionary<ulong, RestBan>();

    [JsonIgnore]
    public ConcurrentDictionary<ulong, RestMember> Members = new ConcurrentDictionary<ulong, RestMember>();

    [JsonPropertyName("apps")]
    public ConcurrentDictionary<ulong, RestApp> Apps = new ConcurrentDictionary<ulong, RestApp>();

    [JsonPropertyName("server")]
    public RestServer Server { get; set; }

    [JsonIgnore]
    public List<RestAuditLog> AuditLogs = new List<RestAuditLog>();

    [JsonPropertyName("channels")]
    public ConcurrentDictionary<ulong, RestChannel> Channels { get; set; } = new ConcurrentDictionary<ulong, RestChannel>();

    [JsonPropertyName("roles")]
    public ConcurrentDictionary<ulong, RestRole> Roles { get; set; } = new ConcurrentDictionary<ulong, RestRole>();

    [JsonPropertyName("emojis")]
    public ConcurrentDictionary<ulong, RestEmoji> Emojis { get; set; } = new ConcurrentDictionary<ulong, RestEmoji>();

    public void PushAuditLog(RestAuditLog auditLog)
    {
        if (AuditLogs.Count >= 101)
            AuditLogs.RemoveAt(100);

        AuditLogs.Add(auditLog);
    }

    public bool HasPermission(RestMember member, ServerPermission permission)
    {
        if (member.Id == Server.OwnerId || Server.DefaultPermissions.ServerPermissions.HasFlag(permission))
            return true;

        if (Server.DefaultPermissions.ServerPermissions.HasFlag(ServerPermission.Administrator))
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
        if (member.Id == Server.OwnerId || Server.DefaultPermissions.ModPermissions.HasFlag(permission))
            return true;

        if (Server.DefaultPermissions.ServerPermissions.HasFlag(ServerPermission.Administrator))
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
        if (member.Id == Server.OwnerId || Server.DefaultPermissions.ChannelPermissions.HasFlag(permission))
            return true;

        if (Server.DefaultPermissions.ServerPermissions.HasFlag(ServerPermission.Administrator))
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
        if (member.Id == Server.OwnerId || Server.DefaultPermissions.VoicePermissions.HasFlag(permission))
            return true;

        if (Server.DefaultPermissions.ServerPermissions.HasFlag(ServerPermission.Administrator))
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
}