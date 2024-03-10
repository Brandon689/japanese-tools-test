using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ReaderAdvanced
{
    public partial class MainWindow : Window
    {
        IFrameEl El;
        public MainWindow()
        {
            InitializeComponent();
            El = element;
            example();
        }

        private void example()
        {
            El.Freeze = true;
            var r = File.ReadAllText(@"..\..\..\..\sample.txt");
            foreach (var item in r.Split("\r\n"))
            {
                El.Add(item, true);
            }

            element.Finally();
            El.Freeze = false;
            El.Invalidate();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            El.Clear();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.C && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                element.Copy();
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                element.Cut(2, 6);
            }
            base.OnKeyDown(e);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            example();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            element.OrientationToggle();
            element.Invalidate();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            element.save();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            element.Loa();
        }
    }
}
