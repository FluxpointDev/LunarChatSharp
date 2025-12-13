using LunarChatSharp.Rest.Accounts;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Account;

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

    [JsonPropertyName("changed")]
    public EditAccountRequest? Changed { get; set; }
}
