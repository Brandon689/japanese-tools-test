﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Baby
{
    public class Glyph
    {
        public int BlockIndex { get; set; }
        public Rect BoundingBox { get; set; }
        public char Letter { get; set; }
        public int ChunkIndex { get; set; }
        public int Index { get; set; }
        public bool Contains(Point p)
        {
            if (BoundingBox.Contains(p))
                return true;
            return false;
        }

        public Glyph(char c, int index, int chunkIndex, int blockIndex)
        {
            Letter = c;
            Index = index;
            BlockIndex = blockIndex;
            ChunkIndex = chunkIndex;
        }

        public override string ToString()
        {
            return ChunkIndex.ToString();
        }
    }
}
