using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Channels;

public class DeleteChannelRequest : ILunarRequest
{
    [JsonPropertyName("server_id")]
    public string? ServerId { get; set; }
}
