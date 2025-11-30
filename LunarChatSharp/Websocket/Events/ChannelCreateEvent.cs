using LunarChatSharp.Rest.Channels;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events;

public class ChannelCreateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "channel_create";

    [JsonPropertyName("channel")]
    public required RestChannel? Channel { get; set; }
}
