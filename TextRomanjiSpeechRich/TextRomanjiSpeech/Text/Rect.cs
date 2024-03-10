using System.Windows;

namespace FlexTextLib
{
    public class RectU
    {
        public Rect rect;
        public char Char;
        public int line;

        public RectU(Rect rect, char @char, int line)
        {
            this.rect = rect;
            Char = @char;
            this.line = line;
        }

        public override string ToString()
        {
            return rect.X + ", " + rect.Y;
        }

        public bool Open { get; set; }
        public bool Cloe { get; set; }
    }
}
