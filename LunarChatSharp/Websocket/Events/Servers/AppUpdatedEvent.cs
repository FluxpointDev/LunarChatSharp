using LunarChatSharp.Rest.Dev;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Servers;

public class AppUpdatedEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "app_update";

    [JsonPropertyName("server_id")]
    public required string? ServerId { get; set; }

    [JsonPropertyName("app_id")]
    public required string? AppId { get; set; }

    [JsonPropertyName("changed")]
    public CreateAppRequest? Changed { get; set; }
}
