using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class EditServerRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
