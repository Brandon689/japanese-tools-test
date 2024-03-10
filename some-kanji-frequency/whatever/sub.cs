using SubtitlesParser.Classes;
using SubtitlesParser.Classes.Parsers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace whatever
{
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
}
