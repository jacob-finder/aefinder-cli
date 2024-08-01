namespace AeFinder.Cli;

public static class CliConsts
{
    public static readonly Dictionary<AeFinderNetwork, AeFinderEndpoint> AeFinderEndpoints =
        new()
        {
            {
                AeFinderNetwork.MainNet, new AeFinderEndpoint
                {
                    AuthEndpoint = "https://indexer-auth.aefinder.io/",
                    ApiEndpoint = "https://indexer-api.aefinder.io/"
                }
            },
            {
                AeFinderNetwork.TestNet, new AeFinderEndpoint
                {
                    AuthEndpoint = "https://gcptest-indexer-auth.aefinder.io/",
                    ApiEndpoint = "https://gcptest-indexer-api.aefinder.io/"
                }
            }
        };

    public const string HttpClientName = "AeFinderHttpClient";
}