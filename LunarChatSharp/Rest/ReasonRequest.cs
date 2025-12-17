using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest;

public class ReasonRequest : ILunarRequest
{
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}
