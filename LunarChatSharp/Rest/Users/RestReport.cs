using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Users;

public class RestReport
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("user_id")]
    public required ulong UserId { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("type")]
    public required ReportType? Type { get; set; }

    [JsonPropertyName("target_id")]
    public required ulong TargetId { get; set; }

    [JsonPropertyName("target_type")]
    public required ReportTargetType? TargetType { get; set; }

    [JsonPropertyName("is_complete")]
    public bool? IsComplete { get; set; }

    [JsonPropertyName("details")]
    public string? Details { get; set; }
}
public enum ReportType : sbyte
{

}
public enum ReportTargetType
{
    Server, Channel, User, Message
}