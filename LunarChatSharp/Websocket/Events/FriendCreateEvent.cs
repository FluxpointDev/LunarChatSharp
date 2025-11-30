using LunarChatSharp.Rest.Users;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events;

public class FriendCreateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "account_friend_create";

    [JsonPropertyName("relation")]
    public required RestRelation? Relation { get; set; }

}