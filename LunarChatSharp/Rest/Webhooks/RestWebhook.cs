using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Webhooks;

public class RestWebhook
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("token")]
    public required string Token { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("channel_id")]
    public required string ChannelId { get; set; }

    [JsonPropertyName("server_id")]
    public string ServerId { get; set; }

    [JsonPropertyName("created_by")]
    public required string? CreatedBy { get; set; }

    [JsonPropertyName("avatar_id")]
    public string? AvatarId { get; set; }

    public string? GetAvatarUrl()
    {
        if (string.IsNullOrEmpty(AvatarId))
            return string.Empty;

        return Static.AttachmentUrl + $"{AvatarId}/avatar.webp";
    }
}
