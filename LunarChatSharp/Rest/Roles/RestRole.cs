using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Roles;

public class RestRole
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("hoist")]
    public bool? Hoist { get; set; }

    [JsonPropertyName("position")]
    public int Position { get; set; }

    [JsonPropertyName("allow_mentions")]
    public bool? AllowMentions { get; set; }

    [JsonPropertyName("permissions")]
    public required RestPermissions Permissions { get; set; }
}
