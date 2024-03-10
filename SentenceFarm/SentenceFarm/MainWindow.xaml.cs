using SentenceFarm.genshinwiki;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SentenceFarm
{
    public partial class MainWindow : Window
    {
        //GenshinWiki gw = new();
        Vm vm = new Vm();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = vm;
            //wanderer.Load(gw.motokoList[0].jpt);

            //wr.AudioFile(gw.ogg[0]);
            //wr.Play();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ob = listView.SelectedItem as motoko;
            if (ob != null)
            {
                Console.WriteLine(ob.engt);
                vm.ke(ob, listView.SelectedIndex);
            }
   
            //wr.Play();
        }

        private void listSea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            koi k = (koi)listSea.SelectedItem;

            if (k != null)
            {
                vm.too(k);

            }
        }
    }
}
