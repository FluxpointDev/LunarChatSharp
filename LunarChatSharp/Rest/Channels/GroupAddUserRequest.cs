using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Channels;

public class GroupAddUserRequest : ILunarRequest
{
    [JsonPropertyName("user_id")]
    public required ulong UserId { get; set; }
}
