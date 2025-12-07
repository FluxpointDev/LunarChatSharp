using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class EditEmojiRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
