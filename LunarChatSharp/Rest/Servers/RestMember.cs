using LunarChatSharp.Rest.Users;
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

    [JsonPropertyName("joined_at")]
    public required DateTime? JoinedAt { get; set; }

    [JsonPropertyName("user")]
    public required RestUser? User { get; set; }

    public string GetCurrentName()
    {
        return (Nickname ?? User.DisplayName ?? User.Username);
    }

    public string GetCurrentNameDiscrim()
    {
        return (Nickname ?? User.DisplayName ?? User.Username) + (User.IsBot ? "#" + User.Discriminator : null);
    }
}
