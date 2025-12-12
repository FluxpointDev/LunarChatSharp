using LunarChatSharp.Core.Channels;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Channels;

public class CreateChannelRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public ChannelType Type { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("server_id")]
    public string ServerId { get; set; }

    [JsonPropertyName("users")]
    public string[]? Users { get; set; }
}
