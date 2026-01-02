namespace LunarChatSharp;

public static class StringExtensions
{
    public static string? ToNullOrString(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return null;

        return str;
    }

    public static ulong? ToNullOrUlong(this string? value)
    {
        if (string.IsNullOrEmpty(value) || value == "0")
            return null;

        return ulong.Parse(value);
    }

    public static bool? ToNullOrTrue(this bool? value)
    {
        if (value.HasValue && value.Value)
            return true;

        return null;
    }
}
