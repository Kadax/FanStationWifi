using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FanStationWifi.WebApi.Extensions;

public static class AuthExtensions
{
    public static WebApplicationBuilder AddApiKeyAuth(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration.GetSection("ApiSettings");
        var expectedApiKey = configuration["ApiKey"];
        var expectedApiKeyHash = configuration["ApiKeyHash"];

        builder.Services.AddAuthentication("ApiKey")
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey", options =>
            {
                options.ApiKeyHeaderName = "X-API-KEY";
            });

        builder.Services.AddAuthorization();

        return builder;
    }
}