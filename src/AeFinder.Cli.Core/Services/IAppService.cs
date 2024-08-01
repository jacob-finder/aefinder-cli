using AeFinder.Cli.Options;

namespace AeFinder.Cli.Services;

public interface IAppService
{
    Task DeployAsync(DeployAppOptions options);
    Task UpdateAsync(UpdateAppOptions options);
}