using LunarChatSharp.Rest.Users;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Groups;

public class GroupAddUserEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "group_user_add";

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("user")]
    public required RestUser User { get; set; }
}
