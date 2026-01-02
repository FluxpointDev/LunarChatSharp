using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Users;

public class RestWarning
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; set; }

    [JsonPropertyName("report_type")]
    public required ReportType? ReportType { get; set; }

    [JsonPropertyName("expire_at")]
    public DateTime? ExpireAt { get; set; }
}
