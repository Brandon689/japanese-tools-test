using Microsoft.CognitiveServices.Speech;
using System;
using System.Threading.Tasks;

namespace MPV_LAUNCHER
{
    public class SynthesizeSpeech
    {
        public SpeechSynthesizer s;
        private readonly SpeechConfig _config;
        private readonly SynthesisOutputFile _man;

        public SynthesizeSpeech(SynthesisOutputFile man)
        {
            _config = SpeechConfig.FromSubscription("fcd8fd85aa4441c3ba18ac64af0eeef2", "australiaeast");
            _config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Riff24Khz16BitMonoPcm);
            s = new SpeechSynthesizer(_config, null);
            _man = man;

            /// TEST OF PATIENCE
            s.VisemeReceived += (s, e) =>
            {
                var ok = e.Animation;
                Console.WriteLine($"Viseme event received. Audio offset: " +
                    $"{e.AudioOffset / 10000}ms, viseme id: {e.VisemeId}.");
            };

        }

        public void BoundaryEvent(EventHandler<SpeechSynthesisWordBoundaryEventArgs> o)
        {
            s.WordBoundary += o;
        }

        public SpeechSynthesizer get()
        {
            return s;
        }

        public (int offset, string ssml) MakeSSMLString(string read)
        {
            var openingBrace = $@"<speak version=""1.0"" xmlns=""http://www.w3.org/2001/10/synthesis"" xmlns:mstts=""https://www.w3.org/2001/mstts"" xml:lang=""ja-JP""><voice name=""ja-JP-NanamiNeural"">";
            //var openingBraceEnglish = $@"<speak version=""1.0"" xmlns=""http://www.w3.org/2001/10/synthesis"" xmlns:mstts=""https://www.w3.org/2001/mstts"" xml:lang=""en-US""><voice name=""en-US-JennyNeural"">";
            int offset = openingBrace.Length;
            var ssml = openingBrace + $"{read}</voice></speak>";
            return (offset, ssml);
        }

        public async Task<Recording> SpeakAsync(string ssml, string plainText)
        {
            var sm1l = ssml;

            string fileid = "";
            var result = await s.SpeakSsmlAsync(ssml);
            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                fileid = await _man.Save(result);
            }
            else if (result.Reason == ResultReason.Canceled)
            {
            }

            Recording recording = new()
            {
                FileId = int.Parse(fileid),
                Text = plainText,
                //MorphemesStr = ArrToJson(mor()),
                //MillisecondTimeStr = ArrToJson(mil())
                //Morphemes = s.mor().ToList(),
                //MillisecondTime = s.mil().ToList()
            };
            return recording;
        }

    }
}
