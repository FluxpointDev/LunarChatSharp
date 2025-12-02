using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Roles;

public class RoleUpdateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "role_update";

    [JsonPropertyName("role_id")]
    public required string? RoleId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
