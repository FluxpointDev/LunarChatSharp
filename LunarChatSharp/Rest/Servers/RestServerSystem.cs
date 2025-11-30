using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class RestServerSystemMessages
{
    [JsonPropertyName("user_joined")]
    public string? UserJoined { get; set; }

    [JsonPropertyName("user_left")]
    public string? UserLeft { get; set; }

    [JsonPropertyName("user_banned")]
    public string? UserBanned { get; set; }

    [JsonPropertyName("user_kicked")]
    public string? UserKicked { get; set; }

    [JsonPropertyName("user_timedout")]
    public string? UserTimedout { get; set; }
}
