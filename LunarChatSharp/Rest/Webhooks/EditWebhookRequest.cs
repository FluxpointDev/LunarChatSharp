using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Webhooks;

public class EditWebhookRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
