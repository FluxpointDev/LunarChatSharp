using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Messages;

namespace LunarChatSharp;

public static class MessageHelpers
{
    public static async Task<RestMessage[]> GetMessagesAsync(this LunarRestClient rest, string channelId)
    {
        RestMessage[]? messages = await rest.GetAsync<RestMessage[]>($"/channels/{channelId}/messages");
        if (messages == null)
            return Array.Empty<RestMessage>();

        return messages;
    }
    public static async Task SendMesssageAsync(this LunarRestClient rest, string channelId, CreateMessageRequest request)
    {
        await rest.PostAsync($"/channels/{channelId}/messages", request);
    }

    public static async Task DeleteMessageAsync(this LunarRestClient rest, string channelId, string messageId)
    {
        await rest.DeleteAsync($"/channels/{channelId}/messages/{messageId}");
    }
}
