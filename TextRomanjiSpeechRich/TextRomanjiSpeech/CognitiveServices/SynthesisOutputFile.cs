using Microsoft.CognitiveServices.Speech;
using System.IO;
using System.Threading.Tasks;

namespace MPV_LAUNCHER
{
    public class SynthesisOutputFile : ISynthesisOutput
    {
        public async Task<string> Save(SpeechSynthesisResult r)
        {
            using var stream = AudioDataStream.FromResult(r);
            string[] g = Directory.GetFiles(TempGlobals.AudioFiles);
            int id = g.Length + 1;
            //filepath cnt contain special characters inc. japanese
            //dont try to run file.create before it, that is not at all correct
            //string filePath = @"..\..\yuki.wav";
            //string filePath = @"C:\demo\yuki.wav";
            string filePath = TempGlobals.AudioFiles + $"{(id).ToString()}.wav";

            await stream.SaveToWaveFileAsync(filePath);
            r.Dispose();
            //return filePath;
            return id.ToString();
        }
    }

    public static class TempGlobals
    {
        public static string AudioFiles = @"C:\Code\MPV_LAUNCHER-3\soundfile\";
    }
}
