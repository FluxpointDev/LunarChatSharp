using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket;

public interface ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}
