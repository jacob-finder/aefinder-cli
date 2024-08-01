using CommandLine;

namespace AeFinder.Cli.Options;

public class CommonOptions
{
    [Option(longName: "appid", Required = true, HelpText = "The appid of the AeFinder App.")]
    public string AppId { get; set; }

    [Option(longName: "key", Required = true, HelpText = "The deploy key of the AeFinder App.")]
    public string Key { get; set; }

    [Option(longName: "network", Required = false, Default = AeFinderNetwork.MainNet, HelpText = "The AeFinder network (MainNet or TestNet).")]
    public AeFinderNetwork Network { get; set; }
}