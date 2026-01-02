using LunarChatSharp.Rest.Roles;

namespace LunarChatSharp.Websocket.Roles;

public class SocketRole : RestRole
{
    public static SocketRole Create(RestRole data)
    {
        SocketRole role = new SocketRole
        {
            Id = data.Id,
            Name = data.Name,
            Mentionable = data.Mentionable,
            CreatedAt = data.CreatedAt,
            Color = data.Color,
            Hoist = data.Hoist,
            Permissions = data.Permissions,
            Position = data.Position,
        };

        return role;
    }
}
