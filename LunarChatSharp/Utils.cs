namespace LunarChatSharp;

public static class Utils
{
    public static string GetImageBase64(Stream stream)
    {
        byte[] bytes;
        int length;
        if (stream.CanSeek)
        {
            bytes = new byte[stream.Length - stream.Position];
            length = stream.Read(bytes, 0, bytes.Length);
        }
        else
        {
            if (stream is MemoryStream ms)
            {
                length = (int)ms.Length;
                bytes = ms.ToArray();
            }
            else
            {
                using (MemoryStream cloneStream = new MemoryStream())
                {
                    stream.CopyTo(cloneStream);
                    bytes = new byte[cloneStream.Length];
                    cloneStream.Position = 0;
                    cloneStream.Read(bytes, 0, bytes.Length);
                    length = (int)cloneStream.Length;
                }
            }
        }

        string base64 = Convert.ToBase64String(bytes, 0, length);
        if (string.IsNullOrEmpty(base64))
            throw new Exception("Invalid image data");
        return $"data:image/png;base64,{base64}";
    }
}
