using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class RestServerSystemMessages
{
    [JsonPropertyName("member_joined")]
    public string? MemberJoined { get; set; }

    [JsonPropertyName("member_left")]
    public string? MemberLeft { get; set; }

    [JsonPropertyName("member_banned")]
    public string? MemberBanned { get; set; }

    [JsonPropertyName("member_unbanned")]
    public string? MemberUnbanned { get; set; }

    [JsonPropertyName("member_kicked")]
    public string? MemberKicked { get; set; }

    [JsonPropertyName("member_timedout")]
    public string? MemberTimedout { get; set; }

    [JsonPropertyName("app_added")]
    public string? AppAdded { get; set; }

    [JsonPropertyName("app_removed")]
    public string? AppRemoved { get; set; }
}
