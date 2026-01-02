using LunarChatSharp.Rest.Messages;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Roles;

public class RestRole
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("icon_id")]
    public ulong? IconId { get; set; }

    public string? GetIconUrl()
    {
        if (!IconId.HasValue)
            return string.Empty;

        return Static.AttachmentUrl + $"{IconId}/role.webp";
    }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("hoist")]
    public bool Hoist { get; set; }

    [JsonPropertyName("position")]
    public int Position { get; set; }

    [JsonPropertyName("mentionable")]
    public bool Mentionable { get; set; }

    [JsonPropertyName("permissions")]
    public required RestPermissions Permissions { get; set; }

    [JsonPropertyName("managed_app_id")]
    public ulong? ManagedAppId { get; set; }

    [JsonIgnore]
    public RestAttachment? EditIcon;
}
