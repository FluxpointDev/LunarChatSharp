using LunarChatSharp.Rest.Accounts;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Websocket.Events.Account;

public class AccountUpdateEvent : ISocketEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "account_update";

    [JsonPropertyName("changed")]
    public EditAccountRequest? Changed { get; set; }
}
