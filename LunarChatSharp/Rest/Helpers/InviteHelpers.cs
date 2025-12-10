using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Channels;

namespace LunarChatSharp;

public static class InviteHelpers
{
    public static async Task<RestInvite> UseInviteAsync(this LunarRestClient rest, string inviteCode)
    {
        return await rest.PostAsync<RestInvite>($"/invites/{inviteCode}");
    }

    public static async Task<RestInvite?> GetInviteAsync(this LunarRestClient rest, string inviteCode)
    {
        return await rest.GetAsync<RestInvite>($"/invites/{inviteCode}");
    }

    public static async Task<RestInvite[]> GetServerInvitesAsync(this LunarRestClient rest, string serverId)
    {
        return await rest.GetAsync<RestInvite[]>($"/servers/{serverId}/invites");
    }

    public static async Task<RestInvite> CreateInviteAsync(this LunarRestClient rest, string channelId)
    {
        return await rest.PostAsync<RestInvite>($"/channels/{channelId}/invites");
    }

    public static async Task DeleteInviteAsync(this LunarRestClient rest, string channelId, string inviteCode)
    {
        await rest.DeleteAsync($"/channels/{channelId}/invites/{inviteCode}");
    }
}
