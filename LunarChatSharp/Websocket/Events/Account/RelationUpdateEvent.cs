using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Account;

public class RelationUpdateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "account_relation_update";

    [JsonPropertyName("user_id")]
    public required string? UserId { get; set; }

    [JsonPropertyName("note")]
    public string? Note { get; set; }

    [JsonPropertyName("is_important")]
    public bool? IsImportant { get; set; }

    [JsonPropertyName("is_pinned")]
    public bool? IsPinned { get; set; }
}
