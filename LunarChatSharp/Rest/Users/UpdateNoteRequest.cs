using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Users;

public class UpdateNoteRequest : ILunarRequest
{
    [JsonPropertyName("note")]
    public string? Note { get; set; }
}
