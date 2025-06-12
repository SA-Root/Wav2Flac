using Wav2Flac;

if (args.Length >= 1)
    Converter.ConvertWavToFlac(args[0].Trim('\"'));
else
    Console.WriteLine("Invalid args.");