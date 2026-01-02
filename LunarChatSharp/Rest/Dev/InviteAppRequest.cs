using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Dev;

public class InviteAppRequest : ILunarRequest
{
    [JsonPropertyName("app_id")]
    public required ulong AppId { get; set; }
}
