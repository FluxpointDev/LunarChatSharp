using LunarChatSharp.Core.Users;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Users;

public class RestUser
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("discriminator")]
    public required string? Discriminator { get; set; }

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("badges")]
    public UserBadgeType? Badges { get; set; }

    [JsonPropertyName("presence")]
    public required RestUserPresence Presence { get; set; }

    [JsonPropertyName("is_bot")]
    public bool IsBot { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }
}
