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

    [JsonPropertyName("about_me")]
    public string? AboutMe { get; set; }

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("avatar_id")]
    public string? AvatarId { get; set; }

    public string? GetAvatarUrl()
    {
        if (string.IsNullOrEmpty(AvatarId))
            return string.Empty;

        return Static.AttachmentUrl + $"{AvatarId}/avatar.webp";
    }

    [JsonPropertyName("badges")]
    public UserBadgeType? Badges { get; set; }

    [JsonPropertyName("presence")]
    public required RestUserPresence Presence { get; set; }

    [JsonPropertyName("is_bot")]
    public bool IsBot { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    public string GetCurrentName()
    {
        return (DisplayName ?? Username);
    }

    public string GetCurrentNameDiscrim()
    {
        return (DisplayName ?? Username) + (IsBot ? "#" + Discriminator : null);
    }

    public string GetFallback()
    {
        string[] Split = GetCurrentName().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (!Split.Any())
            return null!;

        if (Split.Length == 1)
            return Split[0].ToUpper()[0].ToString();

        return $"{Split[0].ToUpper()[0]}{Split.Last().ToUpper()[0]}";
    }
}
