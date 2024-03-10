using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;
using Color = System.Drawing.Color;
using Point = System.Windows.Point;

namespace Omoidasu
{
    public class Overlay : FrameworkElement
    {
        //DispatcherTimer dt;
        double opacity = 1;
        bool dir = true;
        public Overlay()
        {
            //dt = new();
            //dt.Interval = new TimeSpan(0, 0, 0, 0, 100);
            //dt.Tick += Dt_Tick;
            //dt.Start();
        }
        public void aaa(string k)
        {
            kanji = k;
            InvalidateVisual();
        }
        private void Dt_Tick(object? sender, EventArgs e)
        {
            if (dir)
            {
                opacity -= 0.004;
            }
            else
            {
                opacity += 0.004;
            }
            if (opacity < 0.95)
            {
                dir = false;
            }
            if (opacity >= 1)
            {
                dir = true;
            }
            InvalidateVisual();
        }

        public string kanji = "";

        protected override void OnRender(DrawingContext drawingContext)
        {
            
            double fontsize = 176;
            //double fontsize = RenderSize.Width / 2.3;
            FormattedText text = new FormattedText(kanji,
               Thread.CurrentThread.CurrentUICulture,
               FlowDirection.LeftToRight,
               //new Typeface("UD Digi Kyokasho N-B"),
               new Typeface("Noto Sans JP Regular"),
               fontsize,
               Brushes.Black, 2.5);

            double x = 0;
            double y = 0;
            //double y = text.Height / 4.8 - 110;
            //y = -y;
            //y += 200;
            //y = 200;
            Geometry textGeometry = text.BuildGeometry(new Point(x, y));
            Rect rx = new(0,0,430,330);
            RectangleGeometry r = new(rx);
 
            var woop = Geometry.Combine(textGeometry, r, GeometryCombineMode.Xor, null);






            //fontsize = RenderSize.Width / 2.3;
            //text = new FormattedText("極",
            //   Thread.CurrentThread.CurrentUICulture,
            //   FlowDirection.LeftToRight,
            //   //new Typeface("UD Digi Kyokasho N-B"),
            //   new Typeface("Noto Sans JP"),
            //   fontsize,
            //   Brushes.Black, 2.5);

            //x = 300;
            //y = text.Height / 4.8 - 110;
            //y = -y;
            //textGeometry = text.BuildGeometry(new Point(x, y));
            //rx = new(0, 0, 2000, 2000);
            //r = new(rx);

            ///woop = Geometry.Combine(woop, r, GeometryCombineMode.Xor, null);










            SolidColorBrush c = new();
            c = (SolidColorBrush)new BrushConverter().ConvertFrom("#222222");
            c.Opacity = opacity;
            Pen pp = new();
            pp.Brush = Brushes.Black;
            pp.Thickness = 1;
            drawingContext.DrawGeometry(c, null, woop);
            base.OnRender(drawingContext);
        }
    }    
}
