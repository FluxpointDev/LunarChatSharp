using LunarChatSharp.Core.Messages;
using LunarChatSharp.Rest.Users;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Messages;

public class RestMessage
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

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

    [JsonPropertyName("embeds")]
    public RestEmbed[]? Embeds { get; set; }

    [JsonPropertyName("attachments")]
    public RestAttachment[]? Attachments { get; set; }

    [JsonPropertyName("reactions")]
    public ConcurrentDictionary<ulong, RestReaction>? Reactions { get; set; }
}
