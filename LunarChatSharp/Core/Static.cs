namespace LunarChatSharp;

public static class Static
{
    public static int MaxNameLength = 32;
    public static int MaxRoles = 100;
    public static int MaxChannels = 100;
    public static int MaxEmojis = 100;
    public static int MaxDescriptionLength = 500;
    public static int MaxLinkLength = 300;
    public static int MaxMessageContentLength = 2048;
    public static bool IsLink(string? text)
    {

        if (!string.IsNullOrEmpty(text) && (text.StartsWith("https://") || text.StartsWith("http://")) && text.Contains("."))
            return true;

        return false;
    }
}
