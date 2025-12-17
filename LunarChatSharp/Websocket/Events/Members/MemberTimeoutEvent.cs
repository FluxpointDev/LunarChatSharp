using LunarChatSharp.Rest.Servers;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Members;

public class MemberTimeoutEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "member_timeout";

    [JsonPropertyName("server_id")]
    public required string? ServerId { get; set; }

    [JsonPropertyName("member")]
    public required RestMember? Member { get; set; }

    [JsonPropertyName("timeout")]
    public required DateTime? Timeout { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}
