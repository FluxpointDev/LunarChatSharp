using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Servers;

namespace LunarChatSharp;

public static class ServerHelpers
{
    public static async Task CreateServerAsync(this LunarRestClient rest, CreateServerRequest request)
    {
        await rest.PostAsync<CreateServerRequest>("/servers", request);
    }

    /// <summary>
    /// This is only used for internal lunar chat servers!
    /// </summary>
    public static async Task AddMemberAsync(this LunarRestClient rest, string serverId, string userId)
    {
        await rest.PutAsync($"/servers/{serverId}/members/{userId}");
    }

    /// <summary>
    /// This is only used for internal lunar chat servers!
    /// </summary>
    public static async Task RemoveMemberAsync(this LunarRestClient rest, string serverId, string userId)
    {
        await rest.DeleteAsync($"/servers/{serverId}/members/{userId}");
    }

    public static async Task EditServerAsync(this LunarRestClient rest, string serverId, EditServerRequest request)
    {
        await rest.PatchAsync($"/servers/{serverId}", request);
    }

    public static async Task LeaveServerAsync(this LunarRestClient rest, string serverId)
    {
        await rest.DeleteAsync($"/servers/{serverId}");
    }
}
