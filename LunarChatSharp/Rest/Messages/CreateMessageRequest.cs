using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Messages;

public class CreateMessageRequest : ILunarRequest
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}
