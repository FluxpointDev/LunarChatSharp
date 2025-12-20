using LunarChatSharp.Core.Users;
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

    [JsonPropertyName("type")]
    public UserRelationType Type { get; set; }

    [JsonPropertyName("request_by")]
    public string? RequestBy { get; set; }

    [JsonPropertyName("is_important")]
    public bool? IsImportant { get; set; }

    [JsonPropertyName("is_pinned")]
    public bool? IsPinned { get; set; }
}
