using LunarChatSharp.Rest.Users;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events;

public class BlockCreateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "account_block_create";

    [JsonPropertyName("relation")]
    public required RestRelation? Relation { get; set; }

}