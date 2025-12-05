using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Dev;
using LunarChatSharp.Rest.Users;

namespace LunarChatSharp;

public static class DevHelpers
{
    public static async Task AddServerAppAsync(this LunarRestClient rest, string serverId, string appId)
    {
        await rest.PutAsync($"/servers/{serverId}/apps", new InviteAppRequest
        {
            AppId = appId,
        });
    }

    public static async Task RemoveServerAppAsync(this LunarRestClient rest, string serverId, string appId)
    {
        await rest.DeleteAsync($"/servers/{serverId}/apps/{appId}");
    }

    public static async Task<RestApp> EditAppAsync(this LunarRestClient rest, string appId, CreateAppRequest request)
    {
        return await rest.PatchAsync<RestApp>($"/apps/{appId}", request);
    }

    public static async Task<RestApp> CreateAppAsync(this LunarRestClient rest, CreateAppRequest request)
    {
        return await rest.PostAsync<RestApp>("/apps", request);
    }

    public static async Task<RestTeam> CreateTeamAsync(this LunarRestClient rest, CreateTeamRequest request)
    {
        return await rest.PostAsync<RestTeam>("/teams", request);
    }

    public static async Task<RestTeam> EditTeamAsync(this LunarRestClient rest, string teamId, CreateTeamRequest request)
    {
        return await rest.PatchAsync<RestTeam>($"/teams/{teamId}", request);
    }

    public static async Task<RestApp?> GetAppAsync(this LunarRestClient rest, string appId)
    {
        return await rest.GetAsync<RestApp>($"/apps/{appId}");
    }

    public static async Task<RestTeam?> GetTeamAsync(this LunarRestClient rest, string teamId)
    {
        return await rest.GetAsync<RestTeam>($"/teams/{teamId}");
    }

    public static async Task DeleteAppAsync(this LunarRestClient rest, string appId)
    {
        await rest.DeleteAsync($"/apps/{appId}");
    }

    public static async Task DeleteTeamAsync(this LunarRestClient rest, string teamId)
    {
        await rest.DeleteAsync($"/teams/{teamId}");
    }

    public static async Task<RestDev> GetDevAsync(this LunarRestClient rest)
    {
        return (await rest.GetAsync<RestDev>("/accounts/@me/dev", throwGetRequest: true))!;
    }
}
