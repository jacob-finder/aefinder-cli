using AeFinder.Cli.Options;

namespace AeFinder.Cli.Services;

public interface IDevelopmentTemplateAppService
{
    Task CreatePrjectAsync(InitAppOptions options);
}