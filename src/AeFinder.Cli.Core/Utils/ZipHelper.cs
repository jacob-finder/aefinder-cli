using ICSharpCode.SharpZipLib.Zip;

namespace AeFinder.Cli.Utils;

public class ZipHelper
{
    public static void ZipDirectory(string zipFileName, string sourceDirectory)
    {
        var zip = new FastZip();
        zip.CreateZip(zipFileName, sourceDirectory,true, string.Empty);
    }
    
    public static void UnZip(Stream fileStream, string targetDirectory)
    {
        var zip = new FastZip();
        zip.ExtractZip(fileStream, targetDirectory, FastZip.Overwrite.Always, null, null, null,
            false, true);
    }
}