using LunarChatSharp.Rest.Messages;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Messages;

public class MessageCreateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "message_create";

    [JsonPropertyName("message")]
    public required RestMessage Message { get; set; }
}
