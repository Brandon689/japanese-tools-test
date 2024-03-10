using System.Collections.Generic;
using System.Windows.Media;

namespace FlexTextLib
{
    public class Chunk
    {
        public string GlyphRunString { get; set; }
        public bool IsAccent { get; set; }
        public string Tag { get; set; } // set to a defined part such as noun, verb or other naming entimology
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public List<RectU> Vinland { get; set; }
        public GlyphRun GlyphRun { get; set; }

        public Chunk()
        {
            Vinland = new();
            GlyphRun = new();
        }
    }
}
