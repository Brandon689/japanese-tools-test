using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ReaderAdvanced
{
    internal class FrameEl : FrameworkElement, IFrameEl
    {
        #region Public Properties
        public bool Freeze { get; set; }
        public string FullText { get; set; }
        public string HoveredText { get; private set; }
        public Brush TextBrush { get; set; }
        public Brush HoverTextBrush { get; set; }
        public Brush HighlightTextBrush { get; set; }
        public new Brush Background { get; set; }
        public double LineHeight { get; set; }
        public double LineWidth { get; set; }
        #endregion

        public FrameEl()
        {
            FullText = string.Empty;
            HoveredText = String.Empty;
            type = new Typeface("Noto Sans JP Regular");
            type.TryGetGlyphTypeface(out glyphTypeFace);
            baseline = glyphTypeFace.Baseline - 1;

            HoverTextBrush = Brushes.Red;
            HighlightTextBrush = new SolidColorBrush(Colors.LightBlue);
            HighlightTextBrush.Opacity = 0.50;
            TextBrush = new SolidColorBrush(Color.FromRgb(20, 20, 20));
            Background = Brushes.WhiteSmoke;

            LineHeight = 1.3 * FontSize;
            LineWidth = 1.65 * FontSize;
        }

        public void Clear()
        {
            blocks.Clear();
            units.Clear();
            b1 = -1;
            b2 = -1;
            fulltextwithformatting = "";
            FullText = "";
            InvalidateVisual();
        }

        public void Add(string item, bool newLine = false)
        {
            if (string.IsNullOrWhiteSpace(item)) return;
            foreach (var cha in item)
            {
                if (!glyphTypeFace.CharacterToGlyphMap.ContainsKey(cha))
                {
                    item = item.Replace(cha.ToString(), "");
                }
            }

            if (blocks.Count == 0 || newLine)
            {
                blocks.Add(new Block());
            }
            var car = From(item);
            blocks[blocks.Count - 1].StartIndex = FullText.Length;
            FullText += item;
            fulltextwithformatting += item;
            if (newLine)
            {
                fulltextwithformatting += "\n";
            }
            blocks[blocks.Count - 1].EndIndex = FullText.Length;
            blocks[blocks.Count - 1].ItemList.Add(car);
            blocks[blocks.Count - 1].RawText = item;
            FreezeFresh();
        }

        public void Cut(int startIndex, int endIndex, bool merge = true)
        {
            var aa = Math.Min(b1, b2);
            var bb = Math.Max(b1, b2);
            Block? king = null;
            for (int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i].StartIndex <= aa && blocks[i].EndIndex >= bb)
                {
                    king = blocks[i];
                }
            }
            if (king == null)
                return;
            int ss = b1 - king.StartIndex;
            int ee = ss + (b2 - b1) + 1;

            bool cont = AddCuts(king, ss, ee);
            king.Breaks.Sort();
            if (!cont) return;
            var petal = Open(king.RawText, king.Breaks);
            king.ItemList.Clear();
            for (int i = 0; i < petal.Count; i++)
            {
                king.ItemList.Add(From(petal[i]));
            }
            Vertical();
            Invalidate();
        }
        public void Cut2(int index, bool merge = true)
        {
            Block? b = null;
            if (blocks.Count == 1)
            {
                b = blocks[0];
            }
            else
            {
                for (int i = 0; i < blocks.Count; i++)
                {
                    for (int j = 0; j < blocks[i].ItemList.Count; j++)
                    {
                        if (blocks[i].EndIndex > index && blocks[i].StartIndex < index)
                        {
                            b = blocks[i];
                        }
                    }
                }
            }
            if (b == null)
                return;
            int ss = index - b.StartIndex;

            bool cont = AddCut(b, ss);
            b.Breaks.Sort();
            if (!cont) return;
            var petal = Open(b.RawText, b.Breaks);
            b.ItemList.Clear();
            for (int i = 0; i < petal.Count; i++)
            {
                b.ItemList.Add(From(petal[i]));
            }
            Vertical();
            Invalidate();
        }

        private bool AddCuts(Block b, int a, int c)
        {
            bool ok = false;
            if (a == b.RawText.Length || c == b.RawText.Length) return ok;
            if (!b.Breaks.Contains(a) && a != 0)
            {
                b.Breaks.Add(a);
                ok = true;
            }
            if (!b.Breaks.Contains(c) && c != 0)
            {
                b.Breaks.Add(c);
                ok = true;
            }
            return ok;
        }

        private bool AddCut(Block b, int a)
        {
            bool ok = false;
            if (a == b.RawText.Length) return ok;
            if (!b.Breaks.Contains(a) && a != 0)
            {
                b.Breaks.Add(a);
                ok = true;
            }
            return ok;
        }

        public List<string> Open(string rawText, List<int> breaks)
        {
            List<string> h = new();

            int last = 0;
            for (int i = 0; i < breaks.Count; i++)
            {
                int length = breaks[i] - last;
                var llone = rawText.Substring(last, length);
                h.Add(llone);
                last = breaks[i];
            }
            var mini = rawText.Substring(last);
            h.Add(mini);
            return h;
        }

        public void Invalidate()
        {
            InvalidateVisual();
        }

        public void AddRange(IEnumerable<string> items)
        {
        }

        public void Copy()
        {
            string go = "";
            for (int i = Math.Min(b1, b2); i <= Math.Max(b1, b2); i++)
            {
                go += FullText[i];
                //go += fulltextwithformatting[i];
            }
            Clipboard.SetText(go);
        }

        #region Private Logic

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            hold = false;
            InvalidateVisual();
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            b2 = -1;
            b1 = Bound1(e.GetPosition(this));
            hold = true;
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool chng = false;
            if (hold)
            {
                int ops = Bound1(e.GetPosition(this));
                if (b2 != ops && ops != -1)
                {
                    b2 = ops;
                    chng = true;
                }
            }
            var rey = Bound2(e.GetPosition(this));
            if (rey != cc || chng)
            {
                Invalidate();
                cc = rey;
            }
        }

        private void FreezeFresh()
        {
            if (!Freeze)
            {
                InvalidateVisual();
            }
        }
        List<int> ccc;

        public void Loa()
        {
            var k = db.Load();
            var t = db.LoadT().ToList();

            var w = k.Where(x => x.TextId == t[0].Id).ToList();

            foreach (var item in t[0].Text.Split("\r\n"))
            {
                Add(item, true);
            }

            for (int i = 0; i < w.Count; i++)
            {
                Cut2(w[i].Index);
            }
            Invalidate();
        }

        public void Finally()
        {
            ccc = new() { 4, 8, 16, 22, 29 };
            for (int i = 0; i < ccc.Count; i++)
            {
                Cut2(ccc[i]);
            }
        }

        private Chunk From(string item)
        {
            Chunk g = new();
            g.GlyphRunString = item;
            g.StartIndex = FullText.Length;
            //FullText += item;
            g.EndIndex = FullText.Length + item.Length;
            return g;
        }

        private int Bound1(Point p)
        {
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Rect.rect.Contains(p))
                    return i;
            }
            return -1;
        }
        private Chunk Bound2(Point p)
        {
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Rect.rect.Contains(p))
                    return units[i].C;
            }
            return null;
        }

        public void save()
        {
            TextItem ti = new();
            ti.Text = fulltextwithformatting;
            int id = db.Insert(ti);
            for (int i = 0; i < blocks.Count; i++)
            {
                for (int j = 0; j < blocks[i].ItemList.Count; j++)
                {
                    SItem so = new();
                    so.Index = blocks[i].ItemList[j].StartIndex;
                    so.RawText = blocks[i].ItemList[j].GlyphRunString;
                    so.TextId = id;
                    db.i(so);
                }
            }
        }

        private void Horizontal()
        {
            this.MinHeight = 0;
            var cloe = this.Parent as ScrollViewer;
            cloe.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            cloe.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            if (Init)
            {
                cloe.ScrollToRightEnd();
                Init = false;
            }
            units.Clear();
            double PosX = RenderSize.Width - LineHeight;
            double span = 0;
            for (int k = 0; k < blocks.Count; k++)
            {
                double PosY = 0;
                double y = 0;

                for (int j = 0; j < blocks[k].ItemList.Count; j++)
                {
                    double x = 0;
                    string text = blocks[k].ItemList[j].GlyphRunString;

                    Point[] glyphOffsets = new Point[text.Length];
                    ushort[] glyphIndexes = new ushort[text.Length];
                    double[] ZeroAdvanceWidths = new double[text.Length];
                    Point origin = new(PosX, PosY + FontSize - (baseline * FontSize));

                    blocks[k].ItemList[j].Bounds.Clear();
                    for (int i = 0; i < text.Length; i++)
                    {
                        //if (!glyphTypeFace.CharacterToGlyphMap.ContainsKey(text[i]))
                        //{
                        //    continue;
                        //}
                        double manualPosX = 0;
                        double manualPosY = 0;
                        //if (text[i] == '。')
                        //{
                        //    manualPosX = FontSize * 0.55;
                        //    manualPosY = manualPosX * 0.6;
                        //}
                        glyphIndexes[i] = glyphTypeFace.CharacterToGlyphMap[text[i]];
                        glyphOffsets[i] = new Point(x + manualPosX, y + manualPosY);
                        ZeroAdvanceWidths[i] = 0;

                        int pad = 6;
                        Rect boundingBox = new(x + origin.X - (pad / 2), -y + PosY, glyphTypeFace.AdvanceWidths[glyphIndexes[i]] * FontSize + pad, LineHeight);
                        y -= LineHeight;
                        if (i != text.Length - 1)
                        {
                            if (-y + FontSize + scrollbarWidth > RenderSize.Height)
                            {
                                span += LineWidth;
                                x -= LineWidth;
                                y = 0;
                                PosX -= LineWidth;
                            }
                        }
                        RectU r = new(boundingBox, text[i], k);
                        blocks[k].ItemList[j].Bounds.Add(r);
                        units.Add(new Unit(r, blocks[k], blocks[k].ItemList[j]));
                    }
                    blocks[k].ItemList[j].GlyphRun = NewGlyphRun(glyphIndexes, origin, ZeroAdvanceWidths, glyphOffsets);
                }
                PosX -= LineWidth;
                span += LineWidth;
            }
            //if (RenderSize.Width < span)
            {
                this.MinWidth = span + scrollbarWidth;
            }
        }

        private void Vertical()
        {
            MinWidth = 0;
            var cloe = this.Parent as ScrollViewer;
            cloe.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            cloe.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;

            units.Clear();
            double PosY = 0;
            for (int k = 0; k < blocks.Count; k++)
            {
                double y = 0;
                double PosX = 0;
                double x = 0;
                double n = 0;
                for (int j = 0; j < blocks[k].ItemList.Count; j++)
                {
                    string text = blocks[k].ItemList[j].GlyphRunString;

                    Point[] glyphOffsets = new Point[text.Length];
                    ushort[] glyphIndexes = new ushort[text.Length];
                    double[] ZeroAdvanceWidths = new double[text.Length];
                    Point origin = new(PosX, PosY + FontSize - (baseline * FontSize));

                    blocks[k].ItemList[j].Bounds.Clear();
                    for (int i = 0; i < text.Length; i++)
                    {
                        glyphIndexes[i] = glyphTypeFace.CharacterToGlyphMap[text[i]];
                        glyphOffsets[i] = new Point(x, y);
                        ZeroAdvanceWidths[i] = glyphTypeFace.AdvanceWidths[glyphIndexes[i]] * FontSize;

                        int pad = 6;
                        Rect fail = new(n, -y + PosY - (pad / 2), ZeroAdvanceWidths[i], FontSize + pad);
                        PosX += ZeroAdvanceWidths[i];
                        n += ZeroAdvanceWidths[i];
                        //Console.WriteLine(scrollbarWidth);
                        if (n + FontSize + scrollbarWidth > RenderSize.Width)
                        {
                            x -= n;
                            n = 0;
                            y -= FontSize + gapPoint;
                        }
                        RectU r = new(fail, text[i], k);
                        blocks[k].ItemList[j].Bounds.Add(r);
                        units.Add(new Unit(r, blocks[k], blocks[k].ItemList[j]));
                    }
                    blocks[k].ItemList[j].GlyphRun = NewGlyphRun(glyphIndexes, origin, ZeroAdvanceWidths, glyphOffsets);
                }
                PosY -= y;
                PosY += FontSize + gapPoint;
            }
            this.MinHeight = PosY + scrollbarWidth;
        }

        private GlyphRun NewGlyphRun(ushort[] glyphIndexes, Point origin, double[] advanceWidths, Point[] offsets)
        {
            return new GlyphRun(glyphTypeFace,
                        0,
                        false,
                        FontSize,
                        2.5f,
                        glyphIndexes,
                        origin,
                        advanceWidths,
                        offsets,
                        null, null, null, null, null);
        }

        protected sealed override void OnRender(DrawingContext ctx)
        {
            if (ORIENTATION_FLAG)
                Vertical();
            else
                Horizontal();
            ctx.DrawRectangle(Background, null, new Rect(RenderSize));

            for (int i = 0; i < blocks.Count; i++)
            {
                for (int j = 0; j < blocks[i].ItemList.Count; j++)
                {
                    if (cc != null && blocks[i].ItemList[j] == cc)
                    {
                        ctx.DrawGlyphRun(HoverTextBrush, blocks[i].ItemList[j].GlyphRun);
                    }
                    else
                    {
                        ctx.DrawGlyphRun(TextBrush, blocks[i].ItemList[j].GlyphRun);
                    }
                    for (int h = 0; h < blocks[i].ItemList[j].Bounds.Count; h++)
                    {
                        ctx.DrawRectangle(null, new Pen(Brushes.LightGray, 1), blocks[i].ItemList[j].Bounds[h].rect);
                    }
                }
            }
            if (b1 != -1 && b2 != -1)
            {
                for (int i = Math.Min(b1, b2); i <= Math.Max(b1, b2); i++)
                {
                    ctx.DrawRectangle(HighlightTextBrush, null, units[i].Rect.rect);
                    ctx.DrawLine(new Pen(Brushes.DarkCyan, 1), units[i].Rect.rect.BottomLeft, units[i].Rect.rect.BottomRight);
                }
            }
        }

        public void OrientationToggle()
        {
            ORIENTATION_FLAG = !ORIENTATION_FLAG;
        }

        #endregion

        #region Private Properties
        sql db = new();
        const int FontSize = 18;
        const double gapPoint = FontSize * 0.666;
        List<Block> blocks = new();
        List<Unit> units = new();

        Typeface type;
        GlyphTypeface glyphTypeFace;
        double baseline;
        int b1 = -1;
        int b2 = -1;
        bool hold = false;
        bool ORIENTATION_FLAG = true;
        string fulltextwithformatting = "";
        Chunk cc = null;
        int scrollbarWidth = 14;
        bool Init = true;
        #endregion
    }
}
