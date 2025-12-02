using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Channels;

public class UpdateChannelRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }
}
