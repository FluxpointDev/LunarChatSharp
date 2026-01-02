using LunarChatSharp.Rest.Messages;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Messages;

public class MessageUpdateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "message_update";

    [JsonPropertyName("message_id")]
    public required ulong MessageId { get; set; }

    [JsonPropertyName("changed")]
    public required EditMessageRequest? Changed { get; set; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("updated_at")]
    public required DateTime? UpdatedAt { get; set; }
}
