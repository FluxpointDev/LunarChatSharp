using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Accounts;

public class EditDisplayNameRequest : ILunarRequest
{
    [JsonPropertyName("display_name")]
    public required string? DisplayName { get; set; }
}
