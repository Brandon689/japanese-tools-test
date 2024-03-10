using FlyleafLib.MediaPlayer;
using FlyleafLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static FlyleafLib.Config;
using System.Threading;
using System.IO;

namespace clipSearchEngine
{
    public partial class Window1 : Window
    {
        public Player Player { get; set; }
        EngineConfig engineConfig;
        Config playerConfig;
        public Window1()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            ThreadPool.GetMinThreads(out int workers, out int ports);
            ThreadPool.SetMinThreads(workers + 6, ports + 6);

            // Engine's Config
#if RELEASE
            if (File.Exists("Flyleaf.Engine.xml"))
                try { engineConfig = EngineConfig.Load("Flyleaf.Engine.xml"); } catch { engineConfig = DefaultEngineConfig(); }
            else
                engineConfig = DefaultEngineConfig();
#else
            engineConfig = DefaultEngineConfig();
#endif

            Engine.Start(engineConfig);

            // Player's Config (Cannot be initialized before Engine's initialization)
#if RELEASE
            // Load Player's Config
            if (File.Exists("Flyleaf.Config.xml"))
                try { playerConfig = Config.Load("Flyleaf.Config.xml"); } catch { playerConfig = DefaultConfig(); }
            else
                playerConfig = DefaultConfig();
#else
            playerConfig = DefaultConfig();
#endif
            //playerConfig.act
            // Initializes the Player
            Player = new Player(playerConfig);
            Player.Config.Player.ActivityTimeout = 1000;//500000;
            Player.Config.Subtitles.Enabled = false;

            // Allowing VideoView to access our Player
            DataContext = this;
            //Player.OpenCompleted += Player_OpenCompleted;
            InitializeComponent();

            // Allow Flyleaf WPF Control to Load UIConfig and Save both Config & UIConfig (Save button will be available in settings)
            flyleafControl.ConfigPath = "Flyleaf.Config.xml";
            flyleafControl.EnginePath = "Flyleaf.Engine.xml";
            flyleafControl.UIConfigPath = "Flyleaf.UIConfig.xml";

            // If the user requests reverse playback allocate more frames once
            Player.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "ReversePlayback" && !ReversePlaybackChecked)
                {
                    ReversePlaybackChecked = true;
                    if (playerConfig.Decoder.MaxVideoFrames < 60)
                        playerConfig.Decoder.MaxVideoFrames = 60;
                }
            };
        }

        private void Player_OpenCompleted(object? sender, OpenCompletedArgs e)
        {
            
        }

        bool ReversePlaybackChecked;

        private EngineConfig DefaultEngineConfig()
        {
            EngineConfig engineConfig = new EngineConfig();

            engineConfig.PluginsPath    = ":Plugins";
            engineConfig.FFmpegPath     = ":FFmpeg";

            //engineConfig.PluginsPath = "C:\\Users\\BLaze\\Downloads\\Flyleaf_v3.5.2_FFmpeg_v4\\Plugins\\";
            //engineConfig.FFmpegPath = "C:\\Users\\BLaze\\Downloads\\Flyleaf_v3.5.2_FFmpeg_v4\\FFmpeg\\";
            //engineConfig.FFmpegPath = @"C:\Users\BLaze\Downloads\Flyleaf-master\Flyleaf-master\FFmpeg\";
            engineConfig.HighPerformaceTimers
                                        = false;
            engineConfig.UIRefresh = true;

#if RELEASE
            engineConfig.LogOutput      = "Flyleaf.FirstRun.log";
            engineConfig.LogLevel       = LogLevel.Debug;
            engineConfig.FFmpegDevices  = true;
#else
            engineConfig.LogOutput = ":debug";
            engineConfig.LogLevel = LogLevel.Debug;
            engineConfig.FFmpegLogLevel = FFmpegLogLevel.Warning;
#endif

            return engineConfig;
        }

        private Config DefaultConfig()
        {
            Config config = new Config();
            config.Subtitles.SearchLocal = true;
            config.Video.GPUAdapter = ""; // Set it empty so it will include it when we save it

            return config;
        }
        string fileprev;
        //public void Opi(string file, TimeSpan t)
        public void Opi(string file, int t)
        {
            Console.WriteLine(file);
            Sakana usagi = new();
            usagi.File = file;
            usagi.Time = t;
            File.WriteAllText(@"C:\SENDMESSAGE\s.json", JsonSerializer.Serialize(usagi));
            //Player.OpenCompleted += (object? sender, OpenCompletedArgs e) =>
            //{
            //    Console.WriteLine("?");
            //    Player.SeekAccurate(t);
            //    //Player.Play();
            //};
            Console.WriteLine("!");
            if (file != fileprev)
            {
                Player.Open(file);
                Console.WriteLine("op");
            }
            //Player.Pause();
            Player.SeekAccurate(t);

            //Player.SeekAccurate(t.TotalMilliseconds);
            fileprev = file;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //#if RELEASE
            // Save Player's Config (First Run)
            // Ensures that the Control's handle has been created and the renderer has been fully initialized (so we can save also the filters parsed by the library)
            if (!playerConfig.Loaded)
            {
                try
                {
                    //Utils.AddFirewallRule();// why lol
                    playerConfig.Save("Flyleaf.Config.xml");
                }
                catch { }
            }

            // Stops Logging (First Run)
            if (!engineConfig.Loaded)
            {
                engineConfig.LogOutput = null;
                engineConfig.LogLevel = LogLevel.Quiet;
                engineConfig.FFmpegDevices = false;

                try { engineConfig.Save("Flyleaf.Engine.xml"); } catch { }
            }
            //#endif

            // Gives access to keyboard events on start up
            //Player.VideoView.WinFormsHost.Focus();
            //Player.Open(@"C:\Users\BLaze\Downloads\[VCB-Studio] Seitokai Yakuindomo\[VCB-Studio] Seitokai Yakuindomo 2 [Ma10p_1080p]\[VCB-Studio] Seitokai Yakuindomo 2 [01][Ma10p_1080p][x265_2flac].mkv");
        }
    }


    public class Sakana
    {
        public string File { get; set; }
        public double Time { get; set; }
    }
}
