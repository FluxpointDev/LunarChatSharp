using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Channels;
using LunarChatSharp.Rest.Dev;

namespace LunarChatSharp;

public static class ChannelHelpers
{
    public static async Task<RestApp[]> GetGroupAppsAsync(this LunarRestClient rest, ulong groupId)
    {
        var apps = await rest.GetAsync<RestApp[]>($"/groups/{groupId}/apps");
        if (apps == null)
            return Array.Empty<RestApp>();

        return apps;
    }

    public static async Task<RestChannel> CreateChannelAsync(this LunarRestClient rest, CreateChannelRequest request)
    {
        return await rest.PostAsync<RestChannel>($"/channels", request);
    }

    public static async Task<RestChannel?> GetChannelAsync(this LunarRestClient rest, ulong channelId)
    {
        return await rest.GetAsync<RestChannel>($"/channels/{channelId}");
    }

    public static async Task<RestChannel> UpdateChannelAsync(this LunarRestClient rest, ulong channelId, UpdateChannelRequest request)
    {
        return await rest.PatchAsync<RestChannel>($"/channels/{channelId}", request);
    }

    public static async Task DeleteChannelAsync(this LunarRestClient rest, ulong channelId, DeleteChannelRequest request)
    {
        await rest.DeleteAsync($"/channels/{channelId}", request);
    }
}
