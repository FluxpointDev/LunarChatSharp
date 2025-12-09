using LunarChatSharp.Rest.Channels;
using LunarChatSharp.Rest.Dev;
using LunarChatSharp.Rest.Messages;
using LunarChatSharp.Rest.Roles;
using LunarChatSharp.Rest.Servers;
using LunarChatSharp.Rest.Users;
using LunarChatSharp.Websocket.Events.Account;
using LunarChatSharp.Websocket.Events.Roles;
using LunarChatSharp.Websocket.Events.Servers;

namespace LunarChatSharp.Client;

public class ClientEvents
{
    public Func<RestChannel, RestMessage, Task>? OnMessageRecieved;
    public Func<RestServer, Task>? OnAddServer;
    public Func<RestServer, Task>? OnRemoveServer;
    public Func<ServerUpdateEvent, Task>? OnServerUpdate;
    public Func<RestServer?, Task>? OnSelectServer;
    public Func<RestChannel, RestRelation, Task>? OnSelectChannel;

    public Func<RestRelation, Task>? OnRelationAdd;
    public Func<RestRelation, RelationUpdateEvent, Task>? OnRelationUpdate;
    public Func<RestRelation, Task>? OnRelationRemove;

    public Func<RestChannel, RestMessage, Task>? OnMessageEdit;
    public Func<RestChannel, RestMessage, Task>? OnMessageDelete;
    public Func<RestUserPresence, Task>? OnPresenceUpdate;
    public Func<AccountUpdateEvent, Task>? OnAccountUpdate;

    public Func<RestServer, RestRole, Task>? OnRoleCreate;
    public Func<RestServer, RestRole, RoleUpdateEvent, Task>? OnRoleUpdate;
    public Func<RestServer, RestRole, Task>? OnRoleDelete;

    public Func<RestServer, RestEmoji, Task>? OnEmojiCreate;
    public Func<RestServer, RestEmoji, EmojiUpdateEvent, Task>? OnEmojiUpdate;
    public Func<RestServer, RestEmoji, Task>? OnEmojiDelete;

    public Func<RestServer, RestApp, Task>? OnAppAdd;
    public Func<RestServer, RestApp, AppUpdatedEvent, Task>? OnAppUpdate;
    public Func<RestServer, RestApp, Task>? OnAppRemove;
}
