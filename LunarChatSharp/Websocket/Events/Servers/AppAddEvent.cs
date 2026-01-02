using LunarChatSharp.Rest.Dev;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Servers;

public class AppAddEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "app_add";

    [JsonPropertyName("server_id")]
    public ulong? ServerId { get; set; }

    [JsonPropertyName("group_id")]
    public ulong? GroupId { get; set; }

    [JsonPropertyName("app")]
    public required RestApp App { get; set; }
}
