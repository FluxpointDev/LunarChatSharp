using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Channels;

public class WebhookDeleteEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "webhook_delete";

    [JsonPropertyName("webhook_id")]
    public required ulong WebhookId { get; set; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }
}
