using LunarChatSharp.Rest.Users;
using LunarChatSharp.Websocket.Events;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class RestAuditLog
{
    [JsonPropertyName("server_id")]
    public required ulong ServerId { get; set; }

    [JsonPropertyName("target_id")]
    public required ulong TargetId { get; set; }

    [JsonPropertyName("target_name")]
    public required string TargetName { get; set; }

    [JsonPropertyName("target_type")]
    public required TargetType? TargetType { get; set; }

    [JsonPropertyName("user_id")]
    public required ulong UserId { get; set; }

    [JsonPropertyName("user_name")]
    public required string UserName { get; set; }

    [JsonPropertyName("action_type")]
    public ActionType ActionType { get; set; }

    [JsonPropertyName("changes")]
    public List<AuditLogChange> Changes { get; set; }

    [JsonPropertyName("action_at")]
    public required DateTime ActionAt { get; set; }

    public static RestAuditLog Create(ServerState server, RestUser currentUser, TargetType targetType, ActionType actionType, ulong targetId, string targetName)
    {
        return new RestAuditLog
        {
            ServerId = server.Server.Id,
            ActionType = actionType,
            TargetId = targetId,
            TargetName = targetName,
            TargetType = targetType,
            UserId = currentUser.Id,
            UserName = currentUser.GetCurrentNameDiscrim(),
            Changes = new List<AuditLogChange>(),
            ActionAt = DateTime.UtcNow
        };
    }

    public void AddChange(PropertyType property, string oldValue, string newValue)
    {
        Changes.Add(new AuditLogChange
        {
            OldValue = oldValue,
            NewValue = newValue,
            PropertyType = property
        });
    }

}
public enum ActionType
{
    ServerUpdate, ChannelCreated, ChannelUpdated, ChannelDeleted, MemberBanned, MemberUpdate, MemberUnbanned, MemberKicked, AppAdded, AppRemoved,
    RoleCreated, RoleUpdated, RoleDeleted, InviteCreated, InviteDeleted, WebhookCreated, WebhookUpdated, WebhookDeleted, EmojiCreated, EmojiUpdated,
    EmojiDeleted, BulkMessagesDeleted, MessagePinned, MessageUnpinned
}
public enum TargetType
{
    Server, Member, Invite, Role, Channel, Webhook, Emoji, Message, App,
}
public class AuditLogChange
{
    [JsonPropertyName("old_value")]
    public string? OldValue { get; set; }

    [JsonPropertyName("new_value")]
    public string? NewValue { get; set; }

    [JsonPropertyName("property_type")]
    public required PropertyType PropertyType;
}
public enum PropertyType
{
    Name, Description
}