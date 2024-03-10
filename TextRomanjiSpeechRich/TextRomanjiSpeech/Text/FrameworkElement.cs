using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TextRomanjiSpeech;
using WanaKanaNet;

namespace FlexTextLib
{
    public class FrameEl : FrameworkElement
    {
        #region Public Properties
        public bool Freeze { get; set; }
        public string FullText { get; set; }
        public string HoveredText { get; private set; }
        //public Brush TextBrush { get; set; }
        //public Brush HoverTextBrush { get; set; }
        //public Brush HighlightTextBrush { get; set; }
        //public new Brush Background { get; set; }
        public double LineHeight { get; set; }
        public double LineWidth { get; set; }
        #endregion
        public event Action<Chunk> Click;
        public event Action<Chunk> Harvest;
        public FrameEl()
        {
            FullText = string.Empty;
            HoveredText = String.Empty;
            var type = new Typeface("MS Mincho");
            type.TryGetGlyphTypeface(out glyphTypeFace);

            var type2 = new Typeface("MS Mincho");
            //var type2 = new Typeface("Noto Sans JP Bold");
            type2.TryGetGlyphTypeface(out glyphTypeFaceBold);

            baseline = glyphTypeFace.Baseline - 1;

            //HoverTextBrush = Brushes.DodgerBlue; //Brushes.Red;
            //HighlightTextBrush = new SolidColorBrush(Colors.LightBlue);
            //HighlightTextBrush.Opacity = 0.50;
            //TextBrush = new SolidColorBrush(Color.FromRgb(20,20,20));
            Background = Brushes.White;

            LineHeight = 1.19 * FontSize;
            LineWidth = 1.65 * FontSize;
            Loaded += FrameEl_Loaded;

            gapPoint = FontSize * 0.666;
        }
        public void Select(Chunk c)
        {
            cc = c;
            Invalidate();
        }

