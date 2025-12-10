using LunarChatSharp.Rest.Servers;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Members;

public class MemberKickEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "member_kick";

    [JsonPropertyName("server_id")]
    public required string? ServerId { get; set; }

    [JsonPropertyName("member")]
    public required RestMember? Member { get; set; }
}
