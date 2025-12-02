using LunarChatSharp.Rest.Users;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Messages;

public class MessageUpdateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "message_update";

    [JsonPropertyName("id")]
    public required string? Id { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("channel_id")]
    public required string? ChannelId { get; set; }

    [JsonPropertyName("author")]
    public required RestUser? Author { get; set; }

    [JsonPropertyName("updated_at")]
    public required DateTime? UpdatedAt { get; set; }
}
