using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Users;

public class RestRelation
{
    [JsonPropertyName("user_id")]
    public required string UserId { get; set; }

    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("note")]
    public string? Note { get; set; }
}
