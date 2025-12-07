using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Messages;
using LunarChatSharp.Rest.Servers;

namespace LunarChatSharp;

public static class EmojiHelpers
{
    public static async Task<RestEmoji> EditEmojiAsync(this LunarRestClient rest, string serverId, string emojiId, EditEmojiRequest request)
    {
        return await rest.PatchAsync<RestEmoji>($"/servers/{serverId}/emojis/{emojiId}", request);
    }

    public static async Task<RestEmoji> CreateEmojiAsync(this LunarRestClient rest, string serverId, CreateEmojiRequest request)
    {
        return await rest.PostAsync<RestEmoji>($"/servers/{serverId}/emojis", request);
    }

    public static async Task<RestEmoji?> GetEmojiAsync(this LunarRestClient rest, string serverId, string emojiId)
    {
        return await rest.GetAsync<RestEmoji>($"/servers/{serverId}/emojis/{emojiId}");
    }

    public static async Task<RestEmoji[]> GetEmojisAsync(this LunarRestClient rest, string serverId)
    {
        var roles = await rest.GetAsync<RestEmoji[]>($"/servers/{serverId}/emojis");
        if (roles == null)
            return Array.Empty<RestEmoji>();

        return roles;
    }

    public static async Task DeleteEmojiAsync(this LunarRestClient rest, string serverId, string emojiId)
    {
        await rest.DeleteAsync($"/servers/{serverId}/emojis/{emojiId}");
    }
}
