using LunarChatSharp.Rest.Channels;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Channels;

public class ChannelUpdateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "channel_update";

    [JsonPropertyName("changed")]
    public required UpdateChannelRequest? Changed { get; set; }

    [JsonPropertyName("server_id")]
    public string? ServerId { get; set; }

    [JsonPropertyName("channel_id")]
    public required string? ChannelId { get; set; }
}
