using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Accounts;
using LunarChatSharp.Rest.Users;

namespace LunarChatSharp;

public static class UserHelpers
{
    public static async Task<RestUser> CreateDemoAccount(this LunarRestClient rest, CreateDemoAccountRequest request)
    {
        RestUser Json = await rest.PostAsync<RestUser>("/accounts/test", request);
        return Json;
    }

    public static async Task RemoveBlockAsync(this LunarRestClient rest, string userId)
    {
        await rest.DeleteAsync($"/users/{userId}/block");
    }

    public static async Task UpdateNoteAsync(this LunarRestClient rest, string userId, string? note)
    {
        await rest.PatchAsync($"/users/{userId}/note", new UpdateNoteRequest
        {
            Note = note
        });
    }

    public static async Task RemoveFriendAsync(this LunarRestClient rest, string userId)
    {
        await rest.DeleteAsync($"/users/{userId}/friend");
    }

    public static async Task AddFriendAsync(this LunarRestClient rest, string username)
    {
        await rest.PutAsync($"/users/{username}/friend");
    }

    public static async Task AddBlockAsync(this LunarRestClient rest, string username)
    {
        await rest.DeleteAsync($"/users/{username}/block");
    }

    public static async Task AddIgnoreAsync(this LunarRestClient rest, string username)
    {
        await rest.DeleteAsync($"/users/{username}/ignore");
    }

    public static async Task RemoveIgnoreAsync(this LunarRestClient rest, string userId)
    {
        await rest.DeleteAsync($"/users/{userId}/ignore");
    }
}
