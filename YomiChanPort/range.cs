using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace YomiChanPort
{
    internal class range
    {
        private const int HIRAGANA_SMALL_TSU_CODE_POINT = 0x3063;
        private const int KATAKANA_SMALL_TSU_CODE_POINT = 0x30c3;
        private const int KATAKANA_SMALL_KA_CODE_POINT = 0x30f5;
        private const int KATAKANA_SMALL_KE_CODE_POINT = 0x30f6;
        private const int KANA_PROLONGED_SOUND_MARK_CODE_POINT = 0x30fc;

        private  int[] HIRAGANA_RANGE = { 0x3040, 0x309f };
        private  int[] KATAKANA_RANGE = { 0x30a0, 0x30ff };

        private readonly int[] HIRAGANA_CONVERSION_RANGE = { 0x3041, 0x3096 };
        private readonly int[] KATAKANA_CONVERSION_RANGE = { 0x30a1, 0x30f6 };

        private readonly int[][] KANA_RANGES = new int[2][];

        private readonly List<int[]> JAPANESE_RANGES = new();

        public range()
        {
            KANA_RANGES[0] = HIRAGANA_RANGE;
            KANA_RANGES[1] = KATAKANA_RANGE;

            JAPANESE_RANGES.Add(HIRAGANA_RANGE);
            JAPANESE_RANGES.Add(KATAKANA_RANGE);
            JAPANESE_RANGES.Add(new int[] { 0xff66, 0xff9f }); // Halfwidth katakana

            JAPANESE_RANGES.Add(new int[] { 0x30fb, 0x30fc }); // Katakana punctuation
            JAPANESE_RANGES.Add(new int[] { 0xff61, 0xff65 }); // Kana punctuation
            JAPANESE_RANGES.Add(new int[] { 0x3000, 0x303f }); // CJK punctuation

            JAPANESE_RANGES.Add(new int[] { 0xff10, 0xff19 }); // Fullwidth numbers
            JAPANESE_RANGES.Add(new int[] { 0xff66, 0xff9f }); // Fullwidth upper case Latin letters
            JAPANESE_RANGES.Add(new int[] { 0xff41, 0xff5a }); // Fullwidth lower case Latin letters

            JAPANESE_RANGES.Add(new int[] { 0xff01, 0xff0f }); // Fullwidth punctuation 1
            JAPANESE_RANGES.Add(new int[] { 0xff1a, 0xff1f }); // Fullwidth punctuation 2
            JAPANESE_RANGES.Add(new int[] { 0xff3b, 0xff3f }); // Fullwidth punctuation 3
            JAPANESE_RANGES.Add(new int[] { 0xff5b, 0xff60 }); // Fullwidth punctuation 4
            JAPANESE_RANGES.Add(new int[] { 0xffe0, 0xffee }); // Currency markers

            string[][] HALFWIDTH_KATAKANA_MAPPING = new string[][]
            {
                new string[] { "ｦ", "ヲヺ-" },
                new string[] { "ｧ", "ァ--" },
                new string[] { "ｨ", "ィ--" },
                new string[] { "ｩ", "ゥ--" },
                new string[] { "ｪ", "ェ--" },
                new string[] { "ｫ", "ォ--" },
                new string[] { "ｬ", "ャ--" },
                new string[] { "ｭ", "ュ--" },
                new string[] { "ｮ", "ョ--" },
                new string[] { "ｯ", "ッ--" },
                new string[] { "ｰ", "ー--" },
                new string[] { "ｱ", "ア--" },
                new string[] { "ｲ", "イ--" },
                new string[] { "ｳ", "ウヴ-" },
                new string[] { "ｴ", "エ--" },
                new string[] { "ｵ", "オ--" },
                new string[] { "ｶ", "カガ-" },
                new string[] { "ｷ", "キギ-" },
                new string[] { "ｸ", "クグ-" },
                new string[] { "ｹ", "ケゲ-" },
                new string[] { "ｺ", "コゴ-" },
                new string[] { "ｻ", "サザ-" },
                new string[] { "ｼ", "シジ-" },
                new string[] { "ｽ", "スズ-" },
                new string[] { "ｾ", "セゼ-" },
                new string[] { "ｿ", "ソゾ-" },
                new string[] { "ﾀ", "タダ-" },
                new string[] { "ﾁ", "チヂ-" },
                new string[] { "ﾂ", "ツヅ-" },
                new string[] { "ﾃ", "テデ-" },
                new string[] { "ﾄ", "トド-" },
                new string[] { "ﾅ", "ナ--" },
                new string[] { "ﾆ", "ニ--" },
                new string[] { "ﾇ", "ヌ--" },
                new string[] { "ﾈ", "ネ--" },
                new string[] { "ﾉ", "ノ--" },
                new string[] { "ﾊ", "ハバパ" },
                new string[] { "ﾋ", "ヒビピ" },
                new string[] { "ﾌ", "フブプ" },
                new string[] { "ﾍ", "ヘベペ" },
                new string[] { "ﾎ", "ホボポ" },
                new string[] { "ﾏ", "マ--" },
                new string[] { "ﾐ", "ミ--" },
                new string[] { "ﾑ", "ム--" },
                new string[] { "ﾒ", "メ--" },
                new string[] { "ﾓ", "モ--" },
                new string[] { "ﾔ", "ヤ--" },
                new string[] { "ﾕ", "ユ--" },
                new string[] { "ﾖ", "ヨ--" },
                new string[] { "ﾗ", "ラ--" },
                new string[] { "ﾘ", "リ--" },
                new string[] { "ﾙ", "ル--" },
                new string[] { "ﾚ", "レ--" },
                new string[] { "ﾛ", "ロ--" },
                new string[] { "ﾜ", "ワ--" },
                new string[] { "ﾝ", "ン--" }
            };

            Dictionary<char, string> k = new()
            {
                { 'a', "ぁあかがさざただなはばぱまゃやらゎわヵァアカガサザタダナハバパマャヤラヮワヵヷ" },
                { 'i', "ぁあかがさざただなはばぱまゃやらゎわヵァアカガサザタダナハバパマャヤラヮワヵヷ" },
                { 'u', "ぁあかがさざただなはばぱまゃやらゎわヵァアカガサザタダナハバパマャヤラヮワヵヷ" },
                { 'e', "ぁあかがさざただなはばぱまゃやらゎわヵァアカガサザタダナハバパマャヤラヮワヵヷ" },
                { 'o', "ぁあかがさざただなはばぱまゃやらゎわヵァアカガサザタダナハバパマャヤラヮワヵヷ" },
                //{ '\0', "ぁあかがさざただなはばぱまゃやらゎわヵァアカガサザタダナハバパマャヤラヮワヵヷ" },
            };


            var VOWEL_TO_KANA_MAPPING = new Dictionary<string, string>
            {
                { "a", "ぁあかがさざただなはばぱまゃやらゎわヵァアカガサザタダナハバパマャヤラヮワヵヷ" },
                { "i", "ぃいきぎしじちぢにひびぴみりゐィイキギシジチヂニヒビピミリヰヸ" },
                { "u", "ぅうくぐすずっつづぬふぶぷむゅゆるゥウクグスズッツヅヌフブプムュユルヴ" },
                { "e", "ぇえけげせぜてでねへべぺめれゑヶェエケゲセゼテデネヘベペメレヱヶヹ" },
                { "o", "ぉおこごそぞとどのほぼぽもょよろをォオコゴソゾトドノホボポモョヨロヲヺ" },
                { "", "のノ" }
            };
            KANA_TO_VOWEL_MAPPING_init();
        }

        const string  SMALL_KANA_SET = "ぁぃぅぇぉゃゅょゎァィゥェォャュョヮ";
        // why was ヵ twice in ithere
        //List<KeyValuePair<char, string>> VOWEL_TO_KANA_MAPPING = new()
        //{
        //    new KeyValuePair<char,string>('a', "ぁあかがさざただなはばぱまゃやらゎわヵァアカガサザタダナハバパマャヤラヮワヵヷ"),
        //    new KeyValuePair<char,string>('i', "ぃいきぎしじちぢにひびぴみりゐィイキギシジチヂニヒビピミリヰヸ"),
        //    new KeyValuePair<char, string>('u', "ぅうくぐすずっつづぬふぶぷむゅゆるゥウクグスズッツヅヌフブプムュユルヴ"),
        //    new KeyValuePair<char, string>('e', "ぇえけげせぜてでねへべぺめれゑヶェエケゲセゼテデネヘベペメレヱヶヹ" ),
        //    new KeyValuePair<char, string>('o', "ぉおこごそぞとどのほぼぽもょよろをォオコゴソゾトドノホボポモョヨロヲヺ"),
        //    new KeyValuePair<char,string>(' ', "のノ" ) // this blank one is holding it back from all being char
        //    //{ '\0', "ぁあかがさざただなはばぱまゃやらゎわヵァアカガサザタダナハバパマャヤラヮワヵヷ" },
        //};
        List<Tuple<char, string>> VOWEL_TO_KANA_MAPPING = new()
        {
            new Tuple<char,string>('a', "ぁあかがさざただなはばぱまゃやらゎわヵァアカガサザタダナハバパマャヤラヮワヵヷ"),
            new Tuple<char,string>('i', "ぃいきぎしじちぢにひびぴみりゐィイキギシジチヂニヒビピミリヰヸ"),
            new Tuple<char, string>('u', "ぅうくぐすずっつづぬふぶぷむゅゆるゥウクグスズッツヅヌフブプムュユルヴ"),
            new Tuple<char, string>('e', "ぇえけげせぜてでねへべぺめれゑヶェエケゲセゼテデネヘベペメレヱヶヹ" ),
            new Tuple<char, string>('o', "ぉおこごそぞとどのほぼぽもょよろをォオコゴソゾトドノホボポモョヨロヲヺ"),
            new Tuple<char,string>(' ', "のノ" )
            //{ '\0', "ぁあかがさざただなはばぱまゃやらゎわヵァアカガサザタダナハバパマャヤラヮワヵヷ" },
        };

        List<Tuple<char, char>> KANA_TO_VOWEL_MAPPING = new();

        private void KANA_TO_VOWEL_MAPPING_init()
        {
            foreach (var item in VOWEL_TO_KANA_MAPPING)
            {
                for (int i = 0; i < item.Item2.Length; i++)
                {
                    KANA_TO_VOWEL_MAPPING.Add(new Tuple<char, char>(item.Item2[i], item.Item1));
                }
            }
        }

        bool isCodePointInRange(int codePoint, int min, int max)
        {
            return (codePoint >= min && codePoint <= max);
        }

        bool isCodePointInRanges(int codePoint, int[][] ranges)
        {
            foreach (var item in ranges)
            {
                if (codePoint >= item.Min() && codePoint <= item.Max())
                {
                    return true;
                }
            }
            return false;
        }
        bool isCodePointInRanges(int codePoint, List<int[]> ranges)
        {
            foreach (var item in ranges)
            {
                if (codePoint >= item.Min() && codePoint <= item.Max())
                {
                    return true;
                }
            }
            return false;
        }

        char? getProlongedHiragana(char previousCharacter)
        {
            switch (KANA_TO_VOWEL_MAPPING[previousCharacter].Item2)
            {
                case 'a': return 'あ';
                case 'i': return 'い';
                case 'u': return 'う';
                case 'e': return 'え';
                case 'o': return 'う';
                default: return null;
            }
        }

        //bool isCodePointKanji(int codePoint)
        //{
        //    return isCodePointInRanges(codePoint, CJK_IDEOGRAPH_RANGES);
        //}

        public bool isCodePointKana(int codePoint)
        {
            return isCodePointInRanges(codePoint, KANA_RANGES);
        }

        public bool isCodePointJapanese(int codePoint)
        {
            return isCodePointInRanges(codePoint, JAPANESE_RANGES);
        }

        public bool isStringEntirelyKana(string str)
        {
            if (str.Length == 0) return false;
            foreach (var c in str)
            {
                if (!isCodePointInRanges(c.CodePoint(), KANA_RANGES))
                {
                    return false;
                }
            }
            return true;
        }

        public bool isStringPartiallyJapanese(string str)
        {
            if (str.Length == 0) return false;
            foreach (char c in str)
            {
                if (isCodePointInRanges(c.CodePoint(), JAPANESE_RANGES))
                {
                    return true;
                }
            }
            return false;
        }


        public string convertHiraganaToKatakana(string text)
        {
            string result = "";
            int offset = (KATAKANA_CONVERSION_RANGE[0] - HIRAGANA_CONVERSION_RANGE[0]);
            foreach (var item in text)
            {
                char v = item;
                int codePoint = item.CodePoint();
                if (isCodePointInRange(codePoint, HIRAGANA_CONVERSION_RANGE.Min(), HIRAGANA_CONVERSION_RANGE.Max()))
                {
                    v = (char)(codePoint + offset); //String.fromCodePoint(codePoint + offset);
                }
                result += v;
            }
            return result;
        }
    }
}
