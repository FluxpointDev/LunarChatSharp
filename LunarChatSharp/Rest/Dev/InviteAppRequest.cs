using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Dev;

public class InviteAppRequest : ILunarRequest
{
    [JsonPropertyName("app_id")]
    public string? AppId { get; set; }
}
