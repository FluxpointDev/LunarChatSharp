using LunarChatSharp.Rest.Users;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Messages;

public class RestReaction
{
    [JsonPropertyName("count")]
    public required int Count { get; set; }

    [JsonPropertyName("emoji")]
    public required RestEmoji Emoji { get; set; }

    [JsonIgnore]
    public ConcurrentDictionary<string, RestUser> Users = new ConcurrentDictionary<string, RestUser>();
}
