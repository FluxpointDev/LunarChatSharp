using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Messages;


public class CreateMessageRequest : ILunarRequest
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("embeds")]
    public RestEmbed[]? Embeds { get; set; }

    [JsonPropertyName("attachments")]
    public CreateAttachmentRequest[]? Attachments { get; set; }

    internal bool IsSerialized;
}
