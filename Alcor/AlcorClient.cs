using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Alcor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Alcor;

public class AlcorClient : IAlcorClient
{
    private readonly IConfiguration _config;
    private readonly ILogger<AlcorClient> _logger;

    private readonly Uri _defiUrl;

    public AlcorClient(IConfiguration config, ILogger<AlcorClient> logger)
    {
        _config = config;
        _logger = logger;
        _defiUrl = new Uri(_config["DefiEndpoint"]);
    }

    private HttpClient? _client;
    private List<MarketInfo>? _marketsInfo;
    private DateTime _lastUpDateTime = DateTime.Now;
    private readonly object _lock = new();
    private bool _busy;

    private async Task<T?> GetData<T>(CancellationToken cancelToken)
    {
        return await ResponseHttpAsync<T>(new Uri(_config["AlcorEndpoint"]), cancelToken).ConfigureAwait(false);
    }

    public async Task<T?> GetData<T>(string market, bool deals, CancellationToken cancelToken)
    {
        await LoadData().ConfigureAwait(false);

        var marketId = string.IsNullOrWhiteSpace(market) ? default : await GetMarketId(market).ConfigureAwait(false);
        var uri = new Uri($"{_config["AlcorEndpoint"]}{(marketId.HasValue ? marketId.Value.ToString() : string.Empty)}{(deals ? "/deals" : string.Empty)}");

        return await ResponseHttpAsync<T>(uri, cancelToken).ConfigureAwait(false);
    }

    private async Task<T?> GetData<T>(int? marketId, bool deals, CancellationToken cancelToken)
    {
        await LoadData().ConfigureAwait(false);

        var uri = new Uri($"{_config["AlcorEndpoint"]}{(marketId.HasValue ? marketId.Value.ToString() : string.Empty)}{(deals ? "/deals" : string.Empty)}");

        return await ResponseHttpAsync<T>(uri, cancelToken).ConfigureAwait(false);
    }

    [Serializable]
    private class DefiRequest
    {
        [JsonPropertyName("pairId")] 
        public int pairId;
    }

    public async Task<T?> GetDefiData<T>(int id) where T : new()
    {
        var content = JsonContent.Create(new DefiRequest { pairId = id }, MediaTypeHeaderValue.Parse("application/json"));

        var result = await ResponseHttpAsync<T?>(_defiUrl, content, CancellationToken.None).ConfigureAwait(false);
        return result != null ? result : new T();
    }

    public async Task<string> GetDefiData(int id, CancellationToken cancelToken)
    {
        var content = JsonContent.Create(new DefiRequest { pairId = id }, MediaTypeHeaderValue.Parse("application/json"));

        var result = await ResponseHttpAsync<string>(_defiUrl, content, CancellationToken.None).ConfigureAwait(false);
        return result != null ? result : string.Empty;
    }

