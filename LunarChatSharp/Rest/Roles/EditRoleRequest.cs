using LunarChatSharp.Rest.Messages;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Roles;

public class EditRoleRequest : ILunarRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("hoist")]
    public bool? Hoist { get; set; }

    [JsonPropertyName("position")]
    public int? Position { get; set; }

    [JsonPropertyName("mentionable")]
    public bool? Mentionable { get; set; }

    public string? GetIconUrl()
    {
        if (string.IsNullOrEmpty(Icon))
            return string.Empty;

        return Static.AttachmentUrl + $"{Icon}/emoji.webp";
    }

    [JsonPropertyName("permissions")]
    public RestPermissions? Permissions { get; set; }

    [JsonIgnore]
    public RestAttachment? EditIcon;
}
