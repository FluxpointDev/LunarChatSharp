using LunarChatSharp.Core.Channels;
using LunarChatSharp.Rest.Dev;
using LunarChatSharp.Rest.Users;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Channels;

public class RestChannel
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public ChannelType Type { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("server_id")]
    public string? ServerId { get; set; }

    [JsonPropertyName("users")]
    public HashSet<RestUser>? Users { get; set; }

    [JsonPropertyName("group_settings")]
    public RestGroupSettings? GroupSettings { get; set; }

    [JsonPropertyName("is_nsfw")]
    public bool IsNsfw { get; set; }
}
public class RestGroupSettings
{
    [JsonPropertyName("owner_id")]
    public string? OwnerId { get; set; }

    [JsonPropertyName("apps")]
    public ConcurrentDictionary<string, RestApp> Apps = new ConcurrentDictionary<string, RestApp>();
}
