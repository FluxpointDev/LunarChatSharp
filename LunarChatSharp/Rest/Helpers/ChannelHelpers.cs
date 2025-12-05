using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Channels;

namespace LunarChatSharp;

public static class ChannelHelpers
{
    public static async Task<RestChannel> CreateChannelAsync(this LunarRestClient rest, CreateChannelRequest request)
    {
        return await rest.PostAsync<RestChannel>($"/channels", request);
    }

    public static async Task<RestChannel?> GetChannelAsync(this LunarRestClient rest, string channelId)
    {
        return await rest.GetAsync<RestChannel>($"/channels/{channelId}");
    }

    public static async Task<RestChannel> UpdateChannelAsync(this LunarRestClient rest, string channelId, UpdateChannelRequest request)
    {
        return await rest.PatchAsync<RestChannel>($"/channels/{channelId}", request);
    }

    public static async Task DeleteChannelAsync(this LunarRestClient rest, string channelId, DeleteChannelRequest request)
    {
        await rest.DeleteAsync($"/channels/{channelId}", request);
    }
}
