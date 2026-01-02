using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class RestServerSystemMessages
{
    [JsonPropertyName("member_joined")]
    public ulong? MemberJoined { get; set; }

    [JsonPropertyName("member_left")]
    public ulong? MemberLeft { get; set; }

    [JsonPropertyName("member_banned")]
    public ulong? MemberBanned { get; set; }

    [JsonPropertyName("member_unbanned")]
    public ulong? MemberUnbanned { get; set; }

    [JsonPropertyName("member_kicked")]
    public ulong? MemberKicked { get; set; }

    [JsonPropertyName("member_timedout")]
    public ulong? MemberTimedout { get; set; }

    [JsonPropertyName("app_added")]
    public ulong? AppAdded { get; set; }

    [JsonPropertyName("app_removed")]
    public ulong? AppRemoved { get; set; }
}
