using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Dev;

public class EditAppRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("is_public")]
    public bool? IsPublic { get; set; }

    [JsonPropertyName("website")]
    public string? Website { get; set; }

    [JsonPropertyName("terms")]
    public string? Terms { get; set; }

    [JsonPropertyName("privacy")]
    public string? Privacy { get; set; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }
}