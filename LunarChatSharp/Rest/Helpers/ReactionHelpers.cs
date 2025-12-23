namespace LunarChatSharp.Rest.Helpers;

public static class ReactionHelpers
{
    public static async Task AddReactionAsync(this LunarRestClient rest, string channelId, string messageId, string emojiId)
    {
        await rest.PutAsync($"/channels/{channelId}/messages/{messageId}/reactions/{emojiId}/me");
    }

    public static async Task RemoveReactionAsync(this LunarRestClient rest, string channelId, string messageId, string emojiId)
    {
        await rest.DeleteAsync($"/channels/{channelId}/messages/{messageId}/reactions/{emojiId}/me");
    }

    public static async Task DeleteReactionUserAsync(this LunarRestClient rest, string channelId, string messageId, string emojiId, string userId)
    {
        await rest.DeleteAsync($"/channels/{channelId}/messages/{messageId}/reactions/{emojiId}/{userId}");
    }

    public static async Task DeleteReactionEmojiAsync(this LunarRestClient rest, string channelId, string messageId, string emojiId)
    {
        await rest.DeleteAsync($"/channels/{channelId}/messages/{messageId}/reactions/{emojiId}");
    }
    public static async Task DeleteReactionsAllAsync(this LunarRestClient rest, string channelId, string messageId)
    {
        await rest.DeleteAsync($"/channels/{channelId}/messages/{messageId}/reactions");
    }
}
