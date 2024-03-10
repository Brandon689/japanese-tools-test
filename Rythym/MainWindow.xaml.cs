using System.Windows;
using System.Windows.Input;

namespace Rythym
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                game.c();
            }
            base.OnKeyDown(e);
        }
    }
}
