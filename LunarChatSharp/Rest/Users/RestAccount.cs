using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Users;

public class RestAccount
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("friend_request_access")]
    public required FriendRequestAccess? FriendRequestAccess { get; set; }

    [JsonPropertyName("direct_messages_access")]
    public required DirectMessagesAccess? DirectMessagesAccess { get; set; }
}

public class FriendRequestAccess
{
    [JsonPropertyName("everyone")]
    public bool Everyone { get; set; }

    [JsonPropertyName("mutual_servers")]
    public bool MutualServers { get; set; }

    [JsonPropertyName("mutual_friends")]
    public bool MutualFriends { get; set; }
}
public class DirectMessagesAccess
{
    [JsonPropertyName("everyone")]
    public bool Everyone { get; set; }
    [JsonPropertyName("mutual_servers")]

    public bool MutualServers { get; set; }

    [JsonPropertyName("mutual_friends")]
    public bool MutualFriends { get; set; }
}