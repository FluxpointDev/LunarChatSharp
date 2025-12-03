using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Channels;

namespace LunarChatSharp;

public static class ChannelHelpers
{
    public static async Task CreateChannelAsync(this LunarRestClient rest, CreateChannelRequest request)
    {
        await rest.PostAsync($"/channels", request);
    }

    public static async Task UpdateChannelAsync(this LunarRestClient rest, string channelId, UpdateChannelRequest request)
    {
        await rest.PatchAsync($"/channels/{channelId}", request);
    }

    public static async Task DeleteChannelAsync(this LunarRestClient rest, string channelId, DeleteChannelRequest request)
    {
        await rest.DeleteAsync($"/channels/{channelId}", request);
    }
}
