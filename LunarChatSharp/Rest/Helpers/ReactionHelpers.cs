namespace LunarChatSharp.Rest.Helpers;

public static class ReactionHelpers
{
    public static async Task AddReactionAsync(this LunarRestClient rest, ulong channelId, ulong messageId, ulong emojiId)
    {
        await rest.PutAsync($"/channels/{channelId}/messages/{messageId}/reactions/{emojiId}/me");
    }

    public static async Task RemoveReactionAsync(this LunarRestClient rest, ulong channelId, ulong messageId, ulong emojiId)
    {
        await rest.DeleteAsync($"/channels/{channelId}/messages/{messageId}/reactions/{emojiId}/me");
    }

    public static async Task DeleteReactionUserAsync(this LunarRestClient rest, ulong channelId, ulong messageId, ulong emojiId, ulong userId)
    {
        await rest.DeleteAsync($"/channels/{channelId}/messages/{messageId}/reactions/{emojiId}/{userId}");
    }

    public static async Task DeleteReactionEmojiAsync(this LunarRestClient rest, ulong channelId, ulong messageId, ulong emojiId)
    {
        await rest.DeleteAsync($"/channels/{channelId}/messages/{messageId}/reactions/{emojiId}");
    }
    public static async Task DeleteReactionsAllAsync(this LunarRestClient rest, ulong channelId, ulong messageId)
    {
        await rest.DeleteAsync($"/channels/{channelId}/messages/{messageId}/reactions");
    }
}
