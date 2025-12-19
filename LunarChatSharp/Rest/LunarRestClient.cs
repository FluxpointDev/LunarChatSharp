using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace LunarChatSharp.Rest;

public class LunarRestClient
{
    public LunarRestClient(LunarClient client)
    {
        Client = client;
        HttpClientHandler ClientHandler = new HttpClientHandler();
        if (client.Config.RestProxy != null)
        {
            ClientHandler.UseProxy = true;
            ClientHandler.Proxy = client.Config.RestProxy;
        }

        Url = client.Config.ApiUrl;
        Http = new HttpClient(ClientHandler)
        {
            BaseAddress = new Uri(Url)
        };
        try
        {
            Http.DefaultRequestHeaders.Add("Accept", "application/json");
            Http.DefaultRequestHeaders.Add("User-Agent", "LunarChatSharp");
        }
        catch { }
    }

    internal LunarClient Client { get; private set; }

    public string Url;
    public HttpClient Http;

    public Task<HttpResponseMessage> SendRequestAsync(RequestType method, string endpoint, ILunarRequest? json = null)
    => InternalRequest(GetMethod(method), endpoint, json);

    public Task<TResponse?> SendRequestAsync<TResponse>(RequestType method, string endpoint, Dictionary<string, object>? json) where TResponse : class
        => InternalJsonRequest<TResponse>(GetMethod(method), endpoint, json);

    public Task<TResponse?> SendRequestAsync<TResponse>(RequestType method, string endpoint, Dictionary<string, string> json) where TResponse : class
        => InternalJsonRequest<TResponse>(GetMethod(method), endpoint, json);

    public Task<TResponse?> GetAsync<TResponse>(string endpoint, ILunarRequest json = null, bool throwGetRequest = false) where TResponse : class
        => SendRequestAsync<TResponse>(RequestType.Get, endpoint, json, throwGetRequest);

    public Task DeleteAsync(string endpoint, ILunarRequest json = null)
        => SendRequestAsync(RequestType.Delete, endpoint, json);

    public Task<TResponse> DeleteAsync<TResponse>(string endpoint, ILunarRequest json = null) where TResponse : class
        => SendRequestAsync<TResponse>(RequestType.Delete, endpoint, json)!;

    public Task<TResponse> PatchAsync<TResponse>(string endpoint, ILunarRequest json = null) where TResponse : class
        => SendRequestAsync<TResponse>(RequestType.Patch, endpoint, json)!;

    public Task PatchAsync(string endpoint, ILunarRequest json = null)
        => SendRequestAsync(RequestType.Patch, endpoint, json);

    public Task<TResponse> PutAsync<TResponse>(string endpoint, ILunarRequest json = null) where TResponse : class
        => SendRequestAsync<TResponse>(RequestType.Put, endpoint, json)!;

    public Task PutAsync(string endpoint, ILunarRequest json = null)
        => SendRequestAsync(RequestType.Put, endpoint, json);

    public Task<TResponse> PostAsync<TResponse>(string endpoint, MultipartFormDataContent form = null, bool isWebhookRequest = false) where TResponse : class
        => InternalJsonRequest<TResponse>(HttpMethod.Post, endpoint, form, false, isWebhookRequest)!;

    public Task<TResponse> PostAsync<TResponse>(string endpoint, ILunarRequest json = null, bool isWebhookRequest = false) where TResponse : class
        => SendRequestAsync<TResponse>(RequestType.Post, endpoint, json, false, isWebhookRequest)!;

    public Task<TResponse> PostAsync<TResponse>(string endpoint) where TResponse : class
        => SendRequestAsync<TResponse>(RequestType.Post, endpoint);

    public Task PostAsync(string endpoint, ILunarRequest json = null)
        => SendRequestAsync(RequestType.Post, endpoint, json);

    public Task<TResponse?> SendRequestAsync<TResponse>(RequestType method, string endpoint, ILunarRequest json = null, bool throwGetRequest = false, bool isWebhookRequest = false) where TResponse : class
        => InternalJsonRequest<TResponse>(GetMethod(method), endpoint, json, throwGetRequest, isWebhookRequest);


    internal static HttpMethod GetMethod(RequestType method)
    {
        switch (method)
        {
            case RequestType.Post:
                return HttpMethod.Post;
            case RequestType.Delete:
                return HttpMethod.Delete;
            case RequestType.Patch:
                return HttpMethod.Patch;
            case RequestType.Put:
                return HttpMethod.Put;
            case RequestType.Get:
                break;
        }
        return HttpMethod.Get;
    }

