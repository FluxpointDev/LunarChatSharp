using LunarChatSharp.Rest.Users;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Members;

public class MemberUnbanEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "member_unban";

    [JsonPropertyName("server_id")]
    public required ulong ServerId { get; set; }

    [JsonPropertyName("user")]
    public required RestUser User { get; set; }
}
