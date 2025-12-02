namespace LunarChatSharp.Core.Servers;

[Flags]
public enum ServerFeature : ulong
{
    Discoverable = 1L << 0,
    Community = 1L << 1,
    InvitesDisabled = 1L << 2,
    Official = 1L << 3,
    Verified = 1L << 4
}
