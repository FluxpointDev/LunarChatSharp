using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Account;

public class RelationDeleteEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "account_relation_delete";

    [JsonPropertyName("user_id")]
    public required string? UserId { get; set; }
}
