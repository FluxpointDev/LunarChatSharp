using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Messages;

public class RestAttachment
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("file_name")]
    public required string FileName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("is_spoiler")]
    public bool IsSpoiler { get; set; }

    [JsonPropertyName("channel_id")]
    public ulong? ChannelId { get; set; }

    [JsonPropertyName("server_id")]
    public ulong? ServerId { get; set; }

    [JsonPropertyName("user_id")]
    public required ulong UserId { get; set; }

    [JsonIgnore]
    public byte[] Image;
}
