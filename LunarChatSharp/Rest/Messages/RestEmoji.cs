using LunarChatSharp.Core.Messages;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Messages;

public class RestEmoji
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("icon_id")]
    public required ulong IconId { get; set; }

    public string? GetIconUrl()
    {
        if (IconId == 0)
            return string.Empty;

        return Static.AttachmentUrl + $"{IconId}/emoji.webp";
    }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("created_by")]
    public required ulong CreatedBy { get; set; }

    [JsonPropertyName("source_id")]
    public required ulong SourceId { get; set; }

    [JsonPropertyName("source_type")]
    public required EmojiSourceType? SourceType { get; set; }

    [JsonPropertyName("is_animated")]
    public bool IsAnimated { get; set; }
}
