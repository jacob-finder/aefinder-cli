using AeFinder.Cli.Options;

namespace AeFinder.Cli.Services;

public interface IDevelopmentTemplateAppService
{
    Task CreateProjectAsync(InitAppOptions options);
}