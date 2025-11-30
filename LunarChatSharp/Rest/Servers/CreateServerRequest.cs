using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class CreateServerRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}
