using LunarChatSharp.Rest.Channels;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Servers;

public class InviteCreateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "invite_create";

    [JsonPropertyName("server_id")]
    public required string ServerId { get; set; }

    [JsonPropertyName("invite")]
    public required RestInvite? Invite { get; set; }
}
