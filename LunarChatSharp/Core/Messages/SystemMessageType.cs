namespace LunarChatSharp.Core.Messages;

public enum SystemMessageType
{
    MemberJoined,
    MemberLeft,
    MemberBanned,
    MemberUnbanned,
    MemberKicked,
    MemberTimedout,
    AppAdded,
    AppRemoved,
    GroupUserAdded,
    GroupUserRemoved,
    GroupUserLeft,
    GroupOwnershipChanged,
    GroupNameChanged,
    GroupTopicChanged
}
