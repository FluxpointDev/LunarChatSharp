using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Dev;

public class CreateTeamRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}