    private async Task<T?> ResponseHttpAsync<T>(Uri server, HttpContent content, CancellationToken cancelToken)
    {
        try
        {
            _client ??= new HttpClient
            {
                BaseAddress = new Uri(server.AbsoluteUri.Replace(server.LocalPath, "")),
                Timeout = TimeSpan.FromMinutes(5)
            };

            _logger.LogDebug($"Content: {await content.ReadAsStringAsync(cancelToken).ConfigureAwait(false)}, Type: content.");
            using var request = new HttpRequestMessage(HttpMethod.Post, server.LocalPath) { Content = content };
            request.Headers.Add("Accept", "application/json");
            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancelToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                // Get the request stream and read it
                await using var httpStream = await response.Content.ReadAsStreamAsync(cancelToken).ConfigureAwait(false);
                
                T? result;
                using var sr = new StreamReader(httpStream);

                if (typeof(T) == typeof(MemoryStream))
                {
                    var ms = new MemoryStream();
                    await sr.BaseStream.CopyToAsync(ms, cancelToken).ConfigureAwait(false);
                    result = (T)Convert.ChangeType(ms, typeof(T));
                }
                else if (typeof(T) != typeof(string))
                {
                    result = await JsonSerializer.DeserializeAsync<T>(httpStream, cancellationToken: cancelToken)
                        .ConfigureAwait(false);
                }
                else
                {
                    result = (T)Convert.ChangeType(await sr.ReadToEndAsync().ConfigureAwait(false), typeof(T));
                }

                response.EnsureSuccessStatusCode();

                return result;
            }

            var data = await response.Content.ReadAsStringAsync(cancelToken).ConfigureAwait(false);

            response.Content.Dispose();

            var errorMessage = $"ResponseHttpAsync failed with code {response.StatusCode}: {data}";
            _logger.LogError(errorMessage);
            throw new AlcorException(errorMessage);
        }
        catch (WebException webException)
        {
            var responseStream = webException.Response?.GetResponseStream();
            var sbResponse = new StringBuilder("", 65536);
            sbResponse.AppendLine("WebException thrown at ResponseHttpAsync");

            if (responseStream != null)
            {
                try
                {
                    var readBuffer = new byte[1000];

                    for (; ; )
                    {
                        var intCnt = await responseStream.ReadAsync(readBuffer, 0, readBuffer.Length, cancelToken).ConfigureAwait(false);

                        if (intCnt == 0)
                            // EOF
                            break;

                        sbResponse.Append(Encoding.UTF8.GetString(readBuffer, 0, intCnt));
                    }

                    _logger.LogError(webException, sbResponse.ToString());
                    throw new AlcorException(sbResponse.ToString(), webException);
                }
                finally
                {
                    responseStream.Close();
                    _logger.LogError(webException, sbResponse.ToString());
                }
            }
            else
            {
                _logger.LogError(webException, webException.Message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(1, ex, $"Unknown {ex.GetType().FullName}");
        }

        return default;
    }

    private async Task<T?> ResponseHttpAsync<T>(Uri server, CancellationToken cancelToken)
    {
        try
        {
            _client ??= new HttpClient
            {
                BaseAddress = new Uri(server.AbsoluteUri.Replace(server.LocalPath, "")),
                Timeout = TimeSpan.FromMinutes(5)
            };

            // _logger.LogDebug($"Content: {await content.ReadAsStringAsync().ConfigureAwait(false)}");
            using var request = new HttpRequestMessage(HttpMethod.Get, server.LocalPath);
            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancelToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                // Get the request stream and read it
                await using var httpStream = await response.Content.ReadAsStreamAsync(cancelToken).ConfigureAwait(false);

                T? result;
                using var sr = new StreamReader(httpStream);

                if (typeof(T) == typeof(MemoryStream))
                {
                    var ms = new MemoryStream();
                    await sr.BaseStream.CopyToAsync(ms, cancelToken).ConfigureAwait(false);
                    result = (T)Convert.ChangeType(ms, typeof(T));
                }
                else if (typeof(T) != typeof(string))
                {
                    result = await JsonSerializer.DeserializeAsync<T>(httpStream, cancellationToken: cancelToken)
                        .ConfigureAwait(false);
                }
                else
                {
                    result = (T)Convert.ChangeType(await sr.ReadToEndAsync().ConfigureAwait(false), typeof(T));
                }

                response.EnsureSuccessStatusCode();

                return result;
            }

            var data = await response.Content.ReadAsStringAsync(cancelToken).ConfigureAwait(false);

            response.Content.Dispose();

            var errorMessage = $"ResponseHttpAsync failed with code {response.StatusCode}: {data}";
            _logger.LogError(errorMessage);
            throw new AlcorException(errorMessage);
        }
        catch (WebException webException)
        {
            var responseStream = webException.Response?.GetResponseStream();
            var sbResponse = new StringBuilder("", 65536);
            sbResponse.AppendLine("WebException thrown at ResponseHttpAsync");

            if (responseStream != null)
            {
                try
                {
                    var readBuffer = new byte[1000];

                    for (; ; )
                    {
                        var intCnt = await responseStream.ReadAsync(readBuffer, 0, readBuffer.Length, cancelToken).ConfigureAwait(false);

                        if (intCnt == 0)
                            // EOF
                            break;

                        sbResponse.Append(Encoding.UTF8.GetString(readBuffer, 0, intCnt));
                    }

                    _logger.LogError(webException, sbResponse.ToString());
                    throw new AlcorException(sbResponse.ToString(), webException);
                }
                finally
                {
                    responseStream.Close();
                    _logger.LogError(webException, sbResponse.ToString());
                }
            }
            else
            {
                _logger.LogError(webException, webException.Message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(1, ex, $"Unknown {ex.GetType().FullName}");
        }

        return default;
    }


    private async Task LoadData()
    {
        if (_marketsInfo?.Count > 0 && DateTime.Now < _lastUpDateTime.AddSeconds(60))
            return;

        if (!_busy)
        {
            try
            {
                lock (_lock)
                {
                    _busy = true;
                }

                if (_marketsInfo == null || _marketsInfo?.Count == 0)
                {
                    _marketsInfo = await GetData<List<MarketInfo>>(CancellationToken.None).ConfigureAwait(false);
                    _lastUpDateTime = DateTime.Now;
                }
            }
            finally
            {
                lock (_lock)
                {
                    _busy = false;
                }
            }
        }
        else
        {
            while (_busy)
                Thread.Sleep(500);
        }

    }

    private async Task<int?> GetMarketId(string market)
    {
        var info = _marketsInfo!.Find(m => string.Equals(m.QuoteToken.TokenSymbol.Name, market, StringComparison.InvariantCultureIgnoreCase));
        return await Task.FromResult(info?.MarketId);
    }

    public async Task<bool> IsValidMarketName(string market)
    {
        var marketId = await GetMarketId(market).ConfigureAwait(false);
        return marketId.HasValue;
    }

    public async Task<MarketInfo?> GetMarketInfo(string market, CancellationToken cancelToken)
    {
        return await GetData<MarketInfo>(market, false, CancellationToken.None).ConfigureAwait(false);
    }

    public async Task<MarketInfo?> GetMarketInfo(int marketId, CancellationToken cancelToken)
    {
        return await GetData<MarketInfo>(marketId, false, CancellationToken.None).ConfigureAwait(false);
    }

    public async Task<List<Transaction>?> GetTransactions(string market, CancellationToken cancelToken)
    {
        return await GetData<List<Transaction>>(market, true, cancelToken).ConfigureAwait(false);
    }
}