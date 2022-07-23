using System.Net;
using System.Text;
using System.Text.Json;

namespace PinupWarlords;

public class EosrioClient
{
    private readonly ILogger _logger;

    public EosrioClient(ILogger logger)
    {
        _logger = logger;
    }

    private HttpClient? _client;
    private const string Server = "https://wax.eosrio.io/";

    public async Task<T?> GetData<T>(string api, CancellationToken cancelToken)
    {
        return await ResponseHttpAsync<T>(new Uri($"{Server}{api}"), cancelToken).ConfigureAwait(false);
    }

    public async Task<T?> GetData<T>(Uri api, CancellationToken cancelToken)
    {
        return await ResponseHttpAsync<T>(api, cancelToken).ConfigureAwait(false);
    }

    private async Task<T?> ResponseHttpAsync<T>(Uri server, CancellationToken cancelToken)
    {
        try
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(server.AbsoluteUri.Replace(server.PathAndQuery, "")),
                Timeout = TimeSpan.FromMinutes(5)
            };

            using var request = new HttpRequestMessage(HttpMethod.Get, server.PathAndQuery);
            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancelToken)
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                // Get the request stream and read it
                await using var httpStream =
                    await response.Content.ReadAsStreamAsync(cancelToken).ConfigureAwait(false);

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
            throw new Exception(errorMessage);
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

                    for (;;)
                    {
                        var intCnt = await responseStream.ReadAsync(readBuffer, 0, readBuffer.Length, cancelToken)
                            .ConfigureAwait(false);

                        if (intCnt == 0)
                            // EOF
                            break;

                        sbResponse.Append(Encoding.UTF8.GetString(readBuffer, 0, intCnt));
                    }

                    _logger.LogError(webException, sbResponse.ToString());
                    throw new Exception(sbResponse.ToString(), webException);
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
        catch (JsonException jex)
        {
            // log the json on error
            var json = await ResponseHttpAsync<string>(server, cancelToken).ConfigureAwait(false);
            _logger.LogCritical($"Failed json deserialization:\r\n{json}");
        }
        catch (Exception ex)
        {
            _logger.LogError(1, ex, $"Unknown {ex.GetType().FullName}");
        }

        return default;
    }

}