using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Groups;

public class GroupRemoveUserEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "group_user_remove";

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("user_id")]
    public required ulong UserId { get; set; }
}
