using LunarChatSharp.Rest.Roles;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class EditServerRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("default_permissions")]
    public RestPermissions? DefaultPermissions { get; set; }

    [JsonPropertyName("system_messages")]
    public RestServerSystemMessages? SystemMessages { get; set; }

    [JsonPropertyName("is_discoverable")]
    public bool? IsDiscoverable { get; set; }

    [JsonPropertyName("vanity_invite")]
    public string? VanityInvite { get; set; }

    [JsonPropertyName("owner_id")]
    public string? OwnerId { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("banner")]
    public string? Banner { get; set; }

    public string? GetIconUrl()
    {
        if (string.IsNullOrEmpty(Icon))
            return string.Empty;

        return Static.AttachmentUrl + $"{Icon}/icon.webp";
    }
    public string? GetBannerUrl()
    {
        if (string.IsNullOrEmpty(Banner))
            return string.Empty;

        return Static.AttachmentUrl + $"{Banner}/banner.webp";
    }
}
