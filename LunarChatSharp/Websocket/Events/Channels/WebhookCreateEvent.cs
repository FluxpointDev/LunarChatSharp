using LunarChatSharp.Rest.Webhooks;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Channels;

public class WebhookCreateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "webhook_create";

    [JsonPropertyName("webhook")]
    public required RestWebhook Webhook { get; set; }
}
