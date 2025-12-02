using LunarChatSharp.Rest.Servers;

namespace LunarChatSharp.Websocket.Servers;

public class SocketMember : RestMember
{
    public static SocketMember Create(RestMember data)
    {
        SocketMember member = new SocketMember
        {
            Id = data.Id,
            Nickname = data.Nickname,
            Roles = data.Roles,
            Timeout = data.Timeout,
            ServerId = data.ServerId,
        };

        return member;
    }
}
