using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Accounts;

public class ChangeEmailRequest
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }
}
