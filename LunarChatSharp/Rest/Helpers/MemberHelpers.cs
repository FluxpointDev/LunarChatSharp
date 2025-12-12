using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Servers;

namespace LunarChatSharp;

public static class MemberHelpers
{
    public static async Task<RestMember?> GetMemberAsync(this LunarRestClient rest, string serverId, string userId)
    {
        return await rest.GetAsync<RestMember>($"/servers/{serverId}/members/{userId}");
    }

    public static async Task<RestMember[]> GetMembersAsync(this LunarRestClient rest, string serverId)
    {
        var members = await rest.GetAsync<RestMember[]>($"/servers/{serverId}/members");
        if (members == null)
            return Array.Empty<RestMember>();

        return members;
    }

    /// <summary>
    /// This is only used for internal lunar chat servers!
    /// </summary>
    public static async Task AddMemberAsync(this LunarRestClient rest, string serverId, string userId)
    {
        await rest.PutAsync($"/servers/{serverId}/members/{userId}");
    }

    /// <summary>
    /// This is only used for internal lunar chat servers!
    /// </summary>
    public static async Task LeaveServerAsync(this LunarRestClient rest, string serverId)
    {
        await rest.DeleteAsync($"/servers/{serverId}/leave");
    }

    public static async Task AddMemberRoleAsync(this LunarRestClient rest, string serverId, string userId, string roleId)
    {
        await rest.PutAsync($"/servers/{serverId}/members/{userId}/roles/{roleId}");
    }

    public static async Task RemoveMemberRoleAsync(this LunarRestClient rest, string serverId, string userId, string roleId)
    {
        await rest.DeleteAsync($"/servers/{serverId}/members/{userId}/roles/{roleId}");
    }

    public static async Task KickMemberAsync(this LunarRestClient rest, string serverId, string userId)
    {
        await rest.DeleteAsync($"/servers/{serverId}/members/{userId}");
    }

    public static async Task<RestBan> BanMemberAsync(this LunarRestClient rest, string serverId, string userId)
    {
        return await rest.PutAsync<RestBan>($"/servers/{serverId}/bans/{userId}");
    }

    public static async Task<RestBan> UnbanMemberAsync(this LunarRestClient rest, string serverId, string userId)
    {
        return await rest.DeleteAsync<RestBan>($"/servers/{serverId}/bans/{userId}");
    }

    public static async Task<RestMember> EditMemberAsync(this LunarRestClient rest, string serverId, string userId, EditMemberRequest request)
    {
        return await rest.PatchAsync<RestMember>($"/servers/{serverId}/members/{userId}", request);
    }

    public static async Task<RestMember> TimeoutMemberAsync(this LunarRestClient rest, string serverId, string userId, DateTime? time)
    {
        var req = new EditMemberRequest();
        if (time == null)
            req.TimeoutRemove = true;
        else
            req.Timeout = time.Value;

        return await rest.EditMemberAsync(serverId, userId, req);
    }
}
