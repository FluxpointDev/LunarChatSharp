using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Messages;

public class RestEmoji
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("icon_id")]
    public required string IconId { get; set; }

    public string? GetIconUrl()
    {
        if (string.IsNullOrEmpty(IconId))
            return string.Empty;

        return Static.AttachmentUrl + $"{IconId}/emoji.webp";
    }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("created_by")]
    public required string CreatedBy { get; set; }
}
