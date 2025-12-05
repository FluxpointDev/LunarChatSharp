using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Dev;
using LunarChatSharp.Rest.Servers;

namespace LunarChatSharp;

public static class ServerHelpers
{
    public static async Task<RestCreatedServer> CreateServerAsync(this LunarRestClient rest, CreateServerRequest request)
    {
        return await rest.PostAsync<RestCreatedServer>("/servers", request);
    }

    public static async Task<RestServer?> GetServerAsync(this LunarRestClient rest, string serverId)
    {
        return await rest.GetAsync<RestServer>($"/servers/{serverId}");
    }

    public static async Task<RestApp[]> GetServerAppsAsync(this LunarRestClient rest, string serverId)
    {
        var apps = await rest.GetAsync<RestApp[]>($"/servers/{serverId}/apps");
        if (apps == null)
            return Array.Empty<RestApp>();

        return apps;
    }

    public static async Task<RestApp?> GetServerAppAsync(this LunarRestClient rest, string serverId, string appId)
    {
        return await rest.GetAsync<RestApp>($"/servers/{serverId}/{appId}");
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

    public static async Task<RestServer> EditServerAsync(this LunarRestClient rest, string serverId, EditServerRequest request)
    {
        return await rest.PatchAsync<RestServer>($"/servers/{serverId}", request);
    }

    public static async Task LeaveServerAsync(this LunarRestClient rest, string serverId)
    {
        await rest.DeleteAsync($"/servers/{serverId}");
    }
}
