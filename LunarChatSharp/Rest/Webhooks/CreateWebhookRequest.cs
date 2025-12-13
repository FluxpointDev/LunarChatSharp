using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Webhooks;

public class CreateWebhookRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}
