using Catalyst;
using Mosaik.Core;
using System.Text.Json;
using whatever;

KanjiDict dc = new();
await dc.Out();

Sub s = new();
MALDataContext mal = new();

MeCabService nyaa = new();

var hm = nyaa.NodesToModelList("会長");

var kai = mal.GetS(40591).ToList();
kai.AddRange(mal.GetS(37999));
kai.AddRange(mal.GetS(43608));
List<string> re = new();
Class3.A(s, re, kai);


var namesInBrackets = JsonSerializer.Deserialize<List<FreqData>>(File.ReadAllText(@"C:\Logs\mem.json"));

for (int i = 0; i < re.Count; i++)
{
    if (re[i].Length < 2) continue;
    // removing the bracketed text at beginning this is almost always silent context
    if (re[i][0] == '（')
    {
        int x = re[i].IndexOf('）');
        var so = re[i];
        if (x == -1)
        {
            ;
        }
        else
        {
            re[i] = re[i].Substring(1, x - 1);
        }
    }

    re[i] = PunctuationMarks.Cleanse(re[i]);
    re[i] = re[i].Replace("!", "");
    re[i] = re[i].Replace("?", "");
    re[i] = re[i].Replace("...", "");
    re[i] = re[i].Replace(",", "");
    re[i] = re[i].Replace(".", "");


}

List<MeCabRow> ana = new();
List<string> shini = new();
List<string> shini1 = new();
List<string> shini2 = new();
List<string> shini4 = new();
List<string> shini5 = new();
List<string> shini6 = new();
for (int i = 0; i < re.Count; i++)
{
    var h = nyaa.NodesToModelList(re[i]);
    ana.AddRange(h);
    var xo = h.Select(x => x.Morpheme).ToList();
    shini.AddRange(Class3.S(xo, 3));


    var keo = Class3.S(xo, 1);
    if (keo.Count > 0)
    {
        keo = keo.Where(x => x.Length > 1 || LanguageResources.HiraganaDict.ContainsKey(x[0]) == false).ToList();
        keo = keo.Where(x => x.Length > 1 || LanguageResources.KatakanaDict.ContainsKey(x[0]) == false).ToList();
    }
    //keo = keo.Where(x => x == "").ToList();

    //LanguageResources
    shini1.AddRange(keo);


    shini2.AddRange(Class3.S(xo, 2));
    shini4.AddRange(Class3.S(xo, 4));
    shini5.AddRange(Class3.S(xo, 5));
    //shini6.AddRange(Class3.Shinde(xo, 6));
}
Console.WriteLine(shini.Count);
Console.WriteLine(shini1.Count);
Console.WriteLine(shini2.Count);
Console.WriteLine(shini4.Count);
Console.WriteLine(shini5.Count);
Console.WriteLine(shini6.Count);
//Console.WriteLine(ana.Count);

List<FreqData> k = new();
shini.AddRange(shini4);
shini.AddRange(shini1);
shini.AddRange(shini2);
shini.AddRange(shini5);
shini.AddRange(shini6);
shini = shini.Distinct().ToList();
Console.WriteLine(shini.Count);

List<string> shin = new();

for (int i = 0; i < shini.Count; i++)
{
    bool ok = true;
    for (int n = 0; n < namesInBrackets.Count; n++)
    {
        if (shini[i].Contains(namesInBrackets[n].text))
        {
            ok = false; break;
        }
    }
    if (ok)
    {
        shin.Add(shini[i]);
    }
}
shini = shin;
//shini = shini.GetRange(0, 5090).ToList();
for (int i = 0; i < re.Count; i++)
{

    for (int l = 0; l < shini.Count; l++)
    {
        string wah = shini[l];

        if (re[i].Contains(wah))
        {

            int j = finder(k, wah);

            if (j == -1)
            {
                k.Add(new FreqData(wah));
            }
            else
            {
                k[j].freq++;
            }
        }

    }
}
k.Sort((x, y) => y.freq.CompareTo(x.freq));


string tt = "";
for (int i = 0; i < 100; i++)
{
    tt += k[i].text + "\n";
}

int finder(List<FreqData> f, string prob)
{
    for (int i = 0; i < f.Count; i++)
    {
        if (f[i].text == prob) return i;
    }
    return -1;
}




Environment.Exit(1);


// ENGLISH SIDE


