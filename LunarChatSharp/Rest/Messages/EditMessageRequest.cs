using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Messages;

public class EditMessageRequest : ILunarRequest
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("embeds")]
    public RestEmbed[]? Embeds { get; set; }
}
