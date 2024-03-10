using MeCab;
using System.Collections.Generic;
using System.Linq;

namespace whatever
{
    public class MeCabService
    {
        private readonly MeCabParam _mecabParam;
        private readonly MeCabTagger _mecabTagger;

        public MeCabService()
        {
            _mecabParam = new MeCabParam();
            _mecabTagger = MeCabTagger.Create(_mecabParam);
        }

        #region public methods

        public List<MeCabRow> NodesToModelList(string text)
        {
            List<MeCabNode> Nodes = ParseToNodes(text);
            List<MeCabRow> models = new();
            for (int i = 0; i < Nodes.Count; i++)
            {
                MeCabRow converted = NodeToModel(Nodes[i]);
                if (converted == null)
                    continue;
                if (models.Exists(x => x.Morpheme == converted.Morpheme))
                    continue;
                models.Add(converted);
            }
            return models;
        }

        #endregion

        #region private method

        private List<MeCabNode> ParseToNodes(string text)
        {
            // what does it return if u put in garbage text? count 0? null?
            var nodes = _mecabTagger.ParseToNodes(text);
            return nodes.Where(x => x.CharType > 0).ToList();
        }

        private MeCabRow NodeToModel(MeCabNode node)
        {
            var x = node.Feature.Split(',');
            if (x.Length != 9)
                return null;
            return MeCabHelper.MakeMeCabRowModel(node.Surface, x[0], x[1], x[2], x[3], x[4], x[5], x[6], x[7], x[8]);
        }

        #endregion
    }

    public class MeCabRow// : DomainObject
    {
        public string? Hegen { get; set; }

        public string? Morpheme { get; set; }
        // 表層形   品詞
        public string? SurfaceFormPartOfSpeech { get; set; }
        // 品詞細分類1
        public string? PartOfSpeechSubclassification1 { get; set; }
        // 品詞細分類2
        public string? PartOfSpeechSubclassification2 { get; set; }
        // 品詞細分類3
        public string? PartOfSpeechSubclassification3 { get; set; } //Part of speech subdivision
        // 活用型
        public string? ConjugationType { get; set; }
        // 活用形
        public string? ConjugatedForm { get; set; }//conjugationform
        // 原形
        public string? OriginalForm { get; set; } // prototype, original form, base form, root infinitive
        // 読み
        public string? Reading { get; set; }
        // 発音
        public string? Pronunciation { get; set; }

        public int Freq { get; set; }

        // yes put this in better object at some point i know
        public int SubtitleNumber { get; set; }

        public int Index { get; set; }

        public string? Text { get; set; }

        public bool IsGap { get; set; }
        public MeCabRow()
        {

        }

        public MeCabRow(int index, string text, bool isgap)
        {
            Index = index;
            Text = text;
            IsGap = isgap;
        }
    }

    public class MeCabHelper
    {
        private readonly MeCabService _meCabService;

        public MeCabHelper(MeCabService meCabService)
        {
            _meCabService = meCabService;
        }

        //public List<MorphemeCount> RankMorphemeFrequency(string text)
        //{
        //    List<MorphemeCount> res = new();
        //    var node = _meCabService.NodesToModelList(text);
        //    for (int i = 0; i < node.Count; i++)
        //    {
        //        var init = res.Find(x => x.Morpheme == node[i].Morpheme);
        //        if (init != null)
        //        {
        //            ++init.Freq;
        //            continue;
        //        }
        //        res.Add(new MorphemeCount
        //        {
        //            Freq = 1,
        //            Morpheme = node[i].Morpheme,
        //            Row = node[i]
        //        });
        //    }
        //    res.Sort((x, y) => y.Freq.CompareTo(x.Freq));
        //    return res;
        //}

        public List<MeCabRow> RankMorphemeFrequency(string text)
        {
            List<MeCabRow> res = new();
            var node = _meCabService.NodesToModelList(text);
            for (int i = 0; i < node.Count; i++)
            {
                var init = res.Find(x => x.Morpheme == node[i].Morpheme);
                if (init != null)
                {
                    ++init.Freq;
                    continue;
                }
                //res.Add(new MeCabRow
                //{
                //    Freq = 1,
                //    Morpheme = node[i].Morpheme,
                //    Row = node[i]
                //});
                res.Add(node[i]);
                res[res.Count - 1].Freq = 1;
            }
            res.Sort((x, y) => y.Freq.CompareTo(x.Freq));
            return res;
        }

        public List<MeCabRow> RankMorphemeFrequency(IEnumerable<string> text)
        {
            List<MeCabRow> all = new();
            List<MeCabRow> res = new();
            foreach (string item in text)
            {
                var node = _meCabService.NodesToModelList(item);
                all.AddRange(node);
            }
            for (int i = 0; i < all.Count; i++)
            {
                var init = res.Find(x => x.Morpheme == all[i].Morpheme);
                if (init != null)
                {
                    ++init.Freq;
                    continue;
                }
                res.Add(all[i]);
                res[res.Count - 1].Freq = 1;
            }
            res.Sort((x, y) => y.Freq.CompareTo(x.Freq));
            return res;
        }

        //public List<MorphemeCount> RankMorphemeFrequency(IEnumerable<string> text)
        //{
        //    List<MeCabRow> all = new();
        //    List<MorphemeCount> res = new();
        //    foreach (string item in text)
        //    {
        //        var node = _meCabService.NodesToModelList(item);
        //        all.AddRange(node);
        //    }
        //    for (int i = 0; i < all.Count; i++)
        //    {
        //        var init = res.Find(x => x.Morpheme == all[i].Morpheme);
        //        if (init != null)
        //        {
        //            ++init.Freq;
        //            continue;
        //        }
        //        res.Add(new MorphemeCount
        //        {
        //            Freq = 1,
        //            Morpheme = all[i].Morpheme
        //        });
        //    }
        //    res.Sort((x, y) => y.Freq.CompareTo(x.Freq));
        //    return res;
        //}

        public static MeCabRow MakeMeCabRowModel(string morpheme, string surfaceFormPartOfSpeech, string partOfSpeechSubclassification1, string partOfSpeechSubclassification2,
            string partOfSpeechSubclassification3, string conjugationType, string conjugatedForm, string originalForm, string reading, string pronunciation)
        {
            MeCabRow r = new();
            r.Morpheme = morpheme;
            r.SurfaceFormPartOfSpeech = surfaceFormPartOfSpeech;
            r.PartOfSpeechSubclassification1 = partOfSpeechSubclassification1;
            r.PartOfSpeechSubclassification2 = partOfSpeechSubclassification2;
            r.PartOfSpeechSubclassification3 = partOfSpeechSubclassification3;
            r.ConjugationType = conjugationType;
            r.ConjugatedForm = conjugatedForm;
            r.OriginalForm = originalForm;
            r.Reading = reading;
            r.Pronunciation = pronunciation;
            return r;
        }
    }
}