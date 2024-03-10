using Microsoft.CognitiveServices.Speech;
using System.Collections.Generic;

namespace MPV_LAUNCHER
{
    public class SynthesisEventState
    {
        private uint Offset;
        //public ICollection<ulong> MillisecondStarts { get; }
        //public ICollection<string> Morphemes { get; }
        public List<ulong> MillisecondStarts { get; set; }
        public List<uint> Offsets { get; set; }
        public List<string> Morphemes { get; set; }
        private string Text;

        public SynthesisEventState()
        {
            MillisecondStarts = new();
            Morphemes = new();
            Offsets = new();
            Text = string.Empty;
        }

        public void Boundary(object? sender, SpeechSynthesisWordBoundaryEventArgs e)
        {
            uint startPos = e.TextOffset - Offset;
            string morpheme = Text.Substring((int)startPos, (int)e.WordLength);
            if (startPos != 0)
            {
                ulong millisecond = (e.AudioOffset / 10000);
                MillisecondStarts.Add(millisecond);
            }
            Offsets.Add(e.TextOffset - Offset);
            Morphemes.Add(morpheme);
        }

        //TEMP move to static helper

        public void j(string tex)
        {
            // if end doesnt have full sstop, add it
            if (Text[Text.Length - 1] != '。')
            {
                //Text += '。';
            }
            // if end morpeheme has full stop
            // this is dumb do u really need to check morpeheme list as well
            // it hsould always be there no? or not if i decide to get rid of it
            //string resin = Morphemes.Last();
            //if (resin[resin.Length - 1] == '。')
            //{
            //    Fullstop();
            //}
        }

        //public void Fullstop()
        //{
        //    int count = Morphemes.Count;
        //    string x = Morphemes.Last();
        //    Morphemes[count - 1] = x.Substring(0, x.Length - 1);

        //    if (Morphemes.Last() == "")
        //    {
        //        Morphemes.RemoveAt(count - 1);
        //    }
        //}

        public void SetText(string t)
        {
            Text = t;
            //j(Text);
            Morphemes.Clear();
            MillisecondStarts.Clear();
        }

        public void SetOffset(uint offset)
        {
            Offset = offset;
        }
    }
}
