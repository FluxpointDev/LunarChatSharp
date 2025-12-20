using LunarChatSharp.Rest.Channels;
using LunarChatSharp.Rest.Dev;
using LunarChatSharp.Rest.Messages;
using LunarChatSharp.Rest.Roles;
using LunarChatSharp.Rest.Servers;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Servers;

public class ServerCreateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "server_create";

    [JsonPropertyName("server")]
    public required RestServer? Server { get; set; }

    [JsonPropertyName("member")]
    public required RestMember? Member { get; set; }

    [JsonPropertyName("channels")]
    public required ConcurrentDictionary<string, RestChannel> Channels { get; set; }

    [JsonPropertyName("roles")]
    public required ConcurrentDictionary<string, RestRole> Roles { get; set; }

    [JsonPropertyName("emojis")]
    public required ConcurrentDictionary<string, RestEmoji> Emojis { get; set; }

    [JsonPropertyName("apps")]
    public required ConcurrentDictionary<string, RestApp> Apps { get; set; }
}
