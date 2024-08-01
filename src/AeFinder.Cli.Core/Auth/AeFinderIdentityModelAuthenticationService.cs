using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityModel;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace AeFinder.Cli.Auth;

[Dependency(ReplaceServices = true)]
public class AeFinderIdentityModelAuthenticationService : IdentityModelAuthenticationService,
    IIdentityModelAuthenticationService, ITransientDependency
{
    public AeFinderIdentityModelAuthenticationService(IOptions<AbpIdentityClientOptions> options,
        ICancellationTokenProvider cancellationTokenProvider, IHttpClientFactory httpClientFactory,
        ICurrentTenant currentTenant,
        IOptions<IdentityModelHttpRequestMessageOptions> identityModelHttpRequestMessageOptions,
        IDistributedCache<IdentityModelTokenCacheItem> tokenCache,
        IDistributedCache<IdentityModelDiscoveryDocumentCacheItem> discoveryDocumentCache,
        IAbpHostEnvironment abpHostEnvironment) : base(options, cancellationTokenProvider, httpClientFactory,
        currentTenant, identityModelHttpRequestMessageOptions, tokenCache, discoveryDocumentCache, abpHostEnvironment)
    {
    }

    protected override async Task<TokenResponse> GetTokenResponse(IdentityClientConfiguration configuration)
    {
        using var httpClient = HttpClientFactory.CreateClient(HttpClientName);
        AddHeaders(httpClient);

        switch (configuration.GrantType)
        {
            case OidcConstants.GrantTypes.ClientCredentials:
                return await RequestClientCredentialsTokenAsync(httpClient,
                    await CreateClientCredentialsTokenRequestAsync(configuration),
                    CancellationTokenProvider.Token
                );
            case OidcConstants.GrantTypes.Password:
                return await httpClient.RequestPasswordTokenAsync(
                    await CreatePasswordTokenRequestAsync(configuration),
                    CancellationTokenProvider.Token
                );

            case OidcConstants.GrantTypes.DeviceCode:
                return await RequestDeviceAuthorizationAsync(httpClient, configuration);

            default:
                throw new AbpException("Grant type was not implemented: " + configuration.GrantType);
        }
    }

    private async Task<TokenResponse> RequestClientCredentialsTokenAsync(HttpMessageInvoker client,
        ClientCredentialsTokenRequest request, CancellationToken cancellationToken = default)
    {
        var clone = request.Clone();

        clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.ClientCredentials);
        clone.Parameters.AddRequired(OidcConstants.TokenRequest.ClientId, request.ClientId);
        clone.Parameters.AddRequired(OidcConstants.TokenRequest.ClientSecret, request.ClientSecret);
        clone.Parameters.AddOptional(OidcConstants.TokenRequest.Scope, request.Scope);

        foreach (var resource in request.Resource)
        {
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.Resource, resource, allowDuplicates: true);
        }

        return await RequestTokenAsync(client, clone, cancellationToken);
    }

    private static async Task<TokenResponse> RequestTokenAsync(HttpMessageInvoker client, ProtocolRequest request,
        CancellationToken cancellationToken = default)
    {
        request.Prepare();
        request.Method = HttpMethod.Post;

        HttpResponseMessage response;
        try
        {
            response = await client.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            return ProtocolResponse.FromException<TokenResponse>(ex);
        }

        return await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(response);
    }
}