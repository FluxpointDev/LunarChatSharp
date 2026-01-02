using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Roles;

public class CreateRoleRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("permissions")]
    public RestPermissions? Permissions { get; set; }

    [JsonPropertyName("hoist")]
    public bool? Hoist { get; set; }

    [JsonPropertyName("mentionable")]
    public bool? Mentionable { get; set; }
}
