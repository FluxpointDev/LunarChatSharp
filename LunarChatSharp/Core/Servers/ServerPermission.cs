namespace LunarChatSharp.Core.Servers;

[Flags]
public enum ServerPermission : ulong
{
    CreateInvites = 1L << 0,

}
