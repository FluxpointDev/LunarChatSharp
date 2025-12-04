using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Servers;

public class MemberUpdateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "member_update";

    [JsonPropertyName("roles")]
    public List<string>? Roles { get; set; }
}
