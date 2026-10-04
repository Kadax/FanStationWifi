using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace FanStationWifi.WebApi.Extensions;

public static class AuthExtensions
{
    public static WebApplicationBuilder AddApiKeyAuth(this WebApplicationBuilder builder)
    {
        var apiKey = builder.Configuration.GetSection("ApiSettings").GetValue<string>("ApiKey");

        if (string.IsNullOrEmpty(apiKey))
        {
            builder.Services.AddAuthentication("ApiKey")
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey", options =>
                {
                    options.ApiKeyHeaderName = "X-API-KEY";
                });
        }
        else
        {
            builder.Services.AddAuthentication("ApiKey")
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey", options =>
                {
                    options.ApiKeyHeaderName = "X-API-KEY";
                });
        }

        builder.Services.AddAuthorization();

        return builder;
    }
}