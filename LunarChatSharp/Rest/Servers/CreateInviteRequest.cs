using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class CreateInviteRequest : ILunarRequest
{
    [JsonPropertyName("max_age")]
    public int MaxAge { get; set; }

    [JsonPropertyName("max_uses")]
    public int MaxUses { get; set; }
}
