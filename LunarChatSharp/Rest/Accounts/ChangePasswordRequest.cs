using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Accounts;

public class ChangePasswordRequest : ILunarRequest
{
    [JsonPropertyName("password")]
    public required string Password { get; set; }
}
