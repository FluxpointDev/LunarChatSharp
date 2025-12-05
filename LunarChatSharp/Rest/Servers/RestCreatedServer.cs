using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class RestCreatedServer
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}
