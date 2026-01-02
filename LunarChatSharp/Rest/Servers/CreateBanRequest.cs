using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class CreateBanRequest : ReasonRequest
{
    [JsonPropertyName("max_days")]
    public int? MaxDays { get; set; }
}
