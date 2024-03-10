using KaimiraGames;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Omoidasu
{
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        DataContext dc;
        int side = 0;
        int index = 50;
        WeightedList<Card> ha = new();
        Card pickcard;
        Card prevcard;
        List<pack> p = new();
        int studyStart = 20;
        bool frontCard = true;
        bool random=true;
        int studyEnd = 70;
        string kanji = @"..\..\..\shinounme.txt";
        public MainWindow()
        {
            dc = new();

            var lines = File.ReadLines(kanji);

            List<Card> cards = new List<Card>();
            List<WeightedListItem<Card>> myItems = new();
            int j = 0;
            foreach (var line in lines)
            {
                var a = line.Split(";");
                Card c = new();
                c.Index = j++;
                c.Front = a[0];
                c.Back = a[1];
                cards.Add(c);

            }
            cards = cards.GetRange(0, 88);
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].CardIndex = i;
                int w = 20;
                //if (i ==0)
                //{
                //    w = 1;
                //}
                myItems.Add(new WeightedListItem<Card>(cards[i], w));
            }
            //myItems = myItems.GetRange(20, 70);
            //pickcard = cards[index];
            ha = new(myItems);
            pickcard = ha[0];

            //int dj = 1;
            //for (int i = 0; i < ha.Count; i++)
            //{
            //    pack pk = new();
            //    pk.deck = 1;
            //    pk.card = dj++;
            //    p.Add(pk);

            //}
            p = dc.GetFiles().ToList();
            //ha = new List<string>()
            //{
            //    "腹",
            //    "orange",
            //    "立",
            //    "ant",
            //    "炭",
            //    "love"
            //};
            InitializeComponent();
            g();
            //++index;
            MediaPlay.MediaEnded += MediaPlay_MediaEnded;
            MediaPlay.UnloadedBehavior = MediaState.Manual;
            Closing += MainWindow_Closing;
        }

        private void MediaPlay_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaPlay.Position = TimeSpan.Zero;
            MediaPlay.Play();
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            dc.UpdateAll(p);
            //dc.Add(p);
        }

        private void pck(int z)
        {
            //if (z == 0)
            //{
            //    p[index].skipcount++;
            //}
            //if (z == 1)
            //{
            //    p[index].wincount++;
            //}
            //if (z == 2)
            //{
            //    p[index].misscount++;
            //}
            //if (z == -1)
            //{
            //    p[index].wincount--;
            //    p[index].misscount++;

            //}
            //else
            //{
            //    p[index].impressioncount++;
            //}
        }

        private Card nextcard()
        {
            if (random)
            {
                //int month = rnd.Next(studyStart, studyEnd);
                //index = month;
                if (pickcard != null)
                {
                    int bee = ha.GetWeightOf(pickcard);
                    ha.SetWeight(pickcard, 0);
                    var k = ha.Next();
                    index = k.CardIndex;
                    ha.SetWeight(pickcard, bee);
                    return k;
                }
                else
                {
                    var k = ha.Next();
                    index = k.CardIndex;
                    return k;
                }
            }
            return null;
            //else
            //{
            //    index += 1;

            //}

        }
        private void gtonex()
        {
            var c = nextcard();
            frontCard = true;
            pickcard = c;

            //else
            {
                //   index += 1;
            }
            var s = g();
            pck(0);
            if (pickcard != null)
            {
                shoo.aaa(pickcard.Front);
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key==Key.V)
            {
                ha.Remove(pickcard);
                pickcard = null;
            }
            if (e.Key == Key.Right)
            {
                gtonex();
            }
            else if (e.Key == Key.Up)
            {

                if (pickcard == null)
                {
                    gtonex();
                }
                else
                {
                    if (!frontCard)
                    {
                        var c = nextcard();
                        pck(2);
                        frontCard = true;
                        pickcard = c;

                    }
                    else
                    {
                        frontCard = false;
                    }
                    var s = g();
                    //if (s == "yes")
                    //{
                    //    // fail
                    //    pck(2);
                    //}
                    if (pickcard != null)
                    {
                        shoo.aaa(pickcard.Front);
                    }
                }

                

            }
            else if (e.Key == Key.Z)
            {
                if (!frontCard)
                {
                    var c = nextcard();
                    pickcard = c;
                    pck(1);
                    frontCard = true;
                }
                else
                {
                    frontCard = false;
                    ha.SetWeightAtIndex(pickcard.CardIndex, 19);

                }
                var s = g();

                //if (s == "yes")
                //{
                //    // good
                //    pck(1);
                //}
            }
            else if (e.Key == Key.Space)
            {
                if (!frontCard)
                {
                    var c = nextcard();
                    pickcard = c;
                    //pck(1);
                    frontCard = true;
                }
                var s = g();

            }
            else if (e.Key == Key.X)
            {
                if (!frontCard)
                {
                    var c = nextcard();
                    pck(2);
                    frontCard = true;
                    pickcard = c;

                }
                else
                {
                    frontCard = false;
                }
                var s = g();
                //if (s == "yes")
                //{
                //    // fail
                //    pck(2);
                //}
            }
            else if (e.Key == Key.C)
            {
                pck(-1);
            }
            //c.Text = (index + 1).ToString();
        }

        private string g()
        {
            if (index >= ha.Count)
            {
                ;
            }
            if (frontCard)
            {
                front.Text = pickcard.Front;
                //front.Text = ha[index].Front;
                back.Text = "";
                return "";
                
            }
            else
            {
                back.Text = pickcard.Back;
                //back.Text = ha[index].Back;
                return "yes";
            }
        }
    }
}
