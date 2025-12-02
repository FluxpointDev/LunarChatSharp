using LunarChatSharp.Rest.Messages;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Messages;

public class MessageDeleteEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "message_delete";

    [JsonPropertyName("message")]
    public required RestMessage? Message { get; set; }
}
