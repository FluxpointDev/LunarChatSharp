using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Accounts;

public class EditUsernameRequest : ILunarRequest
{
    [JsonPropertyName("username")]
    public required string Username { get; set; }
}
