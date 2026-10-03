using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text;

namespace FanStationWifi.WebApi.Extensions;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions { }

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private const string ApiKeyHeaderName = "X-API-KEY";
    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationHandler(
        IConfiguration configuration,
        IOptions<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        System.Text.Encodings.Web.UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        _configuration = configuration;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            return AuthenticateResult.Fail("API Key не найден");
        }

        var apiKey = _configuration["ApiSettings:ApiKey"];

        if (string.IsNullOrEmpty(apiKey))
        {
            return AuthenticateResult.Fail("API Key не настроен");
        }

        var providedKeyBytes = Encoding.UTF8.GetBytes(extractedApiKey);
        var storedKeyBytes = Encoding.UTF8.GetBytes(apiKey);

        if (System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(providedKeyBytes, storedKeyBytes))
        {
            var identities = new List<System.Security.Claims.ClaimsIdentity> { new System.Security.Claims.ClaimsIdentity("ApiKey") };
            var ticket = new System.Security.Authentication.AuthenticationTicket(new System.Security.Claims.ClaimsPrincipal(identities), "ApiKey");
            return AuthenticateResult.Success(ticket);
        }

        return AuthenticateResult.Fail("Неверный API Key");
    }
}