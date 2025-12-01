using LunarChatSharp.Rest.Users;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events;

public class RelationCreateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "account_relation_create";

    [JsonPropertyName("relation")]
    public required RestRelation? Relation { get; set; }

}