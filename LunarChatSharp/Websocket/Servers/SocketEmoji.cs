using LunarChatSharp.Rest.Messages;

namespace LunarChatSharp.Websocket.Servers;

public class SocketEmoji : RestEmoji
{
    public static SocketEmoji Create(RestEmoji data)
    {
        SocketEmoji emoji = new SocketEmoji
        {
            Id = data.Id,
            Name = data.Name,
            CreatedAt = data.CreatedAt,
            CreatedBy = data.CreatedBy,
            IconId = data.IconId,
            SourceId = data.SourceId,
            SourceType = data.SourceType,
            IsAnimated = data.IsAnimated,
        };

        return emoji;
    }
}
