using LunarChatSharp.Core.Users;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Users;

public class RestUser
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("discriminator")]
    public short? Discriminator { get; set; }

    [JsonPropertyName("profile")]
    public UserProfile? Profile { get; set; }

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("avatar_id")]
    public ulong? AvatarId { get; set; }

    public string? GetAvatarUrl()
    {
        if (!AvatarId.HasValue)
            return string.Empty;

        return Static.AttachmentUrl + $"{AvatarId}/avatar.webp";
    }

    [JsonPropertyName("badges")]
    public UserBadgeType? Badges { get; set; }

    [JsonPropertyName("presence")]
    public RestUserPresence Presence { get; set; }

    [JsonPropertyName("is_bot")]
    public bool IsBot { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

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
        if (Split.Length == 0)
            return null!;

        if (Split.Length == 1)
            return Split[0].ToUpper()[0].ToString();

        return $"{Split[0].ToUpper()[0]}{Split.Last().ToUpper()[0]}";
    }
}
public class UserProfile
{
    [JsonPropertyName("about_me")]
    public string? AboutMe { get; set; }
}