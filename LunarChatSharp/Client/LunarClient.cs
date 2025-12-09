using LunarChatSharp.Client;
using LunarChatSharp.Rest;
using LunarChatSharp.Websocket;
using System.Threading.Channels;

namespace LunarChatSharp;

public class LunarClient : ClientEvents
{
    public LunarClient(ClientMode mode, ClientConfig? config = null)
    {
        Config = config ??= new ClientConfig();
        Mode = mode;
        ConfigSafetyChecks();
        Rest = new LunarRestClient(this);
        if (Mode == ClientMode.WebSocket)
            WebSocket = new LunarSocketClient(this);
    }

    public async Task LoginAsync(string token)
    {
        Token = token;
        Rest.Http.DefaultRequestHeaders.Add("Auth-Id", token);
        CurrentId = token;
    }

    public string? CurrentId { get; internal set; }

    private void ConfigSafetyChecks()
    {
        if (string.IsNullOrEmpty(Config.ApiUrl))
        {
            //Logger.LogMessage("Config API Url is missing.", StoatLogSeverity.Error);
            throw new LunarException("Config API Url is missing");
        }

        if (!Config.ApiUrl.EndsWith('/'))
            Config.ApiUrl += "/";

        Config.UserAgent ??= $"LunarChatSharp v ({Config.ClientName})";
        Config.Owners ??= Array.Empty<string>();
    }

    /// <summary>
    /// The current client mode that LunarChatSharp is using either Http or WebSocket
    /// </summary>
    public ClientMode Mode { get; internal set; }

    /// <summary>
    /// LunarChat bot token used for http requests and websocket.
    /// </summary>
    public string? Token { get; internal set; }

    /// <summary>
    /// Client config options for user-agent and debug options.
    /// </summary>
    public ClientConfig Config { get; internal set; }

    /// <summary>
    /// Internal rest/http client used to connect to the LunarChat API.
    /// </summary>
    /// <remarks>
    /// You can also make custom requests with <see cref="LunarRestClient.SendRequestAsync(RequestType, string, ILunarRequest)"/> and json class based on <see cref="ILunarRequest"/>
    /// </remarks>
    public LunarRestClient Rest { get; internal set; }

    /// <summary>
    /// Internal websocket client used to connect to the LunarChat gateway.
    /// </summary>
    public LunarSocketClient? WebSocket { get; internal set; }
}

/// <summary>
/// Run the client with Http requests only or use websocket to get cached data such as servers, channels and users instead of just ids.
/// </summary>
/// <remarks>
/// Using <see cref="ClientMode.Http"/> means that some data will be <see langword="null"/>
/// </remarks>
public enum ClientMode
{
    /// <summary>
    /// Client will only use the http/rest client of Lunar and removes any usage/memory of websocket stuff. 
    /// </summary>
    Http,
    /// <summary>
    /// Will use both WebSocket and http/rest client so you can get cached data for <see cref="RestUser"/>, <see cref="Server"/> and <see cref="Channel"/>
    /// </summary>
    WebSocket
}
