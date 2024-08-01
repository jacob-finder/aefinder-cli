using CommandLine;

namespace AeFinder.Cli.Options;

[Verb("init", HelpText = "Init AeFinder App development project.")]
public class InitAppOptions : CommonOptions
{
    [Option(longName: "name", Required = true, HelpText = "The project name.")]
    public string Name { get; set; }

    [Option(longName: "directory", Required = false, HelpText = "The project directory. (Default: Current directory)")]
    public string Directory { get; set; }
}