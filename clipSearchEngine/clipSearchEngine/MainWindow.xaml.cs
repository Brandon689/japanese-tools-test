using SubtitlesParser.Classes.Parsers;
using SubtitlesParser.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO;
using Path = System.IO.Path;
using FlyleafLib.MediaPlayer;
using FlyleafLib;
using static FlyleafLib.Config;
using System.Threading;
using System.Text.RegularExpressions;
using cmdlib;
using FlyleafLib.MediaFramework.MediaRenderer;
using System.Windows.Threading;
using Vortice.MediaFoundation;
using System.Diagnostics;
using System.Text.Json;
using System.Timers;
using Vortice.XAudio2;

namespace clipSearchEngine
{
    public partial class MainWindow : Window
    {
        private ViewModel vm;
        public Window1 w1;
        DispatcherTimer dt = new();
        FileSystemWatcher fw;

        public MainWindow()
        {
            InitializeComponent();
            vm = new();
            this.DataContext = vm;
            w1 = new();
            vm.Wot(w1);
            w1.Show();
            vm.act += Vm_Acto;
            vm.Sentence += Vm_Sentence;

            dt.Interval = new TimeSpan(0, 0, 0, 0, 50);
            dt.Tick += Dt_Tick;
            InitializeComponent();

            fw = new FileSystemWatcher();
            fw.Path = @"C:\なじみ\に\clipSearchEngine\writer\";

            fw.NotifyFilter = NotifyFilters.LastWrite; //NotifyFilters.FileName | NotifyFilters.Size;//this is bad hack to make the bug of fire twice not be issue
            // make my own thing for it where the reciever locks out for 30 millisecond at least that is what i want anyway
            fw.Filter = "*.txt*";
            fw.EnableRaisingEvents = true;
            fw.Changed += Fw_Changed;
        }
        int dispose = 0;

        private void Fw_Changed(object sender, FileSystemEventArgs e)
        {
            ++dispose;
            if (dispose % 2 == 0) return;
            //Thread.Sleep(1);

            dt.Start();
            soo = DateTime.Now;
            sto.Start();

            //var contents = JsonConvert.DeserializeObject<Sakana>(System.IO.File.ReadAllText(@"C:\s.json"));
            //string[] files = new string[1];
            //files[0] = contents.File;
            //Core.LoadFiles(files, false, false);

            //time = contents.Time / 1000;
            //Console.WriteLine(time);
        }

        private void Vm_Sentence()
        {

        }

        private void Vm_Acto()
        {
            playthis();         
        }
        int next = 0;
        long duration = 0;
        long ending = 0;
        Stopwatch sto = new();
        private void Dt_Tick(object? sender, EventArgs e)
        {
            ////long mill = w1.Player.CurTime / 10000;
            //////var eo = mill > ending;

            ////if (mill > ending)
            ////{
            ////    dt.Stop();
            ////    playthis();
            ////}
            Console.WriteLine(sto.ElapsedMilliseconds);
            Console.WriteLine(duration);
            if (sto.ElapsedMilliseconds >= duration)
            {
                janai.Text = sto.ElapsedMilliseconds.ToString();

                dt.Stop();
                Console.WriteLine(sto.ElapsedMilliseconds);
                var dob = sto.ElapsedMilliseconds;
                sto.Reset();

                Console.WriteLine(soo);
                var fl = DateTime.Now;

                ; ;
                playthis();
            }
        }
        private void playthis()
        {
            //if (next == vm.Ohno.Count) return;
            oko.SelectedItem = vm.o[next];

            var c = oko.SelectedItem as Anime;
            if (c != null)
            {
                ending = c.sub.EndTime;
                duration = (c.sub.EndTime - c.sub.StartTime) + 100;
                //if (duration < 1200)
                //{
                //    Console.WriteLine("!");
                //    Console.WriteLine("!");
                //    Console.WriteLine("!");
                //    Console.WriteLine("!");
                //    Console.WriteLine("!");
                //    Console.WriteLine("!");
                //}
                Console.WriteLine("durat: " + duration);
                //int intg = chano.sub.StartTime - 30;
                //if (intg % 2 != 0) intg -= 1;
                //Console.WriteLine(chano.filename);
                oko.ScrollIntoView(oko.SelectedItem);
                Opi(c.filename, c.sub.StartTime);
                w1.Opi(c.filename, c.sub.StartTime);
                //Console.WriteLine(intg);
                CMDProcess cmd = new(new CommandTemplate());
                RunProcess p = new();
                var workingDir = Path.GetDirectoryName(c.filename);
                var imageOutFolder = $"C:\\Users\\BLaze\\Music\\sad\\{Path.GetFileNameWithoutExtension(c.filename)}.wav";
                //await cmd.SoundClip(p, workingDir, chano.filename, imageOutFolder, TimeLord.fey(intg), TimeLord.fey(intg + 9000));
            }
    

            ++next;
        }
        public DateTime soo = new();
        public void Opi(string file, int t)
        {
            Console.WriteLine(file);
            Sakana usagi = new();
            usagi.File = file;
            usagi.Time = t;
            File.WriteAllText(@"C:\なじみ\に\SENDMESSAGE\ucan\sakana.json", JsonSerializer.Serialize(usagi));
            //////Player.OpenCompleted += (object? sender, OpenCompletedArgs e) =>
            //////{
            //////    Console.WriteLine("?");
            //////    Player.SeekAccurate(t);
            //////    //Player.Play();
            //////};
            ////Console.WriteLine("!");
            ////if (file != fileprev)
            ////{
            ////    Player.Open(file);
            ////    Console.WriteLine("op");
            ////}
            //////Player.Pause();
            ////Player.SeekAccurate(t);

            //////Player.SeekAccurate(t.TotalMilliseconds);
            ////fileprev = file;
        }

