using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Messages;

public class ReactionRemoveAllEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "reaction_remove_all";

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("message_id")]
    public required ulong MessageId { get; set; }

    [JsonPropertyName("server_id")]
    public ulong? ServerId { get; set; }
}
