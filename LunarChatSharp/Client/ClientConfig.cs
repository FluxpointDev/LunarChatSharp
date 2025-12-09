using System.Net;

namespace LunarChatSharp;

/// <summary>
/// Config options for the LunarChatSharp lib.
/// </summary>
public class ClientConfig
{
    /// <summary>
    /// Set your own custom client name to show in the user agent.
    /// </summary>
    public string? ClientName = "Default";

    /// <summary>
    /// Set a proxy for http rest calls.
    /// </summary>
    public IWebProxy? RestProxy = null;

    /// <summary>
    /// Set a proxy for the websocket itself.
    /// </summary>
    public IWebProxy? WebSocketProxy = null;

    internal string? UserAgent { get; set; }

    /// <summary>
    /// Do not change this.
    /// </summary>
    /// <remarks>
    /// Defaults to https://lunar.fluxpoint.dev/api/
    /// </remarks>
    public string ApiUrl = "https://lunar.fluxpoint.dev/api/";


    /// <summary>
    /// Useful for owner checks and also used for RequireOwnerAttribute when using the built-in command handler.
    /// </summary>
    public string[] Owners = null;

    public bool OwnerBypassPermissions { get; set; }

}