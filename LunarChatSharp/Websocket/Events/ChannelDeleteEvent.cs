using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events;

public class ChannelDeleteEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "channel_delete";

    [JsonPropertyName("server_id")]
    public string? ServerId { get; set; }

    [JsonPropertyName("channel_id")]
    public required string? ChannelId { get; set; }
}
