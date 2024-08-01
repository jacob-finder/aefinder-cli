using CommandLine;

namespace AeFinder.Cli.Options;

[Verb("update", HelpText = "Update AeFinder App.")]
public class UpdateAppOptions : CommonOptions
{
    [Option(longName: "code", Required = false, HelpText = "The code file path of your AeFinder App.")]
    public string Code { get; set; }

    [Option(longName: "manifest", Required = false, HelpText = "The manifest file path of your AeFinder App.")]
    public string Manifest { get; set; }
    
    [Option(longName: "version", Required = true, HelpText = "The version to update.")]
    public string Version { get; set; }
}