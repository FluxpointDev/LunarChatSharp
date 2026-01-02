using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Servers;

namespace LunarChatSharp;

public static class MemberHelpers
{
    public static async Task<RestMember?> GetMemberAsync(this LunarRestClient rest, ulong serverId, ulong userId)
    {
        return await rest.GetAsync<RestMember>($"/servers/{serverId}/members/{userId}");
    }

    public static async Task<RestMember[]> GetMembersAsync(this LunarRestClient rest, ulong serverId)
    {
        var members = await rest.GetAsync<RestMember[]>($"/servers/{serverId}/members");
        if (members == null)
            return Array.Empty<RestMember>();

        return members;
    }

    /// <summary>
    /// This is only used for internal lunar chat servers!
    /// </summary>
    public static async Task AddMemberAsync(this LunarRestClient rest, ulong serverId, ulong userId)
    {
        await rest.PutAsync($"/servers/{serverId}/members/{userId}");
    }

    /// <summary>
    /// This is only used for internal lunar chat servers!
    /// </summary>
    public static async Task LeaveServerAsync(this LunarRestClient rest, ulong serverId)
    {
        await rest.DeleteAsync($"/servers/{serverId}/leave");
    }

    public static async Task AddMemberRoleAsync(this LunarRestClient rest, ulong serverId, ulong userId, ulong roleId)
    {
        await rest.PutAsync($"/servers/{serverId}/members/{userId}/roles/{roleId}");
    }

    public static async Task RemoveMemberRoleAsync(this LunarRestClient rest, ulong serverId, ulong userId, ulong roleId)
    {
        await rest.DeleteAsync($"/servers/{serverId}/members/{userId}/roles/{roleId}");
    }

    public static async Task KickMemberAsync(this LunarRestClient rest, ulong serverId, ulong userId, ReasonRequest req)
    {
        await rest.DeleteAsync($"/servers/{serverId}/members/{userId}", req);
    }

    public static async Task<RestBan> BanMemberAsync(this LunarRestClient rest, ulong serverId, ulong userId, CreateBanRequest req)
    {
        return await rest.PutAsync<RestBan>($"/servers/{serverId}/bans/{userId}", req);
    }

    public static async Task<RestBan> UnbanMemberAsync(this LunarRestClient rest, ulong serverId, ulong userId)
    {
        return await rest.DeleteAsync<RestBan>($"/servers/{serverId}/bans/{userId}");
    }

    public static async Task<RestMember> EditMemberAsync(this LunarRestClient rest, ulong serverId, ulong userId, EditMemberRequest request)
    {
        return await rest.PatchAsync<RestMember>($"/servers/{serverId}/members/{userId}", request);
    }

    public static async Task<RestMember> TimeoutMemberAsync(this LunarRestClient rest, ulong serverId, ulong userId, DateTime? time, string? reason)
    {
        var req = new EditMemberRequest
        {
            Reason = reason
        };

        if (time == null)
            req.TimeoutRemove = true;
        else
            req.Timeout = time.Value;

        return await rest.EditMemberAsync(serverId, userId, req);
    }
}
