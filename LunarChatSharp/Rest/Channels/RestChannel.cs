using LunarChatSharp.Core.Channels;
using LunarChatSharp.Rest.Dev;
using LunarChatSharp.Rest.Users;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Channels;

public class RestChannel
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public ChannelType Type { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("archive_at")]
    public DateTime? ArchiveAt { get; set; }

    [JsonPropertyName("parent_id")]
    public ulong? ParentId { get; set; }

    [JsonPropertyName("position")]
    public int Position { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("server_id")]
    public ulong? ServerId { get; set; }

    [JsonPropertyName("users")]
    public HashSet<RestUser>? Users { get; set; }

    [JsonPropertyName("group_settings")]
    public RestGroupSettings? GroupSettings { get; set; }

    [JsonPropertyName("is_nsfw")]
    public bool IsNsfw { get; set; }

    public string GetFallback()
    {
        string[] Split = Name.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (Split.Length == 0)
            return null!;

        if (Split.Length == 1)
            return Split[0].ToUpper()[0].ToString();

        return $"{Split[0].ToUpper()[0]}{Split.Last().ToUpper()[0]}";
    }
}
public class RestGroupSettings
{
    [JsonPropertyName("owner_id")]
    public ulong? OwnerId { get; set; }

    [JsonPropertyName("icon_id")]
    public ulong? IconId { get; set; }

    public string? GetIconUrl()
    {
        if (!IconId.HasValue)
            return string.Empty;

        return Static.AttachmentUrl + $"{IconId}/group.webp";
    }

    [JsonPropertyName("apps")]
    public ConcurrentDictionary<ulong, RestApp> Apps = new ConcurrentDictionary<ulong, RestApp>();
}
