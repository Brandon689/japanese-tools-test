using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wacton.Desu.Japanese;
using Wacton.Desu.Kanji;

namespace whatever
{
    public class KanjiAtomizer
    {
        Dictionary<string, Entry> dic = new();
        KanjiDict dc = new();
        public async Task DicGo()
        {
            await dc.Out();
            for (int i = 0; i < dc.kanji.Count; i++)
            {
                dic.Add(dc.kanji[i].Literal, new Entry(dc.kanji[i]));
            }
        }

        public Dictionary<string, int> fofo(List<string> lines, List<FreqData> funny)
        //public List<freak> fofo(List<string> lines)
        {
            Dictionary<string, int> dic2 = new();
            List<FreqData> frog = new();
            string bigdog = string.Join("", lines);
            for (int i = 0; i < bigdog.Length; i++)
            {
                var s = bigdog[i].ToString();
                if (dic.ContainsKey(s))
                {
                    if (dic2.ContainsKey(s))
                    {
                        dic2[s]++;
                    }
                    else
                    {
                        dic2.Add(s, 1);
                    }
                }
            }
            foreach (var item in dic2)
            {
                funny.Add(new FreqData(item.Key, item.Value));
            }
            funny = funny.OrderByDescending(x => x.freq).ToList();
            //funny = dic2.OrderByDescending(x => x.Value).Select(x => new freak(x.Key, x.Value)).ToList();
            return dic2;
        }
    }
    public class Entry
    {
        public IKanjiEntry K { get; set; }
        public int Occur { get; set; }
        public Entry(IKanjiEntry k)
        {
            K = k;
            Occur = 0;
        }
    }
    public class KanjiDict
    {
        public List<IKanjiEntry> kanji;
        public List<IJapaneseEntry> japi;

        public async Task Out()
        {
            kanji = (await KanjiDictionary.ParseEntriesAsync()).ToList();
            japi = (await JapaneseDictionary.ParseEntriesAsync()).ToList();


        }
    }
}
