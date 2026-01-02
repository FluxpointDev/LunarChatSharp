using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Roles;

public class RoleDeleteEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "role_delete";

    [JsonPropertyName("server_id")]
    public required ulong ServerId { get; set; }

    [JsonPropertyName("role_id")]
    public required ulong RoleId { get; set; }

    [JsonPropertyName("positions")]
    public Dictionary<ulong, int> Positions { get; set; }
}