    internal static JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
    };

    internal static MediaTypeHeaderValue JsonHeader = new MediaTypeHeaderValue("application/json");

    internal async Task<HttpResponseMessage> InternalRequest(HttpMethod method, string endpoint, object? request)
    {
        if (endpoint.StartsWith("/"))
            endpoint = endpoint.Substring(1);

        HttpRequestMessage Mes = new HttpRequestMessage(method, Url + endpoint);
        if (request != null)
        {
            Mes.Content = JsonContent.Create(request, mediaType: JsonHeader, options: JsonOptions);
        }

        HttpResponseMessage Req = await Http.SendAsync(Mes);

        if (Req.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            //RetryRequest Retry = null;
            //if (Req.Content.Headers.ContentLength.HasValue)
            //{
            //    using (Stream Stream = await Req.Content.ReadAsStreamAsync())
            //    {
            //        Retry = DeserializeJson<RetryRequest>(Stream);
            //    }
            //}

            //if (Retry != null)
            //{
            //    await Task.Delay(Retry.retry_after + 2);
            //    HttpRequestMessage MesRetry = new HttpRequestMessage(method, Url + endpoint);
            //    if (request != null)
            //        MesRetry.Content = Mes.Content;
            //    Req = await Http.SendAsync(MesRetry);
            //}
        }


        if (method != HttpMethod.Get && !Req.IsSuccessStatusCode)
        {
            //RestError Error = null;
            //if (Req.Content.Headers.ContentLength.HasValue)
            //{
            //    try
            //    {
            //        using (Stream Stream = await Req.Content.ReadAsStreamAsync())
            //        {
            //            Error = DeserializeJson<RestError>(Stream);
            //        }
            //    }
            //    catch { }
            //}
            //if (Error != null)
            //{
            //    if (string.IsNullOrEmpty(Error.Permission))
            //        throw new StoatRestException($"Request failed due to {Error.Type} ({(int)Req.StatusCode})", (int)Req.StatusCode, Error.Type) { Permission = Error.Permission };
            //    else
            //        throw new StoatPermissionException(Error.Permission, (int)Req.StatusCode, Error.Type == StoatErrorType.MissingUserPermission);
            //}
            //else
            throw new LunarRestException(Req.ReasonPhrase, (int)Req.StatusCode);
        }

        return Req;
    }

    internal async Task<TResponse?> InternalJsonRequest<TResponse>(HttpMethod method, string endpoint, object request, bool throwGetRequest = false, bool isWebhookRequest = false)
        where TResponse : class
    {
        if (endpoint.StartsWith("/"))
            endpoint = endpoint.Substring(1);

        HttpRequestMessage Mes = new HttpRequestMessage(method, isWebhookRequest ? endpoint : Url + endpoint);
        if (request != null)
        {
            if (request is MultipartFormDataContent part)
                Mes.Content = part;
            else
                Mes.Content = JsonContent.Create(request, mediaType: JsonHeader, options: JsonOptions);
        }
        HttpResponseMessage Req = await Http.SendAsync(Mes);

        if (Req.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            //RetryRequest Retry = null;
            //if (Req.Content.Headers.ContentLength.HasValue)
            //{
            //    try
            //    {
            //        using (Stream Stream = await Req.Content.ReadAsStreamAsync())
            //        {
            //            Retry = DeserializeJson<RetryRequest>(Stream);
            //        }
            //    }
            //    catch { }
            //}

            //if (Retry != null)
            //{
            //    //Client.InvokeLog($"Retrying request: {endpoint} for {Retry.retry_after}s", StoatLogSeverity.Warn);
            //    await Task.Delay(Retry.retry_after + 2);
            //    HttpRequestMessage MesRetry = new HttpRequestMessage(method, Url + endpoint);
            //    if (request != null)
            //        MesRetry.Content = Mes.Content;
            //    Req = await Http.SendAsync(MesRetry);
            //}

        }

        if (endpoint == "/" && !Req.IsSuccessStatusCode)
        {
            throw new LunarRestException("The Lunar API is down. Please try again later.", 500);
        }


        if (!Req.IsSuccessStatusCode && (throwGetRequest || method != HttpMethod.Get))
        {
            //RestError Error = null;
            //if (Req.Content.Headers.ContentLength.HasValue)
            //{
            //    try
            //    {
            //        using (Stream Stream = await Req.Content.ReadAsStreamAsync())
            //        {
            //            Error = DeserializeJson<RestError>(Stream);
            //        }
            //    }
            //    catch { }
            //}
            //if (Client.Config.Debug.LogRestRequest)
            //    Client.Logger.LogRestMessage(Req, method, endpoint);

            //if (Error != null)
            //{
            //    if (string.IsNullOrEmpty(Error.Permission))
            //        throw new LunarRestException($"Request failed due to {Error.Type} ({(int)Req.StatusCode})", (int)Req.StatusCode, Error.Type) { Permission = Error.Permission };
            //    else
            //        throw new StoatPermissionException(Error.Permission, (int)Req.StatusCode, Error.Type == StoatErrorType.MissingUserPermission);
            //}
            //else
            throw new LunarRestException(Req.ReasonPhrase, (int)Req.StatusCode);
        }

        TResponse? Response = null;
        if (Req.IsSuccessStatusCode)
        {
            string Data = await Req.Content.ReadAsStringAsync();
            try
            {
                using (Stream Stream = await Req.Content.ReadAsStreamAsync())
                {
                    Response = await JsonSerializer.DeserializeAsync<TResponse>(Stream, JsonOptions);
                }
            }
            catch (Exception ex)
            {
                throw new LunarRestException("Failed to parse json response: " + ex.Message, 500);
            }
        }
        return Response;
    }

}
