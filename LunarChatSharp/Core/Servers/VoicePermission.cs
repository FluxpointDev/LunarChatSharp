namespace LunarChatSharp.Core.Servers;

[Flags]
public enum VoicePermission : ulong
{
    Connect = 1L << 0,
    Speak = 1L << 1,
    Video = 1L << 2,
    UseVoiceActivity = 1L << 3,
    MuteMembers = 1L << 4,
    DeafenMembers = 1L << 5,
    MoveMembers = 1L << 6,
    SetVoiceStatus = 1L << 7,
    RequestToSpeak = 1L << 8,
}
