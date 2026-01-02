using LunarChatSharp.Rest.Servers;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Servers;

public class ServerUpdateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "server_update";

    [JsonPropertyName("server_id")]
    public required ulong ServerId { get; set; }

    [JsonPropertyName("changed")]
    public required EditServerRequest Changed { get; set; }
}
