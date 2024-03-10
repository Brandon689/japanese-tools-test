using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace clipSearchEngine
{
    public class CustomFE : FrameworkElement
    {
        public List<string> MyProperty
        {
            get { return (List<string>)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(List<string>), typeof(CustomFE), new PropertyMetadata(null));


        public string Finder
        {
            get { return (string)GetValue(FinderProperty); }
            set { SetValue(FinderProperty, value); }
        }

        public static readonly DependencyProperty FinderProperty =
            DependencyProperty.Register("Finder", typeof(string), typeof(CustomFE), new PropertyMetadata(string.Empty));



        List<Brush> brushes = new();
        public CustomFE()
        {
            brushes.Add(Brushes.Red);
            brushes.Add(Brushes.Black);
            brushes.Add(Brushes.Blue);
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            Random rnd = new Random();
            int month = rnd.Next(0, 3);
            int zmonth = rnd.Next(0, 30);
            double x = 0;
            for (int i = 0; i < MyProperty.Count; i++)
            {
                //double x = 10 + i * 30 + zmonth;
                //drawingContext.DrawRectangle(rano[month], null, new Rect(x, 3, 16, 16));

                var brush = Brushes.Black;
                if (Finder== MyProperty[i])
                {
                    brush = Brushes.Red;
                }
                FormattedText text = new FormattedText(MyProperty[i],
                Thread.CurrentThread.CurrentUICulture,
                FlowDirection.LeftToRight,
                //new Typeface("UD Digi Kyokasho N-B"),
                new Typeface("Noto Sans JP"),
                16,
                brush, 2.5);

                drawingContext.DrawText(text, new Point(x, 0));
                x += text.Width;
            }

            base.OnRender(drawingContext);
        }
    }
}
