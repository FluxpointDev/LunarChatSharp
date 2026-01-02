using LunarChatSharp.Rest.Dev;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Servers;

public class AppUpdatedEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "app_update";

    [JsonPropertyName("server_id")]
    public ulong? ServerId { get; set; }

    [JsonPropertyName("group_id")]
    public ulong? GroupId { get; set; }

    [JsonPropertyName("app_id")]
    public required ulong AppId { get; set; }

    [JsonPropertyName("changed")]
    public EditAppRequest Changed { get; set; }
}
