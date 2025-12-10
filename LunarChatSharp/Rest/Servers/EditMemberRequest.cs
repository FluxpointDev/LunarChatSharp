using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class EditMemberRequest : ILunarRequest
{
    [JsonPropertyName("nickname")]
    public string? Nickname { get; set; }

    [JsonPropertyName("timeout")]
    public DateTime? Timeout { get; set; }

    [JsonPropertyName("timeout_remove")]
    public bool TimeoutRemove { get; set; }
}