        private async void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("#");

            //Sentence?.Invoke();
            //TimeSpan t = new(0, 0, 0, 0, mgh.sub.StartTime);
            var chano = oko.SelectedItem as Anime;
            if (chano != null)
            {
                int intg = chano.sub.StartTime;
                Console.WriteLine(chano.filename);

                Opi(chano.filename, intg);
                //w1.Opi(chano.filename, intg);
                CMDProcess cmd = new(new CommandTemplate());
                RunProcess p = new();
                var workingDir = Path.GetDirectoryName(chano.filename);
                var imageOutFolder = $"C:\\Users\\BLaze\\Music\\sad\\{Path.GetFileNameWithoutExtension(chano.filename)}.wav";
                //await cmd.SoundClip(p, workingDir, chano.filename, imageOutFolder, TimeLord.fey(intg), TimeLord.fey(intg + 9000));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            next = 0;
            dt.Stop();
            Console.WriteLine(sto.ElapsedMilliseconds);
            var dob = sto.ElapsedMilliseconds;
            sto.Reset();

            Console.WriteLine(soo);
            var fl = DateTime.Now;

            ; ;
            playthis();
        }
    }

    public static class TimeLord
    {
        public static string fey(int ms)
        {
            TimeSpan interval = new TimeSpan(0, 0, 0, 0, ms);
            string hour = "0" + interval.Hours.ToString();
            string minute = interval.Minutes.ToString();
            if (minute.Length == 1)
            {
                minute = "0" + minute;
            }
            string second = interval.Seconds.ToString();
            if (second.Length == 1)
            {
                second = "0" + second;
            }
            string millisecond = interval.Milliseconds.ToString();
            if (millisecond.Length == 1)
            {
                millisecond = "00" + millisecond;
            }
            else if (millisecond.Length == 2)
            {
                millisecond = "0" + millisecond;
            }
            return $"{hour}:{minute}:{second}.{millisecond}";
        }
    }

    public class WonderEgg
    {
        DataContext d = new();
        Sub so = new();
        private List<AnimeDict> Dict = new();
        //private Dictionary<long?, AnimeDict> Dict = new();
        private List<SubList> a = new();
        
        public List<Anime> t(string find)
        {
            List<Anime> ad = new();
            for (int i = 0; i < Dict.Count; i++)
            {
                for (int j = 0; j < Dict[i].SubListJ.Count; j++)
                {
                    for (int k = 0; k < Dict[i].SubListJ[j].PlaintextLines.Count; k++)
                    {
                        if (Dict[i].SubListJ[j].PlaintextLines[k].Contains(find)) //&& Dict[i].SubListJ[j].PlaintextLines[k].Length > 10)
                        {
                            Anime mg = new();
                            mg.find = find;
                            mg.episode = Dict[i].Vid.Episode;
                            mg.filename = Dict[i].Vid.FileName;
                            mg.lines = String.Join("", Dict[i].SubListJ[j].PlaintextLines);
                            mg.linesSpl = Regex.Split(mg.lines, "(" + find + ")").ToList();
                            //mg.linesSpl = mg.lines.SplitAndKeepDelimiters(find.ToCharArray()).ToList();
                            //mg.linesSpl = mg.lines.Split(find).ToList();
                            mg.sub = Dict[i].SubListJ[j];
                            mg.malid = Dict[i].Vid.MalId;
                            ad.Add(mg);
                        }
                    }
                }
            }
            return ad;
        }
        public List<J> R3s(string find)
        {
            List<J> ret = new();
            for (int i = 0; i < a.Count; i++)
            {
                for (int j = 0; j < a[i].subs.Count; j++)
                {
                    bool op = false;
                    for (int k = 0; k < a[i].subs[j].PlaintextLines.Count; k++)
                    {
                        if (a[i].subs[j].PlaintextLines[k].Contains(find))
                        {
                            if (!op)
                            {
                                J x = new();
                                x.mid = a[i].malid;
                                x.f = a[i].subs[j];
                                ret.Add(x);
                            }
                            op = true;
                            break;
                        }
                    }
                }
            }
            return ret;
        }

