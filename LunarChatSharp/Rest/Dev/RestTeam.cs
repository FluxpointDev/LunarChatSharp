using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Dev;

public class RestTeam
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("owner_id")]
    public required ulong OwnerId { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("icon_id")]
    public ulong? IconId { get; set; }

    public string? GetIconUrl()
    {
        if (!IconId.HasValue)
            return string.Empty;

        return Static.AttachmentUrl + $"{IconId}/icon.webp";
    }
}
