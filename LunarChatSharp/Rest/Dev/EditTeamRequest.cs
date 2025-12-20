using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Dev;

public class EditTeamRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }
}
