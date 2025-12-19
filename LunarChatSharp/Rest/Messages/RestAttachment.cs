using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Messages;

public class RestAttachment
{
    [JsonPropertyName("id")]
    public required string? Id { get; set; }

    [JsonPropertyName("file_name")]
    public required string? FileName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("is_spoiler")]
    public bool IsSpoiler { get; set; }

    [JsonPropertyName("channel_id")]
    public string? ChannelId { get; set; }

    [JsonPropertyName("server_id")]
    public string ServerId { get; set; }

    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    [JsonIgnore]
    public byte[] Image;
}
