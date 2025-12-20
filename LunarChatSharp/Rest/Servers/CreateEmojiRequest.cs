using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class CreateEmojiRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("icon")]
    public required string Icon { get; set; }
}