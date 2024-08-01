using System.Text;
using System.Text.RegularExpressions;
using AeFinder.Cli.Auth;
using AeFinder.Cli.Http;
using AeFinder.Cli.Options;
using AeFinder.Cli.Utils;
using Microsoft.Extensions.Logging;
using Volo.Abp;
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

    public async Task CreateProjectAsync(InitAppOptions options)
    {
        _logger.LogInformation("Initialize app...");
        
        CheckOptions(options);
        
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

    private void CheckOptions(InitAppOptions options)
    {
        if (options.Name.Length is < 2 or > 20)
        {
            throw new UserFriendlyException("The name should be between 2 and 20 in length.");
        }

        if (!Regex.IsMatch(options.Name, "[A-Za-z][A-Za-z0-9.]+"))
        {
            throw new UserFriendlyException("The Name must begin with a letter and can only contain letters('A'-'Z', 'a'-'z'), numbers(0-9), and dots('.').");
        }
    }
}