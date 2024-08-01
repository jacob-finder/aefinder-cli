using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Volo.Abp;

namespace AeFinder.Cli;

public class Program
{
    private static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
            .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
            .MinimumLevel.Override("Volo.Abp.IdentityModel", LogEventLevel.Information)
#if DEBUG
            .MinimumLevel.Override("AeFinder.Cli", LogEventLevel.Debug)
#else
            .MinimumLevel.Override("AeFinder.Cli", LogEventLevel.Information)
#endif
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
            .CreateLogger();

        using var application = AbpApplicationFactory.Create<AeFinderCliModule>(
            options =>
            {
                options.UseAutofac();
                options.Services.AddLogging(c => c.AddSerilog());
            });
        await application.InitializeAsync();

        await application.ServiceProvider
            .GetRequiredService<CliService>()
            .RunAsync(args);

        await application.ShutdownAsync();

        await Log.CloseAndFlushAsync();
    }
}