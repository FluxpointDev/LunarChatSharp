using LunarChatSharp.Rest.Messages;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Messages;

public class ReactionAddEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "reaction_add";

    [JsonPropertyName("emoji")]
    public required RestEmoji Emoji { get; set; }

    [JsonPropertyName("channel_id")]
    public required string ChannelId { get; set; }

    [JsonPropertyName("user_id")]
    public required string UserId { get; set; }

    [JsonPropertyName("message_id")]
    public required string MessageId { get; set; }

    [JsonPropertyName("server_id")]
    public string? ServerId { get; set; }
}
