namespace LunarChatSharp;

public static class StringExtensions
{
    public static string? ToNullOrString(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return null;

        return str;
    }
}
