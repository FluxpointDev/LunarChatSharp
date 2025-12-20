using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Dev;

public class RestApp
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("owner_id")]
    public required string OwnerId { get; set; }

    [JsonPropertyName("avatar_id")]
    public string? AvatarId { get; set; }

    public string? GetAvatarUrl()
    {
        if (string.IsNullOrEmpty(AvatarId))
            return string.Empty;

        return Static.AttachmentUrl + $"{AvatarId}/avatar.webp";
    }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("website")]
    public string? Website { get; set; }

    [JsonPropertyName("terms")]
    public string? Terms { get; set; }

    [JsonPropertyName("privacy")]
    public string? Privacy { get; set; }

    [JsonPropertyName("is_public")]
    public bool? IsPublic { get; set; }
}
