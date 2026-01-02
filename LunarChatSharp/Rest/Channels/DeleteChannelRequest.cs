using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Channels;

public class DeleteChannelRequest : ILunarRequest
{
    [JsonPropertyName("server_id")]
    public ulong? ServerId { get; set; }
}
