using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Roles;

public class RolePositionsEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "role_positions";

    [JsonPropertyName("server_id")]
    public required ulong ServerId { get; set; }

    [JsonPropertyName("positions")]
    public required Dictionary<ulong, int> Positions { get; set; }
}
