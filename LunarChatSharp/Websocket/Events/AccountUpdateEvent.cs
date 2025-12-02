using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events;

public class AccountUpdateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "account_update";

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("requests_everyone")]
    public bool? FriendRequestsEveryone { get; set; }

    [JsonPropertyName("requests_server_members")]
    public bool? FriendRequestsServerMembers { get; set; }

    [JsonPropertyName("requests_mutual_friends")]
    public bool? FriendRequestsMutualFriends { get; set; }
}
