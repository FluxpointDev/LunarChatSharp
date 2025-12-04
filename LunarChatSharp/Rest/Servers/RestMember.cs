using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class RestMember
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("server_id")]
    public required string ServerId { get; set; }

    [JsonPropertyName("nickname")]
    public string? Nickname { get; set; }

    [JsonPropertyName("roles")]
    public required HashSet<string>? Roles { get; set; }

    [JsonPropertyName("timeout")]
    public DateTime? Timeout { get; set; }

}
