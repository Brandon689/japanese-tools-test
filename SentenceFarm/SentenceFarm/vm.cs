using SentenceFarm.genshinwiki;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SentenceFarm
{
    public class Vm : VmBase
    {
        NAudioWrapper wr = new();

        GenshinWiki gw = new();
        sql s = new sql();
        koi kd;
        private ObservableCollection<motoko> ni;
        public ObservableCollection<motoko> Ni
        {
            get => ni;
            set
            {
                SetProperty(ref ni, value);
            }
        }
        private ObservableCollection<koi> otome;
        public ObservableCollection<koi> Otome
        {
            get => otome;
            set
            {
                SetProperty(ref otome, value);
            }
        }
        public void ke(motoko s, int a)
        {
            wr.AudioFile(kd.GenshinFile + "\\ogg\\" + a.ToString() + ".ogg");

        }
        public void too(koi k)
        {
            kd = k;
            var motokoList = JsonSerializer.Deserialize<List<motoko>>(File.ReadAllText(k.GenshinFile + "\\wiki.json"));
            //var ogg = Directory.GetFiles(k.GenshinFile + "\\ogg");
            Ni = new ObservableCollection<motoko>(motokoList);
        }
        public Vm()
        {

            var d = s.Load().ToList();
            otome = new ObservableCollection<koi>(d.ToList());
        }
    }

    public class Ichi
    {
        public string title { get; set; }
        public string engtitle { get; set; }
        public string romaji { get; set; }
        public string jpt { get; set; }
    }
}
