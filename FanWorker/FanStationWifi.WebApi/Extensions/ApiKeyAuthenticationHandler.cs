using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

namespace FanStationWifi.WebApi.Extensions;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions {

    public const string DefaultScheme = "ApiKey";

    // Свойство, определяющее, в каком HTTP-заголовке искать ключ
    public string ApiKeyHeaderName { get; set; } = "X-API-KEY";
}

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{                                                      
    //public const string ApiKeyHeaderName = "X-API-KEY";
    private readonly IConfiguration _configuration;    
    private string ApiKeyHeaderName = "X-API-KEY";

    public ApiKeyAuthenticationHandler(
        IConfiguration configuration,
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        System.Text.Encodings.Web.UrlEncoder encoder)
        : base(options, logger, encoder)
    {
        _configuration = configuration;
        ApiKeyHeaderName = options.CurrentValue.ApiKeyHeaderName;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue( ApiKeyHeaderName, out var extractedApiKey))
        {
            return AuthenticateResult.Fail("API Key не найден");
        }

        var apiKey = _configuration["ApiSettings:ApiKey"];

        if (string.IsNullOrEmpty(apiKey))
        {
            return AuthenticateResult.Fail("API Key не настроен");
        }

        var providedKeyBytes = Encoding.UTF8.GetBytes(extractedApiKey!);
        var storedKeyBytes = Encoding.UTF8.GetBytes(apiKey!);

        if (System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(providedKeyBytes, storedKeyBytes))
        {
            var identities = new List<System.Security.Claims.ClaimsIdentity> { new System.Security.Claims.ClaimsIdentity("ApiKey") };
            var ticket = new AuthenticationTicket(new System.Security.Claims.ClaimsPrincipal(identities), "ApiKey");
            return AuthenticateResult.Success(ticket);
        }

        return AuthenticateResult.Fail("Неверный API Key");
    }
}