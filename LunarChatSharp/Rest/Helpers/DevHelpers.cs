using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Dev;
using LunarChatSharp.Rest.Users;

namespace LunarChatSharp;

public static class DevHelpers
{
    public static async Task EditAppAsync(this LunarRestClient rest, string appId, CreateAppRequest request)
    {
        await rest.PatchAsync($"/apps/{appId}", request);

    }

    public static async Task<RestApp> CreateAppAsync(this LunarRestClient rest, CreateAppRequest request)
    {
        return await rest.PostAsync<RestApp>("/apps", request);
    }

    public static async Task<RestTeam> CreateTeamAsync(this LunarRestClient rest, CreateTeamRequest request)
    {
        return await rest.PostAsync<RestTeam>("/teams", request);
    }

    public static async Task EditTeamAsync(this LunarRestClient rest, string teamId, CreateTeamRequest request)
    {
        await rest.PatchAsync($"/teams/{teamId}", request);
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
        return await rest.GetAsync<RestDev>("/users/@me/dev", throwGetRequest: true);
    }
}