        public string CCBrain(int startIndex)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                for (int j = 0; j < blocks[i].ItemList.Count; j++)
                {
                    if (blocks[i].ItemList[j].StartIndex == startIndex)
                    {
                        cc = blocks[i].ItemList[j];
                        Invalidate();
                        return cc.GlyphRunString;
                    }
                }
            }
            return "";
        }

        private void FrameEl_Loaded(object sender, RoutedEventArgs e)
        {
            //Loa();
        }

        public void Clear()
        {
            blocks.Clear();
            ski.Clear();
            b1 = -1;
            b2 = -1;
            fulltextwithformatting = "";
            FullText = "";
            InvalidateVisual();
        }
        //public Chunk poo;
        public void Harvesting()
        {
            var g = SelectedPartition();
            Cut2(g.StartIndex);
            Cut2(g.EndIndex + 1);
            //poo = g;
            Console.WriteLine(g.GlyphRunString);
            Harvest?.Invoke(g);
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
        htmlinc ht = new htmlinc();
        int x = 0;
        List<List<Block>> bl;
        public async Task Met()
        {
            bl = await ht.f(File.ReadAllText(@"..\..\..\..\taekim.html"), glyphTypeFace);
            FullText = ht.FullText;
        }
        public async Task set()
        {

            blocks = bl[x++];
            FreezeFresh();
        }
        //public void Cut(int startIndex, int endIndex, bool merge = true)
        //{
        //    var aa = Math.Min(b1, b2);
        //    var bb = Math.Max(b1, b2);
        //    Block? king = null;
        //    for (int i = 0; i < blocks.Count; i++)
        //    {
        //        if (blocks[i].StartIndex <= aa && blocks[i].EndIndex >= bb)
        //        {
        //            king = blocks[i];
        //        }
        //    }
        //    if (king == null)
        //        return;
        //    int ss = b1 - king.StartIndex;
        //    int ee = ss + (b2 - b1) + 1;

        //    bool cont = AddCuts(king, ss, ee);
        //    king.Breaks.Sort();
        //    if (!cont) return;
        //    var petal = opem(king.RawText, king.Breaks);
        //    king.ItemList.Clear();
        //    for (int i = 0; i < petal.Count; i++)
        //    {
        //        king.ItemList.Add(From(petal[i]));
        //    }
        //    Kin();
        //    Invalidate();
        //}
        public void Cut3(int startIndex, int endIndex, bool merge = true)
        {
            Block? king = null;
            for (int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i].StartIndex <= endIndex && blocks[i].EndIndex >= endIndex)
                {
                    king = blocks[i];
                }
            }
            if (king == null)
                return;
            int ss = startIndex - king.StartIndex;
            int ee = ss + (endIndex - startIndex) + 1;

            bool cont = AddCuts(king, ss, ee);
            king.Breaks.Sort();
            if (!cont) return;
            var petal = opem(king.RawText, king.Breaks);
            king.ItemList.Clear();
            for (int i = 0; i < petal.Count; i++)
            {
                king.ItemList.Add(From(petal[i]));
            }
            Kin();
            Invalidate();
        }
        public void CutT(string texto)
        {
            var a = FullText.IndexOf(texto);
            if (a == -1)
                return;
            Cut2(a);
            Cut2(a + texto.Length);

            for (int i = 0; i < blocks.Count; i++)
            {
                for (int j = 0; j < blocks[i].ItemList.Count; j++)
                {
                    if (blocks[i].ItemList[j].GlyphRunString == texto)
                    {
                        blocks[i].ItemList[j].IsAccent = true;
                    }
                }
            }
            Invalidate();
        }

        public void Cut2(int index, bool merge = true)
        {
            Block? king = null;
            if (blocks.Count == 1)
            {
                king = blocks[0];
            }
            else
            {
                for (int i = 0; i < blocks.Count; i++)
                {
                    for (int j = 0; j < blocks[i].ItemList.Count; j++)
                    {
                        if (blocks[i].EndIndex > index && blocks[i].StartIndex < index)
                        {
                            king = blocks[i];
                        }
                    }
                }
            }
            if (king == null)
                return;
            int ss = index - king.StartIndex;

            bool cont = AddCut(king, ss);
            king.Breaks.Sort();
            if (!cont) return;
            var petal = opem(king.RawText, king.Breaks);
            int place = king.ItemList[0].StartIndex;
            king.ItemList.Clear();
            for (int i = 0; i < petal.Count; i++)
            {
                king.ItemList.Add(From(petal[i], place));
                place += petal[i].Length;
            }
            Kin();
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

        public List<string> opem(string rawText, List<int> breaks)
        {
            List<string> hero = new();

            int last = 0;
            for (int i = 0; i < breaks.Count; i++)
            {
                int length = breaks[i] - last;
                var llone = rawText.Substring(last, length);
                hero.Add(llone);
                last = breaks[i];
            }
            var mini = rawText.Substring(last);
            hero.Add(mini);
            return hero;
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

        public string SelectedText()
        {
            string go = "";
            if (b1 != -1 && b2 != -1)
            {
                for (int i = Math.Min(b1, b2); i <= Math.Max(b1, b2); i++)
                {
                    go += FullText[i];
                    //go += fulltextwithformatting[i];
                }

            }

            return go;
        }

        public Chunk SelectedPartition()
        {
            string go = "";
            var b = Math.Min(b1, b2);
            var d = Math.Max(b1, b2);
            for (int i = b; i <= d; i++)
            {
                go += FullText[i];
            }
            Chunk c = new();
            c.GlyphRunString = go;
            c.StartIndex = b;
            c.EndIndex = d;
            return c;
        }
        //public void expres()
        //{
        //    HoveredText = "";

        //    if (b1 != -1 && b2 != -1)
        //    {
        //        int y = Math.Max(b1, b2);
        //        int max = Math.Max(b1, b2);
        //        var goffset = Math.Min(b1, b2);

        //        for (int i = goffset; i <= max; i++)
        //        {
        //            HoveredText += _glyphs[i].Letter;
        //        }
        //        //_highlightTextString += _glyphs[max + 1].Letter;
        //        //Console.WriteLine(  _highlightTextString);
        //    }
        //}
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
            b1 = Vin(e.GetPosition(this));
            hold = true;
            base.OnMouseDown(e);

            if (cc != null)
            {
                Console.WriteLine(cc.GlyphRunString);
                Click?.Invoke(cc);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool chng = false;
            if (hold)
            {
                int ops = Vin(e.GetPosition(this));
                if (b2 != ops && ops != -1)
                {
                    b2 = ops;
                    chng = true;
                }
            }
            var rey = Rey(e.GetPosition(this));
            if (rey != cc || chng)
            {

                var s = SelectedText();
                romaj = WanaKana.ToRomaji(s);

                Invalidate();
                cc = rey;
            }
        }
        string romaj = "";

        private void FreezeFresh()
        {
            if (!Freeze)
            {
                InvalidateVisual();
            }
        }
        List<int> ccc;
        bool init;
        public void LoadString(string body)
        {
            if (init)
            {
                Clear();
            }
            else
            {
                init = true;
            }
            foreach (var item in body.Split("\r\n"))
            {
                Add(item, true);
            }
            Console.WriteLine("load stnig");
            Invalidate();
        }

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
                //Cut2(w[i].Index);
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

        private Chunk From(string item, int placem = -1)
        {
            Chunk g = new();
            g.GlyphRunString = item;
            //Console.WriteLine(FullText.Length);
            if (placem == -1)
            {
                g.StartIndex = FullText.Length;
                g.EndIndex = FullText.Length + item.Length;
            }
            else
            {
                g.StartIndex = placem;
                g.EndIndex = placem + item.Length;
            }
            return g;
        }

        private int Vin(Point p)
        {
            for (int i = 0; i < ski.Count; i++)
            {
                if (ski[i].Rect.rect.Contains(p))
                    return i;
            }
            return -1;
        }
        private Chunk Rey(Point p)
        {
            for (int i = 0; i < ski.Count; i++)
            {
                if (ski[i].Rect.rect.Contains(p))
                    return ski[i].C;
            }
            return null;
        }

        public void save()
        {
            TextItem totot = new();
            totot.Text = fulltextwithformatting;
            int id = db.IQ(totot);
            //List<int> ssso = new();
            //int b = 0;
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

            //;
            //for (int i = 0; i < ccc.Count; i++)
            //{
            //    SItem so = new();
            //}
        }

        private void Verdant()
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
            ski.Clear();
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

                    blocks[k].ItemList[j].Vinland.Clear();
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
                        blocks[k].ItemList[j].Vinland.Add(r);
                        ski.Add(new Unit(r, blocks[k], blocks[k].ItemList[j]));
                    }
                    blocks[k].ItemList[j].GlyphRun = NewGlyphRun(glyphIndexes, origin, ZeroAdvanceWidths, glyphOffsets);
                }
                PosX -= LineWidth;
                span += LineWidth;
            }
            //if (RenderSize.Width < span)
            {
                this.MinWidth = span + scrollbarWidth;// doing this causes this method to run again, use one time bool or hook to another event maybe?
                Console.WriteLine("cocococ");
            }
        }
        bool swap = false;
        private void Kin()
        {
            MinWidth = 0;
            var cloe = this.Parent as ScrollViewer;
            cloe.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            cloe.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;

            ski.Clear();
            double PosY = 0;
            for (int k = 0; k < blocks.Count; k++)
            {
                if (blocks[k].linebreak)
                {
                    // PosY -= y;
                    PosY += FontSize + gapPoint;
                    continue;
                }
                FontSize = blocks[k].fontsize;
                double y = 0;
                double PosX = 0;
                double x = 0;
                double n = 0;
                //if (swap)
                //{
                //    FontSize = 14;
                //}
                //else
                //{
                //    FontSize = 20;
                //}
                swap = !swap;
                for (int j = 0; j < blocks[k].ItemList.Count; j++)
                {
                    string text = blocks[k].ItemList[j].GlyphRunString;

                    Point[] glyphOffsets = new Point[text.Length];
                    ushort[] glyphIndexes = new ushort[text.Length];
                    double[] ZeroAdvanceWidths = new double[text.Length];
                    Point origin = new(PosX, PosY + FontSize - (baseline * FontSize));

                    blocks[k].ItemList[j].Vinland.Clear();
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

                        if (n > RenderSize.Width - 140)
                        {
                            if (text[i] == ' ')
                            {
                                if (text.Length > i)
                                {
                                    int len = 0;
                                    for (int h = i; h < text.Length; h++)
                                    {
                                        if (text[h] == ' ')
                                        {
                                            len = h - i;
                                            break;
                                        }
                                        len = h - i;
                                    }
                                    if (len < 5)
                                    {

                                    }
                                    else
                                    {
                                        x -= n;
                                        n = 0;
                                        y -= FontSize + gapPoint;
                                    }
                                }
                                x -= n;
                                n = 0;
                                y -= FontSize + gapPoint;
                            }
                        }

                        if (n + FontSize + scrollbarWidth > RenderSize.Width)
                        {
                            x -= n;
                            n = 0;
                            y -= FontSize + gapPoint;
                        }
                        RectU r = new(fail, text[i], k);
                        blocks[k].ItemList[j].Vinland.Add(r);
                        ski.Add(new Unit(r, blocks[k], blocks[k].ItemList[j]));
                    }
                    blocks[k].ItemList[j].GlyphRun = NewGlyphRun(glyphIndexes, origin, ZeroAdvanceWidths, glyphOffsets);
                }
                PosY -= y;
                PosY += FontSize + gapPoint;
            }
            this.MinHeight = PosY + scrollbarWidth;
        }

        private GlyphRun NewGlyphRun(ushort[] glyphIndexes, Point origin, double[] advanceWidths, Point[] offsets, bool bold = false)
        {
            if (bold)
            {
                return new GlyphRun(glyphTypeFaceBold,
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
            else
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
        }

        protected sealed override void OnRender(DrawingContext ctx)
        {
            if (MUSHISHI)
                Kin();
            else
                Verdant();
            ctx.DrawRectangle(Background, null, new Rect(RenderSize));
            Random rnd = new Random();

            //List<SolidColorBrush> sordid = new() {Brushes.RosyBrown, Brushes.RoyalBlue, Brushes.Sienna,
            //Brushes.Tomato, Brushes.YellowGreen, Brushes.Orchid, Brushes.LavenderBlush};
            List<SolidColorBrush> sordid = new() { Brushes.DarkTurquoise, Brushes.DarkViolet, Brushes.DarkKhaki, Brushes.DarkBlue, Brushes.DarkCyan, Brushes.DarkGoldenrod, Brushes.DarkGreen, Brushes.DarkMagenta, Brushes.DarkOrange, Brushes.DarkSeaGreen };
            var zz = Math.Min(b1, b2);
            // pt this block AFTER glyphrun draw if using semi transparent select and want to change text color on select
            if (b1 != -1 && b2 != -1)
            {
                for (int i = zz; i <= Math.Max(b1, b2); i++)
                {
                    ctx.DrawRectangle(HighlightBackground, null, ski[i].Rect.rect);
                    ctx.DrawLine(new Pen(Brushes.DarkCyan, 1), ski[i].Rect.rect.BottomLeft, ski[i].Rect.rect.BottomRight);
                }

                //var x = ski[zz].Rect.rect;

                //FormattedText text = new FormattedText(romaj,
                //    Thread.CurrentThread.CurrentUICulture,
                //    FlowDirection.LeftToRight,
                //    //new Typeface("UD Digi Kyokasho N-B"),
                //    new Typeface("Noto Sans JP"),
                //    18,
                //    Brushes.White, 2.5);

                //Point p = x.TopLeft;
                //p.Y -= 30;

                //Point pp = new(p.X + text.Width, p.Y + text.Height);

                //Rect r = new Rect(p, pp);
                //ctx.DrawRectangle(Background, null, r);
                //ctx.DrawText(text, p);

            }
            for (int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i].linebreak)
                {
                    continue;
                }
                int randomBrush = rnd.Next(0, sordid.Count);

                for (int j = 0; j < blocks[i].ItemList.Count; j++)
                {
                    if (cc != null && blocks[i].ItemList[j] == cc)
                    {
                        ctx.DrawGlyphRun(HighlightColor, blocks[i].ItemList[j].GlyphRun);
                    }
                    else if (blocks[i].ItemList[j].IsAccent)
                    {
                        if (sordid[randomBrush] == null)
                        {
                            ;
                        }
                        ctx.DrawGlyphRun(sordid[randomBrush], blocks[i].ItemList[j].GlyphRun);
                    }
                    else
                    {
                        ctx.DrawGlyphRun(Foreground, blocks[i].ItemList[j].GlyphRun);
                    }
                    //for (int h = 0; h < blocks[i].ItemList[j].Vinland.Count; h++)
                    //{
                    //    ctx.DrawRectangle(null, new Pen(Brushes.LightGray, 1), blocks[i].ItemList[j].Vinland[h].rect);
                    //}
                }
            }
            if (b1 != -1 && b2 != -1)
            {


                var x = ski[zz].Rect.rect;

                FormattedText text = new FormattedText(romaj,
                    Thread.CurrentThread.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    //new Typeface("UD Digi Kyokasho N-B"),
                    new Typeface("MS Mincho"),
                    18,
                    Brushes.White, 2.5);

                Point p = x.TopLeft;
                p.Y -= 30;

                Point pp = new(p.X + text.Width, p.Y + text.Height);

                Rect r = new Rect(p, pp);
                ctx.DrawRectangle(Background, null, r);
                ctx.DrawText(text, p);

            }
        }

        public void MUSHI()
        {
            MUSHISHI = !MUSHISHI;
        }

        #endregion

        #region Private Properties
        sql db = new();
        int FontSize = 24;
        //const double gapPoint = FontSize * 0.666;
        double gapPoint;
        public List<Block> blocks = new();
        List<Unit> ski = new();

        //Typeface type;
        GlyphTypeface glyphTypeFace;
        GlyphTypeface glyphTypeFaceBold;
        double baseline;
        int b1 = -1;
        int b2 = -1;
        bool hold = false;
        bool MUSHISHI = true;
        string fulltextwithformatting = "";
        Chunk cc = null;
        int scrollbarWidth = 14;
        bool Init = true;
        #endregion














        #region DP

        public bool Orientation
        {
            get { return (bool)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public bool MouseHighlightText
        {
            get { return (bool)GetValue(MouseHighlightTextProperty); }
            set { SetValue(MouseHighlightTextProperty, value); }
        }

        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public Brush HighlightBackground
        {
            get { return (Brush)GetValue(HighlightBackgroundProperty); }
            set { SetValue(HighlightBackgroundProperty, value); }
        }

        public SolidColorBrush HighlightColor
        {
            get { return (SolidColorBrush)GetValue(HighlightColorProperty); }
            set { SetValue(HighlightColorProperty, value); }
        }

        //public double FontSize
        //{
        //    get { return (double)GetValue(FontSizeProperty); }
        //    set { SetValue(FontSizeProperty, value); }
        //}

        //public static readonly DependencyProperty FontSizeProperty =
        //    DependencyProperty.Register("FontSize", typeof(double), typeof(TextElement), new PropertyMetadata(0d));

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("BackgroundProperty", typeof(Brush), typeof(TextElement), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty HighlightBackgroundProperty =
            DependencyProperty.Register("HighlightBackgroundProperty", typeof(Brush), typeof(TextElement), new PropertyMetadata(Brushes.Black));


        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("ForegroundProperty", typeof(Brush), typeof(TextElement), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register("FontWeightProperty", typeof(FontWeight), typeof(TextElement), new PropertyMetadata(FontWeights.Normal));

        public static readonly DependencyProperty HighlightColorProperty =
             DependencyProperty.Register("HighlightColorProperty", typeof(SolidColorBrush), typeof(TextElement), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(51, 153, 255))));

        public static readonly DependencyProperty MouseHighlightTextProperty =
            DependencyProperty.Register("MouseHighlightTextProperty", typeof(bool), typeof(TextElement), new PropertyMetadata(true));

        public static readonly DependencyProperty OrientationProperty =
    DependencyProperty.Register("OrientationProperty", typeof(bool), typeof(TextElement), new PropertyMetadata(true));

        #endregion










    }
}
