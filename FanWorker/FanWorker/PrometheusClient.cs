using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace FanModelDB
{
    public class PrometheusClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private static readonly Regex PrometheusRegex = new(@"^[a-zA-Z_:][a-zA-Z0-9_:]*$");

        public PrometheusClient(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<string>> GetSensorsAsync(string serverAddress, int maxRetries = 3, int timeoutSeconds = 30)
        {
            if (string.IsNullOrWhiteSpace(serverAddress))
            {
                throw new ArgumentException("Server address cannot be empty or whitespace", nameof(serverAddress));
            }

            if (serverAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || serverAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                serverAddress = serverAddress.TrimEnd('/');
            }
            else
            {
                serverAddress = $"http://{serverAddress.TrimEnd('/')}";
            }

            _logger.LogInformation("Starting Prometheus sensors fetch from: {ServerAddress}", serverAddress);

            var labelValuesUrl = $"{serverAddress}/api/v1/label/__name__/values";

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    _logger.LogDebug("Attempt {Attempt} to get sensors from {ServerAddress}", attempt, serverAddress);

                    using var requestMessage = new HttpRequestMessage(HttpMethod.Get, labelValuesUrl);
                    requestMessage.Headers.Add("Accept", "application/json");

                    using var response = await _httpClient.SendAsync(requestMessage);
                    
                    response.EnsureSuccessStatusCode();

                    var labelValuesResponse = await response.Content.ReadFromJsonAsync<LabelValuesResponse>();
                    
                    if (labelValuesResponse == null)
                    {
                        _logger.LogWarning("Received null response from Prometheus");
                        throw new InvalidOperationException("Received null response from Prometheus");
                    }

                    if (labelValuesResponse.Data == null)
                    {
                        _logger.LogWarning("Received empty data from Prometheus");
                        return new List<string>();
                    }

                    var sensors = new List<string>();
                    foreach (var sensorName in labelValuesResponse.Data)
                    {
                        if (string.IsNullOrWhiteSpace(sensorName))
                            continue;

                        if (!PrometheusRegex.IsMatch(sensorName))
                        {
                            _logger.LogWarning("Skipping invalid Prometheus metric name: {SensorName}", sensorName);
                            continue;
                        }

                        sensors.Add(sensorName);
                    }

                    _logger.LogInformation("Successfully fetched {Count} sensors from Prometheus", sensors.Count);
                    return sensors;
                }
                catch (HttpRequestException ex) when (ex.StatusCode.HasValue && ex.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    _logger.LogWarning("Prometheus service unavailable at attempt {Attempt} of {MaxRetries}: {Message}",
                        attempt, maxRetries, ex.Message);
                    if (attempt == maxRetries)
                    {
                        throw new OperationCanceledException($"Prometheus service unavailable after {maxRetries} attempts", ex);
                    }
                }
                catch (TaskCanceledException ex)
                {
                    _logger.LogWarning("Request timeout at attempt {Attempt} of {MaxRetries}: {Message}",
                        attempt, maxRetries, ex.Message);
                    if (attempt == maxRetries)
                    {
                        throw new OperationCanceledException($"Request timeout after {maxRetries} attempts", ex);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error fetching sensors at attempt {Attempt} of {MaxRetries}: {Message}",
                        attempt, maxRetries, ex.Message);
                    if (attempt == maxRetries)
                    {
                        throw new InvalidOperationException($"Failed to fetch sensors after {maxRetries} attempts: {ex.Message}", ex);
                    }
                }

                if (attempt < maxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2 * attempt));
                }
            }

            throw new InvalidOperationException($"Failed to fetch sensors after {maxRetries} attempts");
        }

        public async Task<double?> GetSensorValueAsync(string serverAddress, string sensorName, int maxRetries = 3, int timeoutSeconds = 30)
        {
            if (string.IsNullOrWhiteSpace(serverAddress))
            {
                throw new ArgumentException("Server address cannot be empty or whitespace", nameof(serverAddress));
            }

            if (string.IsNullOrWhiteSpace(sensorName))
            {
                throw new ArgumentException("Sensor name cannot be empty or whitespace", nameof(sensorName));
            }

            if (!PrometheusRegex.IsMatch(sensorName))
            {
                throw new ArgumentException($"Invalid Prometheus metric name: {sensorName}", nameof(sensorName));
            }

            if (serverAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || serverAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                serverAddress = serverAddress.TrimEnd('/');
            }
            else
            {
                serverAddress = $"http://{serverAddress.TrimEnd('/')}";
            }

            _logger.LogInformation("Starting sensor value fetch: {SensorName} from {ServerAddress}", sensorName, serverAddress);

            var queryUrl = $"{serverAddress}/api/v1/query?query={Uri.EscapeDataString(sensorName)}";

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    _logger.LogDebug("Attempt {Attempt} to get value for {SensorName}", attempt, sensorName);

                    using var requestMessage = new HttpRequestMessage(HttpMethod.Get, queryUrl);
                    requestMessage.Headers.Add("Accept", "application/json");
                    

                    using var response = await _httpClient.SendAsync(requestMessage);
                    
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogInformation("Sensor {SensorName} not found in Prometheus", sensorName);
                        return null;
                    }

                    response.EnsureSuccessStatusCode();

                    var queryResponse = await response.Content.ReadFromJsonAsync<QueryResponse>();
                    
                    if (queryResponse == null)
                    {
                        _logger.LogWarning("Received null response from Prometheus for sensor {SensorName}", sensorName);
                        throw new InvalidOperationException($"Received null response from Prometheus for sensor {sensorName}");
                    }

                    if (queryResponse.Status != "success")
                    {
                        _logger.LogWarning("Prometheus query failed with status: {Status}, error: {Error}",
                            queryResponse.Status, queryResponse.Error);
                        if (queryResponse.Error != null)
                        {
                            throw new InvalidOperationException($"Prometheus query error: {queryResponse.Error}");
                        }
                        return null;
                    }

                    if (queryResponse.Data == null || queryResponse.Data.Result == null || queryResponse.Data.Result.Count == 0)
                    {
                        _logger.LogInformation("No data found for sensor {SensorName}", sensorName);
                        return null;
                    }

                    var lastResult = queryResponse.Data.Result.Last();

                    if (lastResult.Value == null || lastResult.Value.Count < 2)
                    {
                        _logger.LogWarning("Invalid value format for sensor {SensorName}", sensorName);
                        return null;
                    }

                    if (!double.TryParse(lastResult.Value[1]!.ToString().Replace(".", ","), out var value))
                    {
                        _logger.LogWarning("Failed to parse sensor value for {SensorName}: {Value}",
                            sensorName, lastResult.Value[1]);
                        return null;
                    }

                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        _logger.LogWarning("Got NaN or Infinity value for sensor {SensorName}: {Value}", sensorName, value);
                        return null;
                    }

                    _logger.LogInformation("Successfully fetched value {Value} for sensor {SensorName}", value, sensorName);
                    return value;
                }
                catch (HttpRequestException ex) when (ex.StatusCode.HasValue && ex.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    _logger.LogWarning("Prometheus service unavailable at attempt {Attempt} of {MaxRetries} for sensor {SensorName}: {Message}",
                        attempt, maxRetries, sensorName, ex.Message);
                    if (attempt == maxRetries)
                    {
                        throw new OperationCanceledException($"Prometheus service unavailable for sensor {sensorName} after {maxRetries} attempts", ex);
                    }
                }
                catch (TaskCanceledException ex)
                {
                    _logger.LogWarning("Request timeout at attempt {Attempt} of {MaxRetries} for sensor {SensorName}: {Message}",
                        attempt, maxRetries, sensorName, ex.Message);
                    if (attempt == maxRetries)
                    {
                        throw new OperationCanceledException($"Request timeout for sensor {sensorName} after {maxRetries} attempts", ex);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error fetching value for sensor {SensorName} at attempt {Attempt} of {MaxRetries}: {Message}",
                        sensorName, attempt, maxRetries, ex.Message);
                    if (attempt == maxRetries)
                    {
                        throw new InvalidOperationException($"Failed to fetch value for sensor {sensorName} after {maxRetries} attempts: {ex.Message}", ex);
                    }
                }

                if (attempt < maxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2 * attempt));
                }
            }

            throw new InvalidOperationException($"Failed to fetch value for sensor {sensorName} after {maxRetries} attempts");
        }

        public async Task<T?> GetDataAsync<T>(string serverAddress, string endpoint, string? query = null, int maxRetries = 3, int timeoutSeconds = 30)
        {
            if (string.IsNullOrWhiteSpace(serverAddress))
            {
                throw new ArgumentException("Server address cannot be empty or whitespace", nameof(serverAddress));
            }

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentException("Endpoint cannot be empty or whitespace", nameof(endpoint));
            }

            if (serverAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || serverAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                serverAddress = serverAddress.TrimEnd('/');
            }
            else
            {
                serverAddress = $"http://{serverAddress.TrimEnd('/')}";
            }

            var url = query == null 
                ? $"{serverAddress}{endpoint}" 
                : $"{serverAddress}{endpoint}?{Uri.EscapeDataString(query)}";

            _logger.LogDebug("Making GET request to: {Url}", url);

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    _logger.LogDebug("Attempt {Attempt} to fetch data from {Url}", attempt, url);

                    using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                    requestMessage.Headers.Add("Accept", "application/json");


                    using var response = await _httpClient.SendAsync(requestMessage);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Received HTTP {StatusCode} when fetching from {Url}", response.StatusCode, url);
                        if (attempt == maxRetries)
                        {
                            throw new HttpRequestException($"HTTP {response.StatusCode} when fetching data from {url}");
                        }
                    }

                    var data = await response.Content.ReadFromJsonAsync<T>();

                    if (data == null)
                    {
                        _logger.LogWarning("Received null data from {Url}", url);
                        if (attempt == maxRetries)
                        {
                            throw new InvalidOperationException($"Received null data from {url}");
                        }
                    }

                    return data;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error fetching data at attempt {Attempt} of {MaxRetries} from {Url}: {Message}",
                        attempt, maxRetries, url, ex.Message);
                    if (attempt == maxRetries)
                    {
                        throw;
                    }
                }

                if (attempt < maxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2 * attempt));
                }
            }

            throw new InvalidOperationException($"Failed to fetch data from {url} after {maxRetries} attempts");
        }

        public HttpClient GetHttpClient() => _httpClient;

        #region DTO Models

        public class LabelValuesResponse
        {
            [JsonPropertyName("status")]
            public string? Status { get; set; }

            [JsonPropertyName("data")]
            public List<string>? Data { get; set; }

            [JsonPropertyName("errorType")]
            public string? ErrorType { get; set; }

            [JsonPropertyName("error")]
            public string? Error { get; set; }
        }

        public class QueryResponse
        {
            [JsonPropertyName("status")]
            public string? Status { get; set; }

            [JsonPropertyName("data")]
            public QueryData? Data { get; set; }

            [JsonPropertyName("errorType")]
            public string? ErrorType { get; set; }

            [JsonPropertyName("error")]
            public string? Error { get; set; }
        }

        public class QueryData
        {
            [JsonPropertyName("resultType")]
            public string? ResultType { get; set; }

            [JsonPropertyName("result")]
            public List<QueryResult>? Result { get; set; }
        }

        public class QueryResult
        {
            [JsonPropertyName("metric")]
            public Dictionary<string, string?>? Metric { get; set; }

            [JsonPropertyName("value")]
            public List<object?>? Value { get; set; }

            [JsonPropertyName("histogram")]
            public object? Histogram { get; set; }
        }

        #endregion
    }
}