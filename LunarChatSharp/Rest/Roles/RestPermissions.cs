using LunarChatSharp.Core.Servers;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Roles;

public class RestPermissions
{
    [JsonPropertyName("server")]
    public ServerPermission ServerPermission { get; set; }

    [JsonPropertyName("mod")]
    public ModPermission ModPermission { get; set; }

    [JsonPropertyName("channel")]
    public ChannelPermission ChannelPermission { get; set; }

    [JsonPropertyName("voice")]
    public VoicePermission VoicePermission { get; set; }
}
