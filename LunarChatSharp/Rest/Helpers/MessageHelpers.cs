using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Messages;
using System.Net.Http.Json;

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

    public static async Task<RestMessage?> GetMessageAsync(this LunarRestClient rest, string channelId, string messageId)
    {
        return await rest.GetAsync<RestMessage>($"/channels/{channelId}/messages/{messageId}");
    }

    public static async Task<RestMessage> SendMesssageAsync(this LunarRestClient rest, string channelId, CreateMessageRequest request)
    {
        if (request.Attachments != null && request.Attachments.Length != 0)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            var Json = JsonContent.Create(request, mediaType: LunarRestClient.JsonHeader, options: LunarRestClient.JsonOptions);
            form.Add(Json, "\"message\"");

            for (int n = 0; n != request.Attachments.Length; n++)
            {
                var attachment = request.Attachments[n];
                attachment.Id = $"file_{n}";
                form.Add(attachment.Content, $"\"file_{n}\"");
                form.Last().Headers.ContentDisposition?.FileName = $"\"{attachment.FileName}\"";
            }

            return await rest.PostAsync<RestMessage>($"/channels/{channelId}/messages", form);
        }
        else
            return await rest.PostAsync<RestMessage>($"/channels/{channelId}/messages", request);
    }

    public static async Task<RestMessage> EditMesssageAsync(this LunarRestClient rest, string channelId, EditMessageRequest request)
    {
        return await rest.PatchAsync<RestMessage>($"/channels/{channelId}/messages", request);
    }

    public static async Task DeleteMessageAsync(this LunarRestClient rest, string channelId, string messageId)
    {
        await rest.DeleteAsync($"/channels/{channelId}/messages/{messageId}");
    }
}
