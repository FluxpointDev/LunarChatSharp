namespace LunarChatSharp.Core.Servers;

[Flags]
public enum ServerPermission : ulong
{
    ChangeNickname = 1L << 0,
    CreateExpressions = 1L << 1,
    ManageExpressions = 1L << 2,
    ManageServer = 1L << 3,
    ManageApps = 1L << 4,
    Administrator = 1L << 5,
}
