using System.Collections.Generic;
using System.Windows.Media;

namespace ReaderAdvanced
{
    public class Chunk
    {
        public string GlyphRunString { get; set; }
        public bool IsAccent { get; set; }
        public string Tag { get; set; } // set to a defined part such as noun, verb or other naming entimology
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public List<RectU> Bounds { get; set; }
        public GlyphRun GlyphRun { get; set; }

        public Chunk()
        {
            Bounds = new();
            GlyphRun = new();
        }
    }
}
