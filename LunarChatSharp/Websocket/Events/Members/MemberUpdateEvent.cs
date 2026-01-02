using LunarChatSharp.Rest.Servers;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Members;

public class MemberUpdateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "member_update";

    [JsonPropertyName("server_id")]
    public required ulong ServerId { get; set; }

    [JsonPropertyName("user_id")]
    public required ulong UserId { get; set; }

    [JsonPropertyName("data")]
    public required EditMemberRequest Changed { get; set; }
}
