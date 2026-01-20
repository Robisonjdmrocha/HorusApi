using HorusV2.Core.Helpers;
using HorusV2.Core.Models;
using HorusV2.HorusIntegration.DTO.Response;
using HorusV2.HorusIntegration.Settings;
using Microsoft.Extensions.Options;
using Serilog;

namespace HorusV2.HorusIntegration.Core;

public class AccessTokenManager
{
    private readonly HorusIntegrationSettings _settings;
    private AuthenticationResponseDTO _integrationAccess;
    private DateTime? _tokenExpirationTime;

    public AccessTokenManager(IOptions<HorusIntegrationSettings> horusIntegrationSettings)
    {
        _settings = horusIntegrationSettings.Value;
    }

    public async Task<AuthenticationResponseDTO> GetIntegrationAccess()
    {
        try
        {
            if (!ShouldRequestNewToken()) return _integrationAccess;

            HttpRequestModel request = new()
            {
                BaseAddress = _settings.AuthUri,
                RequestMethod = "POST"
            };

            request.AddBasicAuthentication(_settings.UserAccess, _settings.Password);

            _integrationAccess = await HttpRequestHelper.MakeRequest<AuthenticationResponseDTO>(request);

            _tokenExpirationTime = DateTime.UtcNow.ConvertToBrazilianTime()
                .AddMilliseconds(_integrationAccess.expires_in - 300000);

            return _integrationAccess;
        }
        catch (Exception ex)
        {
            string error = $"Falha ao se autenticar no Hórus. Erro: {ex.Message}.";

            Log.Error(error);

            throw;
        }
    }

    private bool ShouldRequestNewToken()
    {
        if (_integrationAccess is null) return true;
        return DateTime.UtcNow.ConvertToBrazilianTime() > _tokenExpirationTime;
    }
}