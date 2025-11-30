using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Messages;

public class RestEmoji
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime? CreatedAt { get; set; }
}
