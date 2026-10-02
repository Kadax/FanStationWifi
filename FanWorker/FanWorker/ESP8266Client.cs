using FanWorker.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace FanWorker;

public class ESP8266Client
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly string _address;



    public ESP8266Client(string address, ILogger logger)
    {
        _httpClient = new HttpClient();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Address cannot be empty or whitespace", nameof(address));
        }  
        
        _address = address ?? throw new ArgumentNullException(nameof(address));

        if (_address.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || _address.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            _address = _address.TrimEnd('/');
        }
        else
        {
            _address = $"http://{_address.TrimEnd('/')}";
        }            
    }

    public async Task<FanStatusResponse> GetStatusAsync()
    {
        

        var statusUrl = $"{_address}/api/status";

        _logger.LogInformation("Fetching FanStation status from: {Address}", _address);

        try
        {
            using var response = await _httpClient.GetAsync(statusUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch FanStation status. Status code: {StatusCode}", response.StatusCode);
                return null;
            }

            var status = await response.Content.ReadFromJsonAsync<FanStatusResponse>();

            if (status == null)
            {
                _logger.LogWarning("Failed to deserialize FanStation status response");
                return null;
            }

            _logger.LogDebug("FanStation status received: Power={Power}, Direction={Direction}, Speed={Speed}, ServoPosition={ServoPosition}",
                status.Power, status.Direction, status.Speed, status.ServoPosition);

            return status;
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Timeout while fetching FanStation status");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while fetching FanStation status");
            return null;
        }
    }

    public async Task<bool> SetPowerAsync(int value)
    {
        
        var powerSetUrl = $"{_address}/api/power/set?value={value}";

        _logger.LogInformation("Sending FanStation power control: {Value}", value);

        try
        {
            using var response = await _httpClient.GetAsync(powerSetUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to set FanStation power. Status code: {StatusCode}", response.StatusCode);
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();

            if (content != "OK")
            {
                _logger.LogWarning("FanStation power control failed. Response: {Content}", content);
                return false;
            }

            _logger.LogInformation("FanStation power successfully set to {Value}", value);
            return true;
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Timeout while setting FanStation power");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while setting FanStation power");
            return false;
        }
    }

    public async Task<bool> SetSpeedAsync(int value)
    {                              

        var speedSetUrl = $"{_address}/api/speed/set?value={value}";

        _logger.LogInformation("Sending FanStation speed control: {Value}", value);

        try
        {
            using var response = await _httpClient.GetAsync(speedSetUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to set FanStation speed. Status code: {StatusCode}", response.StatusCode);
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();

            if (content != "OK")
            {
                _logger.LogWarning("FanStation speed control failed. Response: {Content}", content);
                return false;
            }

            _logger.LogInformation("FanStation speed successfully set to {Value}", value);
            return true;
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Timeout while setting FanStation speed");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while setting FanStation speed");
            return false;
        }
    }

    public async Task<bool> SetDirectionAsync(int value)
    {
        

        var directionSetUrl = $"{_address}/api/direction/set?value={value}";

        _logger.LogInformation("Sending FanStation direction control: {Value}", value);

        try
        {
            using var response = await _httpClient.GetAsync(directionSetUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to set FanStation direction. Status code: {StatusCode}", response.StatusCode);
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();

            if (content != "OK")
            {
                _logger.LogWarning("FanStation direction control failed. Response: {Content}", content);
                return false;
            }

            _logger.LogInformation("FanStation direction successfully set to {Value}", value);
            return true;
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Timeout while setting FanStation direction");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while setting FanStation direction");
            return false;
        }
    }

    public async Task<bool> SetServoAsync(int value)
    {
        
        var servoSetUrl = $"{_address}/api/servo/set?value={value}";

        _logger.LogInformation("Sending FanStation servo control: {Value}", value);

        try
        {
            using var response = await _httpClient.GetAsync(servoSetUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to set FanStation servo. Status code: {StatusCode}", response.StatusCode);
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();

            if (content != "OK")
            {
                _logger.LogWarning("FanStation servo control failed. Response: {Content}", content);
                return false;
            }

            _logger.LogInformation("FanStation servo successfully set to {Value}", value);
            return true;
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Timeout while setting FanStation servo");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while setting FanStation servo");
            return false;
        }
    }
}