using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Accounts;

namespace LunarChatSharp;

public static class AccountHelpers
{
    public static async Task AccountEditDisplayName(this LunarRestClient rest, string? displayName)
    {
        await rest.PatchAsync($"/accounts/@me/displayname", new EditDisplayNameRequest
        {
            DisplayName = displayName
        });
    }

    public static async Task AccountEditUsername(this LunarRestClient rest, string username)
    {
        await rest.PatchAsync($"/accounts/@me/username", new EditUsernameRequest
        {
            Username = username
        });
    }
}
