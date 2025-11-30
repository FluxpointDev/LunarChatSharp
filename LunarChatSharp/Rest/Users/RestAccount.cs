using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Users;

public class RestAccount
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }
}
