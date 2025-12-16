using LunarChatSharp.Core.Messages;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Messages;

public class RestEmbed
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("author")]
    public RestEmbedAuthor? Author { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }

    [JsonPropertyName("thumbnail")]
    public string? Thumbnail { get; set; }

    [JsonPropertyName("url")]
    public string? SourceUrl { get; set; }

    [JsonPropertyName("source_type")]
    public EmbedSourceType? SourceType { get; set; }

    [JsonPropertyName("inline_fields")]
    public bool? InlineFields { get; set; }

    [JsonPropertyName("fields")]
    public RestEmbedField[] Fields { get; set; }

    [JsonPropertyName("video")]
    public string? Video { get; set; }

    [JsonPropertyName("footer")]
    public RestEmbedFooter? Footer { get; set; }
}
public class RestEmbedField
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
public class RestEmbedAuthor
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("icon_url")]
    public string? IconUrl { get; set; }
}
public class RestEmbedFooter
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("icon_url")]
    public string? IconUrl { get; set; }
}