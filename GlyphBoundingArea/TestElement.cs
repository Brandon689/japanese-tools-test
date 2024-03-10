using System.Windows;
using System.Windows.Media;

namespace GlyphBoundingArea
{
    class TestElement : FrameworkElement
    {
        GlyphTypeface gtf;
        float fontSize = 70;
        Point _origin;
        protected override void OnRender(DrawingContext drawingContext)
        {
            var type2 = new Typeface("Consolas");
            //var type2 = new Typeface("Segoe Print");
            //var type2 = new Typeface("Helvetica Neue LT Pro 55 Roman");
            type2.TryGetGlyphTypeface(out gtf);
            string src = "Bounding boxes";
            _origin = new Point(10, 200);

            List<(ushort, double)> rects = new();
            foreach (var item in gtf.AdvanceHeights)
            {
                ushort x = item.Key;
                double m = item.Value;

                var c = gtf.AdvanceWidths[x];
                var op = gtf.AdvanceHeights[x] + gtf.TopSideBearings[x] + gtf.BottomSideBearings[x];
                rects.Add((x, op));
            }

            var o = rects.OrderByDescending(x => x.Item2);

            for (int i = 0; i < src.Length; i++)
            {

                char a = src[i];
                var c = gtf.CharacterToGlyphMap[a];
                ushort index = c;

                var bbh = gtf.Height;
                var bsb = gtf.BottomSideBearings[index];
                var tsb = gtf.TopSideBearings[index];
                var fullHeight = bbh - bsb - tsb;

                var depth = gtf.DistancesFromHorizontalBaselineToBlackBoxBottom[index];
                var height = fullHeight - depth;
                var width = gtf.AdvanceWidths[index];

                var ff = Run(c);

                var originX = _origin.X;
                originX = 0;
                var gp = ff.GlyphOffsets;

                var glyphOffset = gp[0];

                double horBaselineOriginY = -glyphOffset.Y;

                double left, right, bottom, top;
                var baseline = gtf.Baseline;
                var advanceheight = gtf.AdvanceHeights[index];
                var topsidebearing = gtf.TopSideBearings[index];
                var bottomsidebearing = gtf.BottomSideBearings[index];

                var dhb = gtf.DistancesFromHorizontalBaselineToBlackBoxBottom[index];

                //left = originX + gtf.LeftSideBearings[index];
                //right = originX + gtf.AdvanceWidths[index] - gtf.RightSideBearings[index];
                left = gtf.LeftSideBearings[index];
                right = gtf.AdvanceWidths[index] - gtf.RightSideBearings[index];
                //bottom = horBaselineOriginY + emGlyphMetrics.Baseline;
                bottom = horBaselineOriginY + baseline;
                top = baseline - advanceheight + topsidebearing + bottomsidebearing;
                //top = bottom - advanceheight + topsidebearing + bottomsidebearing;
                top *= fontSize;
                bottom *= fontSize;
                left *= fontSize;
                right *= fontSize;
                dhb *= fontSize;
                var eg = bottom - top;

                var t = new Rect(
                        left,
                        top,
                        right,
                        bottom - top
                    );

                double inflation = Math.Min(fontSize / 7.0, 1.0);
                t.Inflate(inflation, inflation);


                //top = bottom - emGlyphMetrics.AdvanceHeight + emGlyphMetrics.TopSideBearing + emGlyphMetrics.BottomSideBearing;

                var r = ff.ComputeAlignmentBox();
                var r4 = ff.ComputeInkBoundingBox();

                //Glyph g = new(400, 100, new Point(30, 200), glyphTypeFace);
                drawingContext.DrawGlyphRun(Brushes.Black, ff);
                //cx.DrawRectangle(null, new Pen(Brushes.Black, 1), g.Bounding());


                //double x1, y1, x2, y2;

                //x1 = _origin.X;
                //y1 = _origin.Y;
                //x2 = _origin.X - 5;
                //y2 = _origin.Y - 5;

                double x1, y1, wid, hei;

                x1 = _origin.X;
                y1 = _origin.Y - r4.Height;
                //wid = r4.Width;
                //hei = r4.Height;
                wid = t.Width;
                hei = t.Height;

                //Rect g = new Rect(_origin.X, _origin.Y - r4.Height, r4.Width, r4.Height);

                Rect g = new Rect(x1, y1 + dhb, wid, hei);

                //drawingContext.DrawRectangle(null, new Pen(Brushes.Black, 1), r);
                drawingContext.DrawRectangle(null, new Pen(Brushes.Black, 1), g);

                //drawingContext.DrawLine(new Pen(Brushes.Red, 1), _origin, new Point(500, 200));

                //drawingContext.DrawLine(new Pen(Brushes.Green, 1), new Point(0, r4.Bottom), new Point(500, r4.Bottom));
                _origin.X += gtf.AdvanceWidths[index] * fontSize;
            }


            //drawingContext.DrawLine(new Pen(Brushes.Red, 1), new Point(40, 80), new Point(40, 80 - m(bbh)));
            //cx.DrawLine(new Pen(Brushes.Black, 1), new Point(0, 200), new Point(2220, 200));
            //var ee = glyphTypeFace.Baseline;
            //var ee2 = glyphTypeFace.Height;
            //var e3 = glyphTypeFace.XHeight;
            //var e33 = glyphTypeFace.TopSideBearings;
            //double min = 99;
            //double max = 0;
        }

        private GlyphRun Run(ushort makeme)
        {
            double xx = 0;
            Point[] glyphOffsets = new Point[1];
            ushort[] glyphIndexes = new ushort[1];
            double[] AdvanceWidths = new double[1];

            double w = 0;
            for (int i = 0; i < 1; i++)
            {
                glyphIndexes[i] = makeme;
                glyphOffsets[i] = new Point(xx, 0);
                AdvanceWidths[i] = gtf.AdvanceWidths[glyphIndexes[i]] * fontSize;
                w += AdvanceWidths[i];
            }
            return NewGlyphRun(glyphIndexes, _origin, AdvanceWidths, glyphOffsets);
        }

        private GlyphRun Run(string text)
        {
            double xx = 0;
            Point[] glyphOffsets = new Point[text.Length];
            ushort[] glyphIndexes = new ushort[text.Length];
            double[] AdvanceWidths = new double[text.Length];

            double w = 0;
            for (int i = 0; i < text.Length; i++)
            {
                glyphIndexes[i] = gtf.CharacterToGlyphMap[text[i]];
                glyphOffsets[i] = new Point(xx, 0);
                AdvanceWidths[i] = gtf.AdvanceWidths[glyphIndexes[i]] * fontSize;
                w += AdvanceWidths[i];
            }
            return NewGlyphRun(glyphIndexes, _origin, AdvanceWidths, glyphOffsets);
        }
        private GlyphRun NewGlyphRun(ushort[] glyphIndexes, Point origin, double[] advanceWidths, Point[] offsets)
        {
            return new GlyphRun(gtf,
                0,
                false,
                fontSize,
                2.5f,
                glyphIndexes,
                origin,
                advanceWidths,
                offsets,
                null, null, null, null, null);
        }
    }
}
