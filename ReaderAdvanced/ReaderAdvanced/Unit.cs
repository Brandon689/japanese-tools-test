namespace ReaderAdvanced
{
    public class Unit
    {
        public Unit(RectU rect, Block b, Chunk c)
        {
            Rect = rect;
            B = b;
            C = c;
        }

        public RectU Rect { get; set; }

        // ref
        public Block B { get; set; }
        public Chunk C { get; set; }
    }
}
