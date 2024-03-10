using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;

namespace MPV_LAUNCHER
{
    public interface ISynthesisOutput
    {
        Task<string> Save(SpeechSynthesisResult r);
    }
}