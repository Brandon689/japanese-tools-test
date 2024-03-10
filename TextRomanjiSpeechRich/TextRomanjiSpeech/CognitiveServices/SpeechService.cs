using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MPV_LAUNCHER
{
    public class SpeechService
    {
        private readonly SynthesizeSpeech _syn;
        private readonly SynthesisEventState _eventState;

        public SpeechService(SynthesizeSpeech syn)
        {
            _syn = syn;
            _eventState = new SynthesisEventState();
            _syn.BoundaryEvent(_eventState.Boundary);
        }

        public ICollection<ulong> mil()
        {
            return _eventState.MillisecondStarts;
        }

        public ICollection<string> mor()
        {
            return _eventState.Morphemes;
        }

        public string AddFullStopIfDoesntHave(string tex)
        {
            // if end doesnt have full stop, add it
            if (tex[tex.Length - 1] != '。')
            {
                tex += '。';
            }
            return tex;
        }

        public async Task<Recording> FullSpeakOut(string text)
        {
            //Recording recording = new();
            //recording.Text = text;

            string punctuationRemove = PunctuationMarks.Cleanse(text);
            // i believe this need be fed into event state and of course speak
            // event state need match speak
            var fullstop = AddFullStopIfDoesntHave(punctuationRemove);
            _eventState.SetText(fullstop);
            (int offset, string ssml) ssml = _syn.MakeSSMLString(fullstop);
            _eventState.SetOffset((uint)ssml.offset);
            var recording = await _syn.SpeakAsync(ssml.ssml, text);
            recording.Morphemes = _eventState.Morphemes;
            recording.MillisecondTime = _eventState.MillisecondStarts;
            recording.Offsets = _eventState.Offsets;
            return recording;
        }
    }

    public static class PunctuationMarks
    {
        public static string Cleanse(string input)
        {
            for (int i = 0; i < PunctuationList.Count; i++)
            {
                input = input.Replace(PunctuationList[i].ToString(), "");
            }
            return input;
        }

        public static readonly ReadOnlyCollection<char> PunctuationList = new List<char> {
            '\n',
            '\r',
            ' ',
            '？',
            '。',
            '！',
            '!',
            '?',
            '（',
            '）',
            ')',
            '(',
            '～',
            '…',
            '…',
            '‥',
            '「',
            '」',
            '一',
            '｛',
            '｝',
            '］',
            '【',
            '、',
            '，',
            '゠',
            '＝',
            '『',
            '』',
            '〝',
            '〟',
            '”',
            '“',
            '"',
            '⟨',
            '⟩',
            '〜',
            '：',
            '♪'
          }.AsReadOnly();
    }
}
