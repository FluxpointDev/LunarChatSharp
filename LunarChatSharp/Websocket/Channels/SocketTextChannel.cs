using LunarChatSharp.Rest.Channels;

namespace LunarChatSharp.Websocket.Channels;

public class SocketTextChannel : RestChannel
{
    public static SocketTextChannel Create(RestChannel data)
    {
        SocketTextChannel channel = new SocketTextChannel
        {
            CreatedAt = data.CreatedAt,
            Id = data.Id,
            Name = data.Name,
            ServerId = data.ServerId,
            Topic = data.Topic,
            Type = data.Type,
        };

        return channel;
    }
}
