using LunarChatSharp.Rest.Users;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class RestBan
{
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("banned_at")]
    public required DateTime BannedAt { get; set; }

    [JsonPropertyName("action_user")]
    public required RestUser? ActionUser { get; set; }

    [JsonPropertyName("target_user")]
    public required RestUser? TargetUser { get; set; }

    [JsonPropertyName("expires_at")]
    public DateTime? ExpiresAt { get; set; }
}
