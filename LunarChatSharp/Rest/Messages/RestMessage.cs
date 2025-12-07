using LunarChatSharp.Core.Messages;
using LunarChatSharp.Rest.Users;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Messages;

public class RestMessage
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("channel_id")]
    public required string ChannelId { get; set; }

    [JsonPropertyName("author")]
    public RestUser? Author { get; set; }

    [JsonPropertyName("source")]
    public MessageSourceType Source { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("system_message")]
    public SystemMessageType? SystemMessage { get; set; }
}
