using LunarChatSharp.Rest.Channels;
using LunarChatSharp.Rest.Messages;
using LunarChatSharp.Rest.Servers;
using LunarChatSharp.Rest.Users;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events;

public class AuthEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "auth";

    [JsonPropertyName("user_id")]
    public required string UserId { get; set; }
}
public class ReadyEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "ready";

    [JsonPropertyName("servers")]
    public RestServer[] Servers { get; set; }

    [JsonPropertyName("channels")]
    public ConcurrentDictionary<string, List<RestChannel>> Channels { get; set; }

    [JsonPropertyName("emojis")]
    public RestEmoji[] Emojis { get; set; }

    [JsonPropertyName("relations")]
    public Dictionary<string, RestRelation> Relations { get; set; }
}

