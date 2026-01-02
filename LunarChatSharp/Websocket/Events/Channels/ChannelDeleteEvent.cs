using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Channels;

public class ChannelDeleteEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "channel_delete";

    [JsonPropertyName("server_id")]
    public ulong? ServerId { get; set; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }
}
