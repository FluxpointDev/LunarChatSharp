using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Accounts;

public class LoginRequest : ILunarRequest
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }
}