var a = mal.GetSE(40591).ToList();
a.AddRange(mal.GetSE(37999));
a.AddRange(mal.GetSE(43608));


List<string> e = new();

List<snow> n = new();

Class3.A2(n, s, e, a);

File.WriteAllText(@"C:\Logs\anemo.txt", String.Join('\n', e));

Console.WriteLine(e.Count);

Catalyst.Models.English.Register(); //You need to pre-register each language (and install the respective NuGet Packages)
Storage.Current = new DiskStorage("catalyst-models");
var nlp = await Pipeline.ForAsync(Language.English);

var nt = e.Select(x => new Document(x, Language.English));

var docs = nt;//GetDocuments();
var parsed = nlp.Process(docs);
DoSomething(parsed);


void DoSomething(IEnumerable<IDocument> docs)
{

    List<string> words = new();
    List<string> lines = new();

    foreach (string item in e)
    {
        lines.Add(item);
        string it = item;
        it = it.Replace("!", "");
        it = it.Replace("?", "");
        it = it.Replace("...", "");
        it = it.Replace(",", "");
        it = it.Replace(".", "");
        words.AddRange(it.Split(' '));
    }

    Dictionary<string, A> h = new();
    for (int i = 0; i < words.Count; i++)
    {
        if (h.ContainsKey(words[i]))
        {
            h[words[i]].Match++;
        }
        else
        {
            h.Add(words[i], new A());
        }
    }
    foreach (var item in docs)
    {
        foreach (var item2 in item.ToTokenList())
        {
            if (h.ContainsKey(item2.Value))
            {
                h[item2.Value].POS = item2.POS.ToString();
                h[item2.Value].Toke.Add((Token)item2);
            }
        }
    }

    var ikki = h.OrderByDescending(x => x.Value.Match);

    var ordered = h.OrderByDescending(x => x.Value.Match).ToDictionary(x => x.Key, x => x.Value);
    ;
    //var ot = ordered.ToList();
    ;
    int noun = ordered.Count(x => x.Value.POS == "NOUN");
    int verb = ordered.Count(x => x.Value.POS == "VERB");
    int propernoun = ordered.Count(x => x.Value.POS == PartOfSpeech.PROPN.ToString());
    int pronoun = ordered.Count(x => x.Value.POS == PartOfSpeech.PRON.ToString());
    int adjective = ordered.Count(x => x.Value.POS == PartOfSpeech.ADJ.ToString());
    int adverb = ordered.Count(x => x.Value.POS == PartOfSpeech.ADV.ToString());
    int particle = ordered.Count(x => x.Value.POS == PartOfSpeech.PART.ToString());
    int auxil = ordered.Count(x => x.Value.POS == PartOfSpeech.AUX.ToString());
    int det = ordered.Count(x => x.Value.POS == PartOfSpeech.DET.ToString());
    int intj = ordered.Count(x => x.Value.POS == PartOfSpeech.INTJ.ToString());
    //int verb = ordered.Count(x => x.Value.POS == "VERB");
    //int verb = ordered.Count(x => x.Value.POS == "VERB");
    //int verb = ordered.Count(x => x.Value.POS == "VERB");
    //int verb = ordered.Count(x => x.Value.POS == "VERB");
    //int verb = ordered.Count(x => x.Value.POS == "VERB");
    //;


    //Console.WriteLine("noun :" + noun);
    //Console.WriteLine("verb :" + verb);
    //Console.WriteLine("propernoun :" + propernoun);
    //Console.WriteLine("pronoun :" + pronoun);
    //Console.WriteLine("adjective :" + adjective);
    //Console.WriteLine("adverb :" + adverb);
    //Console.WriteLine("particle :" + particle);
    //Console.WriteLine("auxil :" + auxil);
    //Console.WriteLine("det :" + det);
    //Console.WriteLine("inj :" + intj);

    var z = ordered.Where(x => x.Value.POS == "NOUN");
    foreach (var item in z)
    {
        //Console.WriteLine(item.Value.Match);
        Console.WriteLine(item.Value.Match + " " + item.Key);
    }
    Console.WriteLine(z.Count());

}

public class A
{
    public int Match { get; set; } = 1;
    public List<Token> Toke { get; set; } = new();
    public string POS { get; set; }
    //public Alone(int m)
    //{
    //    Match = 1;
    //    Toke = new List<Token>();
    //}

    public override string ToString()
    {
        return Match + " " + POS;
    }
}

