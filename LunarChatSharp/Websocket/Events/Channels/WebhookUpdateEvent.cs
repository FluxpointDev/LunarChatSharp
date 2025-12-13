using LunarChatSharp.Rest.Webhooks;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Channels;

public class WebhookUpdateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "webhook_update";

    [JsonPropertyName("webhook_id")]
    public required string? WebhookId { get; set; }

    [JsonPropertyName("channel_id")]
    public required string? ChannelId { get; set; }

    [JsonPropertyName("changed")]
    public EditWebhookRequest Changed { get; set; }
}
