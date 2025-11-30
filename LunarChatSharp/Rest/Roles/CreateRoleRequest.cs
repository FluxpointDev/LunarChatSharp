using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Roles;

public class CreateRoleRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}
