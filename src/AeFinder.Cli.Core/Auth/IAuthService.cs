namespace AeFinder.Cli.Auth;

public interface IAuthService
{
    Task<string> GetAccessTokenAsync(AeFinderNetwork network, string clientId, string clientSecret);
}