using LunarChatSharp.Rest.Users;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Channels;

public class RestInvite
{
    [JsonPropertyName("code")]
    public required string Code { get; set; }

    [JsonPropertyName("server_id")]
    public required string ServerId { get; set; }

    [JsonPropertyName("channel_id")]
    public required string ChannelId { get; set; }

    [JsonPropertyName("inviter")]
    public required RestUser? Inviter { get; set; }

    [JsonPropertyName("uses")]
    public ulong Uses { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime? CreatedAt { get; set; }
}
