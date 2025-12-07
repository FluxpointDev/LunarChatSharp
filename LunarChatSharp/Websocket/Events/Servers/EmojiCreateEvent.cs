using LunarChatSharp.Rest.Messages;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Servers;

public class EmojiCreateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "emoji_create";

    [JsonPropertyName("server_id")]
    public required string? ServerId { get; set; }

    [JsonPropertyName("emoji")]
    public required RestEmoji? Emoji { get; set; }
}
