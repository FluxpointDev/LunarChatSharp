using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Servers;

public class EmojiDeleteEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "emoji_delete";

    [JsonPropertyName("server_id")]
    public required string? ServerId { get; set; }

    [JsonPropertyName("emoji_id")]
    public required string? EmojiId { get; set; }
}
