using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Roles;

public class EditRoleRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }
}
