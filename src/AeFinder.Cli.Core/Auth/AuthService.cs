using IdentityModel;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityModel;

namespace AeFinder.Cli.Auth;

public class AuthService : IAuthService, ITransientDependency
{
    private readonly IIdentityModelAuthenticationService _authenticationService;

    private string _accessToken = string.Empty;

    public AuthService(IIdentityModelAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<string> GetAccessTokenAsync(AeFinderNetwork network, string clientId, string clientSecret)
    {
        var configuration = new IdentityClientConfiguration(
            CliConsts.AeFinderEndpoints[network].AuthEndpoint,
            "AeFinder",
            clientId,
            clientSecret,
            OidcConstants.GrantTypes.ClientCredentials,
            requireHttps: false,
            validateEndpoints: false
        );

        return await _authenticationService.GetAccessTokenAsync(configuration);
    }
}