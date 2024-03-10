using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SentenceFarm.genshinwiki
{
    internal class GenshinWiki
    {
        string f = @"C:\GITHUB\genshinwiki\genshinwiki\VO\Baizhu\ogg";
        string src = @"C:\GITHUB\genshinwiki\genshinwiki\VO\Baizhu\wiki.json";

        public List<motoko> motokoList;
        public string[] ogg;
        public GenshinWiki()
        {
            motokoList = JsonSerializer.Deserialize<List<motoko>>(File.ReadAllText(src));
            ogg = Directory.GetFiles(f);

        }
    }
}
