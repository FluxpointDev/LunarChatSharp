using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Roles;

public class RolePositionsEvents : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "role_positions";

    [JsonPropertyName("roles")]
    public Dictionary<string, int> Roles { get; set; }
}
