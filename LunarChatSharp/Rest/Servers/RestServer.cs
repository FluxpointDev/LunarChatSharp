using LunarChatSharp.Core.Servers;
using LunarChatSharp.Rest.Roles;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class RestServer
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("icon_id")]
    public string? IconId { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("owner_id")]
    public required string OwnerId { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("vanity_invite")]
    public string? VanityInvite { get; set; }

    [JsonPropertyName("system_messages")]
    public required RestServerSystemMessages SystemMessages { get; set; }

    [JsonPropertyName("default_permissions")]
    public required RestPermissions DefaultPermissions { get; set; }

    [JsonPropertyName("features")]
    public ServerFeature Features { get; set; }

    public string GetFallback()
    {
        string[] Split = Name.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (!Split.Any())
            return null!;

        if (Split.Length == 1)
            return Split[0].ToUpper()[0].ToString();

        return $"{Split[0].ToUpper()[0]}{Split.Last().ToUpper()[0]}";
    }
}
