namespace LunarChatSharp.Core.Servers;

[Flags]
public enum ModPermission : ulong
{
    KickMembers = 1L << 0,
    BanMembers = 1L << 1,
    TimeoutMembers = 1L << 2,
    ViewAuditLogs = 1L << 3,
    AssignRoles = 1L << 4,
    ManageRoles = 1L << 5,
    ManageRolePermissions = 1L << 6,
    ManageNicknames = 1L << 7,
    ManageApprovals = 1L << 8,
    ManageAppeals = 1L << 9,
    UseModView = 1L << 10,
}
