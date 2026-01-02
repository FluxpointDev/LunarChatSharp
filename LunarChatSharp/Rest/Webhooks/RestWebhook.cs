using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Webhooks;

public class RestWebhook
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("server_id")]
    public ulong? ServerId { get; set; }

    [JsonPropertyName("created_by")]
    public required ulong CreatedBy { get; set; }

    [JsonPropertyName("avatar_id")]
    public ulong? AvatarId { get; set; }

    public string? GetAvatarUrl()
    {
        if (!AvatarId.HasValue)
            return string.Empty;

        return Static.AttachmentUrl + $"{AvatarId}/avatar.webp";
    }
}
