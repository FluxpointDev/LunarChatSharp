using LunarChatSharp.Core.Servers;
using System.Text.Json.Serialization;

namespace LunarChatSharp.Rest.Roles;

public class RestPermissions
{
    [JsonPropertyName("server")]
    public ServerPermission ServerPermissions { get; set; }

    [JsonPropertyName("mod")]
    public ModPermission ModPermissions { get; set; }

    [JsonPropertyName("channel")]
    public ChannelPermission ChannelPermissions { get; set; }

    [JsonPropertyName("voice")]
    public VoicePermission VoicePermissions { get; set; }

    public void SetValue(bool? value, ServerPermission flag)
            => SetValuePerm(this, value, flag);

    public void SetValue(bool? value, ChannelPermission flag)
            => SetValuePerm(this, value, flag);

    public void SetValue(bool? value, ModPermission flag)
            => SetValuePerm(this, value, flag);

    public void SetValue(bool? value, VoicePermission flag)
            => SetValuePerm(this, value, flag);

    private static void SetValuePerm(RestPermissions rawValue, bool? value, ServerPermission flag)
    {
        if (value.HasValue)
        {
            if (value == true)
                rawValue.ServerPermissions |= flag;
            else
                rawValue.ServerPermissions &= ~flag;
        }
    }

    private static void SetValuePerm(RestPermissions rawValue, bool? value, ChannelPermission flag)
    {
        if (value.HasValue)
        {
            if (value == true)
                rawValue.ChannelPermissions |= flag;
            else
                rawValue.ChannelPermissions &= ~flag;
        }
    }

    private static void SetValuePerm(RestPermissions rawValue, bool? value, ModPermission flag)
    {
        if (value.HasValue)
        {
            if (value == true)
                rawValue.ModPermissions |= flag;
            else
                rawValue.ModPermissions &= ~flag;
        }
    }

    private static void SetValuePerm(RestPermissions rawValue, bool? value, VoicePermission flag)
    {
        if (value.HasValue)
        {
            if (value == true)
                rawValue.VoicePermissions |= flag;
            else
                rawValue.VoicePermissions &= ~flag;
        }
    }

    public static void SetValue(ref ulong rawValue, bool? value, ServerPermission flag)
            => SetValue(ref rawValue, value, (ulong)flag);

    public static void SetValue(ref ulong rawValue, bool? value, ChannelPermission flag)
            => SetValue(ref rawValue, value, (ulong)flag);

    public static void SetValue(ref ulong rawValue, bool? value, ModPermission flag)
            => SetValue(ref rawValue, value, (ulong)flag);

    public static void SetValue(ref ulong rawValue, bool? value, VoicePermission flag)
            => SetValue(ref rawValue, value, (ulong)flag);

    private static void SetValue(ref ulong rawValue, bool? value, ulong flag)
    {
        if (value.HasValue)
        {
            if (value == true)
                SetFlag(ref rawValue, flag);
            else
                UnsetFlag(ref rawValue, flag);
        }
    }

    private static void SetFlag(ref ulong value, ulong flag) => value |= flag;

    private static void UnsetFlag(ref ulong value, ulong flag) => value &= ~flag;
}
