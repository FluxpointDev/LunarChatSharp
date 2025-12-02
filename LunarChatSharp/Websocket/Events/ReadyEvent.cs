using LunarChatSharp.Rest.Channels;
using LunarChatSharp.Rest.Messages;
using LunarChatSharp.Rest.Roles;
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

    [JsonPropertyName("account")]
    public RestAccount? Account { get; set; }

    [JsonPropertyName("servers")]
    public ConcurrentDictionary<string, ServerState> Servers { get; set; }

    [JsonPropertyName("relations")]
    public Dictionary<string, RestRelation> Relations { get; set; }
}
public class ServerState
{
    [JsonPropertyName("server")]
    public RestServer Server { get; set; }

    [JsonIgnore]
    public ConcurrentDictionary<string, List<RestMessage>> Messages = new ConcurrentDictionary<string, List<RestMessage>>();

    [JsonPropertyName("channels")]
    public ConcurrentDictionary<string, RestChannel> Channels { get; set; } = new ConcurrentDictionary<string, RestChannel>();

    [JsonPropertyName("roles")]
    public ConcurrentDictionary<string, RestRole> Roles { get; set; } = new ConcurrentDictionary<string, RestRole>();

    [JsonPropertyName("emojis")]
    public ConcurrentDictionary<string, RestEmoji> Emojis { get; set; } = new ConcurrentDictionary<string, RestEmoji>();
}