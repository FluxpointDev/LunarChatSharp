using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Accounts;

public class CreateAccountRequest : ILunarRequest
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }
}
public class EditAccountRequest : ILunarRequest
{
    [JsonPropertyName("friend_request_access")]
    public EditFriendRequestAccess? FriendRequestAccess { get; set; }

    [JsonPropertyName("direct_messages_access")]
    public EditDirectMessagesAccess? DirectMessagesAccess { get; set; }
}
public class EditFriendRequestAccess
{
    [JsonPropertyName("everyone")]
    public bool? Everyone { get; set; }

    [JsonPropertyName("mutual_servers")]
    public bool? MutualServers { get; set; }

    [JsonPropertyName("mutual_friends")]
    public bool? MutualFriends { get; set; }
}
public class EditDirectMessagesAccess
{
    [JsonPropertyName("everyone")]
    public bool? Everyone { get; set; }

    [JsonPropertyName("mutual_servers")]
    public bool? MutualServers { get; set; }

    [JsonPropertyName("mutual_friends")]
    public bool? MutualFriends { get; set; }
}
public class CreateDemoAccountRequest : ILunarRequest
{
    [JsonPropertyName("username")]
    public required string Username { get; set; }
}
