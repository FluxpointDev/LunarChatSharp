using LunarChatSharp.Core.Users;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Users;

public class RestUserPresence
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("status")]
    public required UserStatusType Status { get; set; }
}
