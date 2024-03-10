using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace whatever
{
    public static class PunctuationMarks
    {
        public static string Cleanse(string input)
        {
            for (int i = 0; i < PunctuationList.Count(); i++)
            {
                input = input.Replace(PunctuationList[i].ToString(), "");
            }
            return input;
        }

        public static readonly ReadOnlyCollection<char> PunctuationList = new List<char> {
            '\n',
            '\r',
            ' ',
            '？',
            '。',
            '！',
            '!',
            '?',
            '（',
            '）',
            ')',
            '(',
            '～',
            '…',
            '…',
            '‥',
            '「',
            '」',
            '一',
            '｛',
            '｝',
            '］',
            '【',
            '、',
            '，',
            '゠',
            '＝',
            '『',
            '』',
            '〝',
            '〟',
            '”',
            '“',
            '"',
            '⟨',
            '⟩',
            '〜',
            '：',
            '♪'
          }.AsReadOnly();
    }
}