        public List<string> Res(string find)
        {
            List<string> ret = new();
            for (int i = 0; i < a.Count; i++)
            {
                for (int j = 0; j < a[i].subs.Count; j++)
                {
                    for (int k = 0; k < a[i].subs[j].PlaintextLines.Count; k++)
                    {
                        if (a[i].subs[j].PlaintextLines[k].Contains(find))
                        {
                            ret.Add(a[i].subs[j].PlaintextLines[k]);
                        }
                    }
                }
            }
            return ret;
        }
        
        public void Protocol()
        {
            var s = d.GetS().ToList();
            var se = d.GetSE().ToList();
            var v = d.GetV().ToList();

            foreach (var item in v)
            {
                if (item.JPSubsTimed == false) continue;
                var s1 = s.Find(x => x.MalId == item.MalId && x.Episode == item.Episode);
                var s2 = se.Find(x => x.MalId == item.MalId && x.Episode == item.Episode);
                AnimeDict di = new();
                di.ESub = s2;
                di.JSub = s1;
                di.Vid = item;
                if (di.JSub!=null)
                {
                    var f = @"C:\GITHUB\subtitle-video-sync-play\";

                    f += item.MalId + "\\all\\";
                    f += "FIXED-JP-SUB\\";
                    var z = Path.GetFileNameWithoutExtension(item.FileName); //.Substring(0,item.FileName.Length - 4);
                    f += z;

                    f += ".jp.srt";

                    di.SubListJ = so.ParseFileAny(f);
                    Dict.Add(di);
                    //Dict.Add(item.MalId + item.Episode, di);
                }
            }
        }


    }
    public class J
    {
        public long? mid { get; set; }
        public SubtitleItem? f { get; set; }
        public List<SubtitleItem?> v { get; set; }
    }
    public class SubList
    {
        public long? malid { get; set; }
        public List<SubtitleItem?> subs { get; set; }
    }
    public class Anime
    {
        public long? malid { get; set; }
        public SubtitleItem? sub { get; set; }
        public string lines { get; set; }
        public List<string> linesSpl { get; set; } = new();
        public int episode { get; set; }
        public string filename { get; set; }
        public string find { get; set; }
    }
    public class Sub
    {
        private readonly SubParser Parser = new();

        public List<SubtitleItem?> ParseFileAny(string file)
        {
            using var fileStream = File.OpenRead(file);
            var fileName = Path.GetFileName(file);
            var mostLikelyFormat = Parser.GetMostLikelyFormat(fileName);
            var items = Parser.ParseStream(fileStream, Encoding.UTF8, mostLikelyFormat);
            return items;
        }
    }
    
    public class ViewModel : VmBase
    {
        public event Action act;
        public event Action Sentence;
        public WonderEgg we = new();
        private Window1 wi;
        //public List<string> Ohno { get; set; } = new();
        private List<Anime> ohno = new();
        public List<Anime> o
        {
            get => ohno;
            set => SetProperty(ref ohno, value);
        }
        public void Wot(Window1 w)
        {
            wi = w;
        }
        private string tox = String.Empty;
        public string n
        {
            get => tox;
            set => SetProperty(ref tox, value);
        }
        private Anime m;
        public Anime AnimeItem
        {
            get => m;
            set
            {
                SetProperty(ref m, value);
            }
        }
        public ViewModel()
        {
            we.Protocol();
        }

        public ICommand Enter
        {
            get
            {
                return new DelegateCommand(param =>
                {
                    var x = we.t(n);
                    o = x;
                    act?.Invoke();
                });
            }
        }     
    }
}
    