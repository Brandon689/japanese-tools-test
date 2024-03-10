using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Baby
{
    public class TextElement : FrameworkElement
    {
        private bool _enabled;

        internal Text? _text;
        private GlyphRun[] _glyphRuns;
        public bool newline = true;
        public TextElement()
        {
            //Loaded += TextElement_Loaded;
            progDrawTimer.Interval = TimeSpan.FromMilliseconds(33);
            progDrawTimer.Tick += ProgDrawTimer_Tick;
        }

        public void Resize(Size size)
        {
            RenderSize = size;
        }

        public void Breakdown(int i)
        {
            _text?.Breakdown(i);
            //renders = cm.Calculate();
            InvalidateVisual();
        }
        public void Whole()
        {
            _text?.Whole();
        }
        public void Redo()
        {
            InvalidateVisual();
        }
        public string high()
        {
            return _text.high();
        }
        public void Load(List<string> birb)
        {
            string s = string.Join("\r\n", birb);
            double lineheightF = 0.7;
            double lineHeightVF = 1 + lineheightF;
            _text = new(FontSize, 1.22 * FontSize, 1.66 * FontSize);
            foreach (var item in s.Split("\r\n"))
            {
                _text.Append(item, newline);
            }
            if (Orientation)
            {
                _glyphRuns = _text.CalculateV(newline).ToArray();
            }
            else
            {
                _glyphRuns = _text.Calculate(newline).ToArray();
            }
            //_glyphRuns = _text.CalculateV(newline).ToArray();
            InvalidateVisual();
        }

        public void Load(string s)
        {
            double lineheightF = 0.7;
            double lineHeightVF = 1 + lineheightF;

            _text = new(FontSize, 1.22 * FontSize, 1.66 * FontSize);
            //            var s = @"aaaaaa
            //bbbbbb
            //cccccccc";
            foreach (var item in s.Split("\n"))
            {
                _text.Append(item, newline);
            }
            if (Orientation)
            {
                _glyphRuns = _text.CalculateV(newline).ToArray();
            }
            else
            {
                _glyphRuns = _text.Calculate(newline).ToArray();
            }
            //_glyphRuns = _text.CalculateV(newline).ToArray();
            InvalidateVisual();
        }
        //        private void TextElement_Loaded(object sender, RoutedEventArgs e)
        //        {
        //            double lineheightF = 0.7;
        //            double lineHeightVF = 1 + lineheightF;
        //            //            _text = new(FontSize, 1.22 * FontSize, 1.66 * FontSize);
        //            var s = @"ブログの閲覧数とチャンネル数とチャンネル
        //登録者数が増えるに連れコメント数も多くなってきて嬉しい
        //限りです。";
        //            _text = new(FontSize, 1.22 * FontSize, 1.66 * FontSize);
        //            //            var s = @"aaaaaa
        //            //bbbbbb
        //            //cccccccc";
        //            foreach (var item in s.Split("\r\n"))
        //            {
        //                _text.Append(item, newline);
        //            }
        //            _glyphRuns = _text.CalculateV(newline).ToArray();
        //            //_glyphRuns = _text.CalculateV(newline).ToArray();
        //            InvalidateVisual();
        //        }

        int s = 0;
        bool progressive = false;
        DispatcherTimer progDrawTimer = new DispatcherTimer();
        public void ProgressiveDrawRoutine()
        {
            progressive = true;
            ++s;
            InvalidateVisual();
            progDrawTimer.Start();
            //if (s < _glyphRuns.Length)
            //{
            //    ++s;

            //    ProgressiveDrawRoutine();
            //}
            //else
            //{
            //    progressive = false;
            //    s = 0;
            //}
        }
        //NAudioWrapper n = new();

        //Timer timer;
        //Recording _recording;
        //public void RTK(Recording recording)
        //{
        //    _recording = recording;
        //    timer = new();
        //    timer.Elapsed += Timer_Elapsed;
        //    timer.Interval = recording.MillisecondTimez[0] - 10;
        //    timer.Start();
        //    string filePath = @"C:\demo\yuki.wav";

        //    n.AudioFile(filePath);
        //    n.Play();


        //    freaky = 0;
        //    InvalidateVisual();
        //}

        //private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        //{

        //    Application.Current.Dispatcher.Invoke(new Action(() => {
        //        ++freaky;
        //        if (freaky == _recording.MillisecondTimez.Count)
        //        {
        //            timer.Stop();
        //            freaky = -1;
        //            InvalidateVisual();
        //            return;
        //        }
        //        timer.Interval = _recording.MillisecondTimez[freaky];

        //        //Console.WriteLine(freaky);
        //        InvalidateVisual();
        //    }));

        //}

        private void ProgDrawTimer_Tick(object? sender, EventArgs e)
        {
            if (s < _glyphRuns.Length)
            {
                ++s;
            }
            else
            {
                progDrawTimer.Stop();
                progressive = false;
                s = 0;
            }
            InvalidateVisual();

        }
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
            }
        }

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

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(TextElement), new PropertyMetadata(0d));

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

        #region override
        public int freaky = -1;
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_glyphRuns == null) return;

            _text._renderSize = RenderSize;
            //Resize(RenderSize);
            if (Orientation)
            {
                _glyphRuns = _text.CalculateV(newline).ToArray();
            }
            else
            {
                _glyphRuns = _text.Calculate(newline).ToArray();
            }

            //Console.WriteLine(RenderSize.Width);

            drawingContext.DrawRectangle(Background, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            //var dree = _text.HighlightedTextBoundingBoxes();
            //if (dree.Count() > 0)
            //{
            //    Rect fatgum = dree[0];
            //    PathGeometry g = new();
            //    for (int i = 1; i < dree.Count; i++)
            //    {

            //        RectangleGeometry r = new(dree[i]);
            //        RectangleGeometry r2 = new(dree[i - 1]);
            //        var p = Geometry.Combine(r, r2, GeometryCombineMode.Union, null);
            //        g = Geometry.Combine(p, g, GeometryCombineMode.Union, null);
            //        //fatgum.Union(dree[i].Location);
            //    }
            //    //drawingContext.DrawGeometry(null, new Pen(Brushes.LightGray, 1), g);
            //    //drawingContext.DrawGeometry(Brushes.Red, null, g);
            //    //drawingContext.DrawRectangle(HighlightBackground, null, fatgum);
            //}

            foreach (var item in _text.HighlightedTextBoundingBoxes())
            {
                drawingContext.DrawRectangle(HighlightBackground, null, item);
                drawingContext.DrawLine(new Pen(Brushes.DarkCyan, 1), item.BottomLeft, item.BottomRight);
            }

            int f = _glyphRuns.Length;
            if (s > f)
            {

            }
            else if (progressive)
            {
                f = s;
            }
            for (int i = 0; i < f; i++)
            {
                var brush = Foreground;
                if (MouseHighlightText && i == _text.HoveredChunkIndex)
                {
                    Console.WriteLine(i);
                    brush = HighlightColor;
                }
                if (i == freaky)
                {
                    brush = Brushes.Red;
                    Console.WriteLine(i);
                }
                drawingContext.DrawGlyphRun(brush, _glyphRuns[i]);
            }

            foreach (var item in _text._glyphs)
            {
                //drawingContext.DrawRectangle(null, new Pen(Brushes.Red, 1), item.BoundingBox);
            }


            //LinearGradientBrush fiveColorLGB = new LinearGradientBrush();
            //fiveColorLGB.StartPoint = new Point(0, 0);
            //fiveColorLGB.EndPoint = new Point(1, 1);

            //GradientStop blueGS = new GradientStop();
            //blueGS.Color = Colors.Blue;
            //blueGS.Offset = 0.0;
            //fiveColorLGB.GradientStops.Add(blueGS);

            //GradientStop orangeGS = new GradientStop();
            //orangeGS.Color = Colors.Orange;
            //orangeGS.Offset = 0.25;
            //fiveColorLGB.GradientStops.Add(orangeGS);

            //GradientStop yellowGS = new GradientStop();
            //yellowGS.Color = Colors.Yellow;
            //yellowGS.Offset = 0.50;
            //fiveColorLGB.GradientStops.Add(yellowGS);

            //GradientStop greenGS = new GradientStop();
            //greenGS.Color = Colors.Green;
            //greenGS.Offset = 0.75;
            //fiveColorLGB.GradientStops.Add(greenGS);

            //GradientStop redGS = new GradientStop();
            //redGS.Color = Colors.Red;
            //redGS.Offset = 1.0;
            //fiveColorLGB.GradientStops.Add(redGS);

            //GeometryGroup fc = new();
            //for (int i = 0; i < _glyphRuns.Length; i++)
            //{
            //    var s = _glyphRuns[i].BuildGeometry();
            //    fc.Children.Add(s);
            //    //drawingContext.DrawGeometry(fiveColorLGB, null, s);

            //}
            //drawingContext.DrawGeometry(null, new Pen(fiveColorLGB, 2), fc);
            //drawingContext.DrawGeometry(fiveColorLGB, null, fc);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            _text.MouseDown(e.GetPosition(this));
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            _text.HoveredChunkIndex = -1;
            InvalidateVisual();
            base.OnMouseLeave(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_text.MouseMove(e.GetPosition(this)))
            {
                // re-calculate
                InvalidateVisual();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            _text.MouseUp(e.GetPosition(this));
            InvalidateVisual();
        }

        #endregion

        #region mainwindow

        public void Key()
        {
            _text.Key();
            InvalidateVisual();
        }
        public void log()
        {
            _text.Log();
        }

        #endregion
    }
}
