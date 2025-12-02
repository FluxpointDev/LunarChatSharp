using LunarChatSharp.Rest.Channels;
using LunarChatSharp.Rest.Servers;
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
    public required Dictionary<string, RestChannel> Channels { get; set; }
}
