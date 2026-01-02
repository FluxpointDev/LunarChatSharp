using LunarChatSharp.Rest.Roles;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Roles;

public class RoleUpdateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "role_update";

    [JsonPropertyName("server_id")]
    public required ulong ServerId { get; set; }

    [JsonPropertyName("role_id")]
    public required ulong RoleId { get; set; }

    [JsonPropertyName("changed")]
    public required EditRoleRequest Changed { get; set; }
}
