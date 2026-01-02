using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Channels;

public class UpdateChannelRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("owner_id")]
    public ulong? OwnerId { get; set; }

    [JsonPropertyName("parent_id")]
    public ulong? ParentId { get; set; }

    [JsonPropertyName("position")]
    public int? Position { get; set; }
}
