using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Servers;

public class InviteDeleteEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "invite_delete";

    [JsonPropertyName("server_id")]
    public required ulong ServerId { get; set; }

    [JsonPropertyName("invite_code")]
    public required string Code { get; set; }
}
