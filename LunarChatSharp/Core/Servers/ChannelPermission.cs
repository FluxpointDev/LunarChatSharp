namespace LunarChatSharp.Core.Servers;

[Flags]
public enum ChannelPermission : ulong
{
    ViewChannel = 1L << 0,
    ReadMessageHistory = 1L << 1,
    SendMessages = 1L << 2,
    EmbedLinks = 1L << 3,
    AttachFiles = 1L << 4,
    AddReactions = 1L << 5,
    CreateInvites = 1L << 6,
    SendPolls = 1L << 7,
    UseExternalEmojis = 1L << 8,
    UseAppCommands = 1L << 9,
    MentionEveryone = 1L << 10,
    ManageMessages = 1L << 11,
    ManagePins = 1L << 12,
    BypassSlowmode = 1L << 13,
    ManageChannel = 1L << 14,
    ManageChannelPermissions = 1L << 15,
    ManageWebhooks = 1L << 16,
}
