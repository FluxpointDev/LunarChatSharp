using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Accounts;

public class ChangeEmailRequest : ILunarRequest
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }
}
