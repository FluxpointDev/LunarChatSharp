using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events;

public class BlockDeleteEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "account_block_delete";

    [JsonPropertyName("user_id")]
    public required string? UserId { get; set; }
}
