using LunarChatSharp.Rest;
using LunarChatSharp.Rest.Roles;

namespace LunarChatSharp;

public static class RoleHelpers
{
    public static async Task EditRoleAsync(this LunarRestClient rest, string serverId, string roleId, EditRoleRequest request)
    {
        await rest.PatchAsync($"/servers/{serverId}/roles/{roleId}", request);
    }

    public static async Task CreateRoleAsync(this LunarRestClient rest, string serverId, CreateRoleRequest request)
    {
        await rest.PostAsync($"/servers/{serverId}/roles", request);
    }

    public static async Task DeleteRoleAsync(this LunarRestClient rest, string serverId, string roleId)
    {
        await rest.DeleteAsync($"/servers/{serverId}/roles/{roleId}");
    }
}
