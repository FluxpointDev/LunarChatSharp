using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events;

public class FriendDeleteEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "account_friend_delete";

    [JsonPropertyName("user_id")]
    public required string? UserId { get; set; }
}
