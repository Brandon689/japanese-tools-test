using Microsoft.CognitiveServices.Speech;
using System;
using System.Threading.Tasks;

namespace MPV_LAUNCHER
{
    public interface ISynthesizeSpeech
    {
        (int offset, string ssml) MakeSSMLString(string read);

        Task<Recording> SpeakAsync(string ssml, string plainText);

        void BoundaryEvent(EventHandler<SpeechSynthesisWordBoundaryEventArgs> o);
    }
}