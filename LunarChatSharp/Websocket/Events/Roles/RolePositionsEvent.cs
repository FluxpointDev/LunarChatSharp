using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Roles;

public class RolePositionsEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "role_positions";

    [JsonPropertyName("server_id")]
    public string ServerId { get; set; }

    [JsonPropertyName("positions")]
    public Dictionary<string, int> Positions { get; set; }
}
