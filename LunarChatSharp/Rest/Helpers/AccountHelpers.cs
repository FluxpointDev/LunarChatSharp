using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Accounts;
using LunarChatSharp.Rest.Users;

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

    public static async Task<RestAccount> AccountEdit(this LunarRestClient rest, EditAccountRequest request)
    {
        return await rest.PatchAsync<RestAccount>($"/accounts/@me", request);
    }

    public static async Task AccountEditUsername(this LunarRestClient rest, string username)
    {
        await rest.PatchAsync($"/accounts/@me/username", new EditUsernameRequest
        {
            Username = username
        });
    }
}
