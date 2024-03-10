using System;
using System.Timers;
using System.Windows;
using System.Windows.Media;

namespace Rythym
{
    public class render : FrameworkElement
    {
        Timer timer;
        double pos = 0;
        SolidColorBrush b;
        public render()
        {
            b = Brushes.Blue;
            timer = new();
            timer.Interval = (1000 / 120);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            pos += 5;
            if (pos > 800)
            {
                pos = 50;
                b = Brushes.Blue;
            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                InvalidateVisual();
            }));
        }
        public void c()
        {
            if (pos > 550 && pos < 650)
            {
                b = Brushes.Pink;
            }
            InvalidateVisual();

        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            var s = new LinearGradientBrush(Color.FromRgb(209, 227, 250), Color.FromRgb(170, 199, 238), new Point(0.5, 0), new Point(0.5, 1));
            drawingContext.DrawRectangle(s, null, new Rect(0, 0, 800, 750));

            drawingContext.DrawLine(new Pen(Brushes.Black, 2), new Point(0, 600), new Point(800, 600));

            drawingContext.DrawEllipse(b, null, new Point(50, pos), 20, 20);

            drawingContext.DrawEllipse(Brushes.DarkRed, null, new Point(50, 600), 20, 20);


            //int x = 0;
            //int y = 20;
            //for (int i = 0; i < 500; i++)
            //{
            //    if (x > 800)
            //    {
            //        x = 0;
            //        y += 40;
            //    }
            //    x += 40;

            //    drawingContext.DrawEllipse(Brushes.White, null, new Point(x, y), 6, 6);
            //}
        }
    }
}
