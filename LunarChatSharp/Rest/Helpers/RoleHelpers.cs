using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Roles;

namespace LunarChatSharp;

public static class RoleHelpers
{
    public static async Task<RestRole> EditRoleAsync(this LunarRestClient rest, string serverId, string roleId, EditRoleRequest request)
    {
        return await rest.PatchAsync<RestRole>($"/servers/{serverId}/roles/{roleId}", request);
    }

    public static async Task<RestRole> CreateRoleAsync(this LunarRestClient rest, string serverId, CreateRoleRequest request)
    {
        return await rest.PostAsync<RestRole>($"/servers/{serverId}/roles", request);
    }

    public static async Task<RestRole?> GetRoleAsync(this LunarRestClient rest, string serverId, string roleId)
    {
        return await rest.GetAsync<RestRole>($"/servers/{serverId}/roles/{roleId}");
    }

    public static async Task<RestRole[]> GetRolesAsync(this LunarRestClient rest, string serverId)
    {
        var roles = await rest.GetAsync<RestRole[]>($"/servers/{serverId}/roles");
        if (roles == null)
            return Array.Empty<RestRole>();

        return roles;
    }

    public static async Task DeleteRoleAsync(this LunarRestClient rest, string serverId, string roleId)
    {
        await rest.DeleteAsync($"/servers/{serverId}/roles/{roleId}");
    }
}
