using LunarChatSharp.Rest.Dev;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Users;

public class RestDev
{
    [JsonPropertyName("apps")]
    public RestApp[] Apps { get; set; }

    [JsonPropertyName("teams")]
    public RestTeam[] Teams { get; set; }
}
