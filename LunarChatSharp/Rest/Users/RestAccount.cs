using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Users;

public class RestAccount
{
    [JsonPropertyName("user_id")]
    public required ulong UserId { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("friend_request_access")]
    public required RestAccountAccess? FriendRequestAccess { get; set; }

    [JsonPropertyName("direct_messages_access")]
    public required RestAccountAccess? DirectMessagesAccess { get; set; }

    [JsonPropertyName("flagged")]
    public bool? Flagged { get; set; }

    [JsonPropertyName("disabled")]
    public bool? Disabled { get; set; }

    [JsonPropertyName("deleted")]
    public bool? Deleted { get; set; }
}

public class RestAccountAccess
{
    [JsonPropertyName("everyone")]
    public bool Everyone { get; set; }

    [JsonPropertyName("mutual_servers")]
    public bool MutualServers { get; set; }

    [JsonPropertyName("mutual_friends")]
    public bool MutualFriends { get; set; }

    [JsonPropertyName("verified")]
    public bool Verified { get; set; }
}