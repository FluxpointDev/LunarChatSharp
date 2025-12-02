using LunarChatSharp.Rest.Roles;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Roles;

internal class RoleCreateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "role_create";

    [JsonPropertyName("server_id")]
    public string ServerId { get; set; }

    [JsonPropertyName("role")]
    public required RestRole? Role { get; set; }
}
