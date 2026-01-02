using LunarChatSharp.Rest.Servers;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Members;

public class MemberJoinEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "member_join";

    [JsonPropertyName("server_id")]
    public required ulong ServerId { get; set; }

    [JsonPropertyName("member")]
    public required RestMember Member { get; set; }
}
