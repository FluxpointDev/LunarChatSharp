using LunarChatSharp.Rest.Messages;
using LunarChatSharp.Rest.Webhooks;

namespace LunarChatSharp.Rest.Helpers;

public static class WebhookHelpers
{
    public static async Task<RestWebhook[]> GetWebhooksAsync(this LunarRestClient rest, ulong channelId)
    {
        var webhooks = await rest.GetAsync<RestWebhook[]>($"/channels/{channelId}/webhooks");
        if (webhooks == null)
            return Array.Empty<RestWebhook>();

        return webhooks;
    }

    public static async Task<RestWebhook?> GetWebhookAsync(this LunarRestClient rest, ulong channelId, ulong webhookId)
    {
        return await rest.GetAsync<RestWebhook>($"/channels/{channelId}/webhooks/{webhookId}");
    }

    public static async Task<RestWebhook> CreateWebhookAsync(this LunarRestClient rest, ulong channelId, CreateWebhookRequest request)
    {
        return await rest.PostAsync<RestWebhook>($"/channels/{channelId}/webhooks", request);
    }

    public static async Task<RestWebhook> EditWebhookAsync(this LunarRestClient rest, ulong channelId, ulong webhookId, EditWebhookRequest request)
    {
        return await rest.PatchAsync<RestWebhook>($"/channels/{channelId}/webhooks/{webhookId}", request);
    }

    public static async Task DeleteWebhookAsync(this LunarRestClient rest, ulong channelId, ulong webhookId)
    {
        await rest.DeleteAsync($"/channels/{channelId}/webhooks/{webhookId}");
    }

    public static async Task<RestWebhook?> GetWebhookAsync(this LunarRestClient rest, ulong channelId, ulong webhookId, string webhookToken)
    {
        return await rest.GetAsync<RestWebhook>($"/webhooks/{webhookId}/{webhookToken}");
    }

    public static async Task DeleteWebhookAsync(this LunarRestClient rest, ulong channelId, ulong webhookId, string webhookToken)
    {
        await rest.DeleteAsync($"/webhooks/{webhookId}/{webhookToken}");
    }

    public static async Task<RestWebhook> EditWebhookAsync(this LunarRestClient rest, ulong channelId, ulong webhookId, string webhookToken, EditWebhookRequest request)
    {
        return await rest.PatchAsync<RestWebhook>($"/webhooks/{webhookId}/{webhookToken}", request);
    }

    public static async Task<RestMessage> SendWebhookMessageAsync(this LunarRestClient rest, ulong channelId, ulong webhookId, string webhookToken, CreateMessageRequest request)
    {
        return await rest.PostAsync<RestMessage>($"/webhooks/{webhookId}/{webhookToken}", request);
    }
}
