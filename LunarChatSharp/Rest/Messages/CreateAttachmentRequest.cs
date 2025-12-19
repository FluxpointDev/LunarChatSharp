using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Messages;

public class CreateAttachmentRequest
{
    [JsonConstructor]
    internal CreateAttachmentRequest()
    {

    }

    public CreateAttachmentRequest(string file, string fileName, string? description = "", bool isSpoiler = false)
    {
        using (FileStream fileStream = File.OpenRead(file))
            Set(fileStream, fileName, description, isSpoiler);
    }

    public CreateAttachmentRequest(Stream stream, string fileName, string? description = "", bool isSpoiler = false)
    {
        Set(stream, fileName, description, isSpoiler);
    }

    internal void Set(Stream stream, string fileName, string? description, bool isSpoiler = false)
    {
        FileName = fileName;
        Description = description;
        IsSpoiler = isSpoiler;
        if (stream is FileStream filestr)
        {
            using (MemoryStream str = new MemoryStream())
            {
                filestr.CopyTo(str);
                Content = new ByteArrayContent(str.ToArray());
            }
        }
        else if (stream is MemoryStream memstr)
        {
            Content = new ByteArrayContent(memstr.ToArray());
        }
        else
            throw new LunarException("Invalid attachment content");

        if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        else if (fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
            Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
        else
            throw new LunarException("Invalid attachment type");
    }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("file_name")]
    public string? FileName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("is_spoiler")]
    public bool IsSpoiler { get; set; }

    [JsonIgnore]
    internal ByteArrayContent Content { get; set; }
}
