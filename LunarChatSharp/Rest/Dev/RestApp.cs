using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Dev;

public class RestApp
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("owner_id")]
    public required ulong OwnerId { get; set; }

    [JsonPropertyName("avatar_id")]
    public ulong? AvatarId { get; set; }

    public string? GetAvatarUrl()
    {
        if (!AvatarId.HasValue)
            return string.Empty;

        return Static.AttachmentUrl + $"{AvatarId}/avatar.webp";
    }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("website_url")]
    public string? WebsiteUrl { get; set; }

    [JsonPropertyName("terms_url")]
    public string? TermsUrl { get; set; }

    [JsonPropertyName("privacy_url")]
    public string? PrivacyUrl { get; set; }

    [JsonPropertyName("is_public")]
    public bool IsPublic { get; set; }
}
