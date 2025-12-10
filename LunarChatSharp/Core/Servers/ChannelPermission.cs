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
    ManageInvites = 1L << 7,
    SendPolls = 1L << 8,
    UseExternalEmojis = 1L << 9,
    UseAppCommands = 1L << 10,
    MentionEveryone = 1L << 11,
    ManageMessages = 1L << 12,
    ManagePins = 1L << 13,
    BypassSlowmode = 1L << 14,
    ManageChannel = 1L << 15,
    ManageChannelPermissions = 1L << 16,
    ManageWebhooks = 1L << 17,
}
