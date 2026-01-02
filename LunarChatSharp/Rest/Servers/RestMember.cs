using LunarChatSharp.Rest.Users;
using LunarChatSharp.Websocket.Events;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Servers;

public class RestMember
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("server_id")]
    public required ulong ServerId { get; set; }

    [JsonPropertyName("nickname")]
    public string? Nickname { get; set; }

    [JsonPropertyName("roles")]
    public required HashSet<ulong> Roles { get; set; }

    [JsonPropertyName("timeout")]
    public DateTime? Timeout { get; set; }

    [JsonPropertyName("joined_at")]
    public required DateTime JoinedAt { get; set; }

    [JsonPropertyName("user")]
    public required RestUser User { get; set; }

    public string GetCurrentName()
    {
        return (Nickname ?? User.DisplayName ?? User.Username);
    }

    public string GetCurrentNameDiscrim()
    {
        return (Nickname ?? User.DisplayName ?? User.Username) + (User.IsBot ? "#" + User.Discriminator : null);
    }

    public int GetRank(ServerState server)
    {
        int CurrentRank = 0;
        if (Id == server.Server.OwnerId)
            CurrentRank = Static.MaxRoles + 1;

        foreach (var r in Roles)
        {
            if (server.Roles.TryGetValue(r, out var role) && role.Position > CurrentRank)
                CurrentRank = role.Position;
        }
        return CurrentRank;
    }
}
