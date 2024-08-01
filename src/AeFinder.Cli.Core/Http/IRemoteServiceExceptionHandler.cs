namespace AeFinder.Cli;

public interface IRemoteServiceExceptionHandler
{
    Task EnsureSuccessfulHttpResponseAsync(HttpResponseMessage responseMessage);

    Task<string> GetAbpRemoteServiceErrorAsync(HttpResponseMessage responseMessage);
}