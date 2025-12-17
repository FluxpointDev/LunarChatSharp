using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Servers;

public class AppRemoveEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "app_remove";

    [JsonPropertyName("server_id")]
    public string? ServerId { get; set; }

    [JsonPropertyName("group_id")]
    public string? GroupId { get; set; }

    [JsonPropertyName("app_id")]
    public required string? AppId { get; set; }
}
