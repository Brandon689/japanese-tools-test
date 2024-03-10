using System.Collections.Generic;

namespace FlexTextLib
{
    public class Block
    {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public List<Chunk> ItemList { get; set; }
        public List<int> Breaks { get; set; }
        public string? RawText { get; set; }
        public int fontsize { get; set; }
        public bool linebreak { get; set; }
        public Block()
        {
            ItemList = new();
            Breaks = new();
        }
        public Block(int i)
        {
            linebreak = true;
        }
    }
}
