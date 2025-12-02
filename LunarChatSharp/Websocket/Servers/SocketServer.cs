using LunarChatSharp.Rest.Servers;

namespace LunarChatSharp.Websocket.Servers;

public class SocketServer : RestServer
{
    public static SocketServer Create(RestServer data)
    {
        SocketServer server = new SocketServer
        {
            Id = data.Id,
            Name = data.Name,
            CreatedAt = data.CreatedAt,
            Description = data.Description,
            OwnerId = data.OwnerId,
            SystemMessages = data.SystemMessages,
        };

        return server;
    }
}
