using System.Diagnostics;

namespace Wav2Flac;

public class Converter
{
    public static void ConvertWavToFlac(string inPath)
    {
        WavFiles.Clear();
        DetectFiles(inPath);
        if (WavFiles.Count > 0)
        {
            Console.WriteLine($"Found {WavFiles.Count} wav files.");
            // WavFiles.AsParallel().ForAll(file =>
            // {
            //     var flacFile = Path.ChangeExtension(file, ".flac");
            //     var p = Process.Start($"./ffmpeg.exe -i \"{file}\" \"{flacFile}\"");
            //     p.WaitForExit();
            //     if (p.ExitCode == 0)
            //         Console.WriteLine($"Converted: {file}");
            //     else
            //         Console.WriteLine($"Failed to convert: {file}");
            // });
        }
    }

    private static List<string> WavFiles = [];

    private static void DetectFiles(string path)
    {
        var di = new DirectoryInfo(path);
        var files = di.GetFiles().Where(x => x.Extension == ".wav");
        WavFiles.AddRange(files.Select(x => x.FullName));
        foreach (var dir in di.GetDirectories())
        {
            DetectFiles(dir.FullName);
        }
    }
}