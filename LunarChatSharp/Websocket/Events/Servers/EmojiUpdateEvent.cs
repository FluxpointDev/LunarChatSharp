using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Servers;

public class EmojiUpdateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "emoji_update";

    [JsonPropertyName("server_id")]
    public required string? ServerId { get; set; }

    [JsonPropertyName("emoji_id")]
    public required string? EmojiId { get; set; }

    [JsonPropertyName("name")]
    public required string? Name { get; set; }
}
