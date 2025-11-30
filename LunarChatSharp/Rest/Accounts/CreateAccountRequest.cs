using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Accounts;

public class CreateAccountRequest : ILunarRequest
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }
}
public class CreateDemoAccountRequest : ILunarRequest
{
    [JsonPropertyName("username")]
    public required string Username { get; set; }
}
