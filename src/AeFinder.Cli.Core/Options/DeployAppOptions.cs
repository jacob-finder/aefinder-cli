using CommandLine;

namespace AeFinder.Cli.Options;

[Verb("deploy", HelpText = "Deploy AeFinder App.")]
public class DeployAppOptions : CommonOptions
{
    [Option(longName: "code", Required = true, HelpText = "The code file path of your AeFinder App.")]
    public string Code { get; set; }

    [Option(longName: "manifest", Required = true, HelpText = "The manifest file path of your AeFinder App.")]
    public string Manifest { get; set; }
}