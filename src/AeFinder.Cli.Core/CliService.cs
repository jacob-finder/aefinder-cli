using System.Reflection;
using AeFinder.Cli.Options;
using AeFinder.Cli.Services;
using CommandLine;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace AeFinder.Cli;

public class CliService : ITransientDependency
{
    private readonly IDevelopmentTemplateAppService _developmentTemplateAppService;
    private readonly IAppService _appService;
    private readonly ILogger<CliService> _logger;

    public CliService(
        ILogger<CliService> logger, IDevelopmentTemplateAppService developmentTemplateAppService,
        IAppService appService)
    {
        _logger = logger;
        _developmentTemplateAppService = developmentTemplateAppService;
        _appService = appService;
    }

    public async Task RunAsync(string[] args)
    {
        var types = LoadVerbs();
        
        await Parser.Default.ParseArguments(args,types)
            .WithParsedAsync(RunAsync);
    }
    
    private	static Type[] LoadVerbs()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();		 
    }

    private async Task RunAsync(object obj)
    {
        var commonOptions = obj as CommonOptions;
        _logger.LogInformation("Network : {network}", commonOptions.Network);
        _logger.LogInformation("AppId   : {appid}", commonOptions.AppId);
        
        switch (obj)
        {
            case InitAppOptions options:
                await _developmentTemplateAppService.CreateProjectAsync(options);
                break;
            case DeployAppOptions options:
                await _appService.DeployAsync(options);
                break;
            case UpdateAppOptions options:
                await _appService.UpdateAsync(options);
                break;
        }
    }
}