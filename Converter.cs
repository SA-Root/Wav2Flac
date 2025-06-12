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
            WavFiles.AsParallel().ForAll(file =>
            {
                try
                {
                    var flacFile = Path.ChangeExtension(file, ".flac");
                    if (File.Exists(flacFile))
                    {
                        return;
                    }
                    var p = Process.Start($"ffmpeg.exe", $"-loglevel warning -i \"{file}\" \"{flacFile}\"");
                    p.WaitForExit();
                    if (p.ExitCode == 0)
                    {
                        File.Delete(file); // Delete the original wav file
                        Console.WriteLine($"Converted: {file}");
                    }
                    else
                        Console.WriteLine($"Failed to convert: {file}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to start process. Error: {e.Message}");
                }
            });
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