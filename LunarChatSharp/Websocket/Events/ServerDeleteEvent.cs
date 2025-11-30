using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events;

public class ServerDeleteEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "server_delete";

    [JsonPropertyName("server_id")]
    public required string? ServerId { get; set; }
}
