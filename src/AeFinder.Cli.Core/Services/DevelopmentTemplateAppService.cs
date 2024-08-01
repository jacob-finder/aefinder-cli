using System.Text;
using AeFinder.Cli.Auth;
using AeFinder.Cli.Http;
using AeFinder.Cli.Options;
using AeFinder.Cli.Utils;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.Json;
using Volo.Abp.Threading;

namespace AeFinder.Cli.Services;

public class DevelopmentTemplateAppService : IDevelopmentTemplateAppService, ITransientDependency
{
    private readonly IAuthService _authService;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly CliHttpClientFactory _cliHttpClientFactory;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    private readonly IRemoteServiceExceptionHandler _remoteServiceExceptionHandler;
    private readonly ILogger<DevelopmentTemplateAppService> _logger;

    public DevelopmentTemplateAppService(IAuthService authService, IJsonSerializer jsonSerializer,
        CliHttpClientFactory cliHttpClientFactory, ICancellationTokenProvider cancellationTokenProvider,
        ILogger<DevelopmentTemplateAppService> logger, IRemoteServiceExceptionHandler remoteServiceExceptionHandler)
    {
        _authService = authService;
        _jsonSerializer = jsonSerializer;
        _cliHttpClientFactory = cliHttpClientFactory;
        _cancellationTokenProvider = cancellationTokenProvider;
        _logger = logger;
        _remoteServiceExceptionHandler = remoteServiceExceptionHandler;
    }

    public async Task CreatePrjectAsync(InitAppOptions options)
    {
        _logger.LogInformation("Initialize app...");
        
        var token = await _authService.GetAccessTokenAsync(options.Network, options.AppId, options.Key);
        
        var postData = _jsonSerializer.Serialize(new
        {
            Name = options.Name
        });
        var url = $"{CliConsts.AeFinderEndpoints[options.Network].ApiEndpoint}api/dev-template";

        var client = _cliHttpClientFactory.CreateClient(token);

        using var response = await client.PostAsync(
            url,
            new StringContent(postData, Encoding.UTF8, MimeTypes.Application.Json),
            _cancellationTokenProvider.Token
        );
        
        await _remoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(response);

        if (options.Directory.IsNullOrWhiteSpace())
        {
            options.Directory = Directory.GetCurrentDirectory();
        }
            
        var result = await response.Content.ReadAsStreamAsync();
        ZipHelper.UnZip(result, options.Directory);
            
        _logger.LogInformation("The AeFinder App: {app} is initialized successfully. Directory: {directory}", options.Name, options.Directory);
    }
}