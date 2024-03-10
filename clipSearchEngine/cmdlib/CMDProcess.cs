using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace cmdlib
{
    public partial class CMDProcess
    {
        private readonly CommandTemplate _ct;

        public CMDProcess(CommandTemplate ct)
        {
            _ct = ct;
        }

        public async Task<string> Screenshot(RunProcess p, string workingDir, string videoFile, string outFolder, string outFilePart, SubtitleModel sc)
        {
            await p.StartNewProcess(
                "cmd.exe",
                _ct.WeScreenshot(videoFile, outFolder, outFilePart, sc.StartOfDuration, sc.SubNumber),
                workingDir
            );
            return outFolder + outFilePart + ".webp";
        }

        public async Task VideoClip(RunProcess p, string workingDir, string videoIn, string videoOut, SubtitleModel sc)
        {
            var fro = SubtitleFileParser.Flip(sc, true);
            var to = SubtitleFileParser.Flip(sc, false);
            await p.StartNewProcess(
                "cmd.exe",
                _ct.VideoClip(videoIn, videoOut, fro, to),
                workingDir
            );
        }

        public async Task SoundClip(RunProcess p, string workingDir, string videoIn, string audioOut, SubtitleModel sc)
        {
            var fro = SubtitleFileParser.Flip(sc, true);
            var to = SubtitleFileParser.Flip(sc, false);
            await p.StartNewProcess(
                "cmd.exe",
                _ct.SoundClip(videoIn, audioOut, fro, to),
                workingDir
            );
        }

        public async Task SoundClip(RunProcess p, string workingDir, string videoIn, string audioOut, string fro, string to)
        {
            await p.StartNewProcess(
                "cmd.exe",
                _ct.SoundClip(videoIn, audioOut, fro, to),
                workingDir
            );
        }

        public async Task SubExtract(RunProcess p, string workingDir, string videoIn, string subOut)
        {
            await p.StartNewProcess(
                "cmd.exe",
                _ct.SubExtract(videoIn, subOut),
                workingDir
            );
        }

        public async Task<MetaInfoModel> VideoMetaInfo(RunProcess p, string videoFilePath)
        {
            var spl = videoFilePath.Split('\\').Last();
            var workingdir = videoFilePath.Replace(spl, "");
            var json = await p.NewBlastoise(
                "cmd.exe",
                _ct.FFProbeVideoInfo(videoFilePath),
                workingdir
            );
            if (json == null)
                return null;
            MetaInfoModel mime = JsonSerializer.Deserialize<MetaInfoModel>(json);
            return mime;
        }
    }

    public class CommandTemplate
    {
        public string FFProbeVideoInfo(string videoFilePath)
        {
            return $"/C ffprobe -i {videoFilePath} -v quiet -print_format json -show_format -show_streams -hide_banner";
        }

        public string SubExtract(string videoFilePath, string subOutput)
        {
            string z = $"/C ffmpeg -i \"{videoFilePath}\" {subOutput}";
            Console.WriteLine(z);
            return z;
        }

        public string SoundClip(string videoFilePathInput, string videoFilePathOutput, string start, string end)
        {
            string v = $"/C mpv \"{videoFilePathInput}\" -o \"{videoFilePathOutput}\" --no-video --start={start} --end={end}";
            Console.WriteLine(v);
            return v;
        }

        public string VideoClip(string videoFilePathInput, string videoFilePathOutput, string start, string end)
        {
            return $"/C mpv {videoFilePathInput} -o {videoFilePathOutput} --start={start} --end={end}";
        }

        public string WebpScreenshot(string videoFilePath, string outFolder, string outFilePart, double startPercent, int subnum)
        {
            return $"/C mpv {videoFilePath} " +
                $"--start={startPercent}% --frames=1 --screenshot-format=webp -o {outFolder}{outFilePart}{subnum}.webp";
        }

        public string WeScreenshot(string videoFilePath, string outFolder, string outFilePart, double startPercent, int subnum)
        {
            //Console.WriteLine($"/C mpv {videoFilePath} " +
            //    $"--start={startPercent}% --frames=1 --screenshot-format=jpg -o {outFolder}{outFilePart}{subnum}.jpg");

            //Console.WriteLine($"/C mpv {videoFilePath} " +
            //    $"--start={startPercent}% --frames=1 --screenshot-format=png -o {outFolder}{outFilePart}{subnum}.png");
            return $"/C mpv {videoFilePath} " +
                $"--start={startPercent}% --frames=1 --screenshot-format=png -o {outFolder}{outFilePart}{subnum}.png";
        }

        public string WexScreenshot(string videoFilePath, string outFolder, string outFilePart, double startPercent, int subnum)
        {
            Console.WriteLine($"/C mpv {videoFilePath} " +
                $"--start={startPercent}% --frames=1 --screenshot-format=jpg -o {outFolder}{outFilePart}{subnum}.jpg");
            return $"/C mpv {videoFilePath} " +
                $"--start={startPercent}% --frames=1 -o {outFolder}{outFilePart}{subnum}.jpg";
        }
    }

    public class RunProcess
    {
        public async Task StartNewProcess(string processName, string parameters, string startDir)
        {
            var proc = new Process();
            var args = new ProcessStartInfo
            {
                FileName = processName,
                Arguments = parameters,
                WorkingDirectory = startDir,
                CreateNoWindow = false,
                UseShellExecute = false
            };
            proc = Process.Start(args);
            await proc.WaitForExitAsync();
        }

        public async Task<string> NewBlastoise(string processName, string parameters, string startDir)
        {
            var proc = new Process();
            var args = new ProcessStartInfo
            {
                FileName = processName,
                Arguments = parameters,
                WorkingDirectory = startDir,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            proc = Process.Start(args);
            var kek = proc.StandardOutput.ReadToEnd();
            await proc.WaitForExitAsync();
            return kek;
        }

    }
}
