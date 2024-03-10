using SubtitlesParser.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clipSearchEngine
{
    public class AnimeDict
    {
        public VideoFileForMALId Vid { get; set; }
        public JpSubtitleFileForMALId JSub { get; set; }
        public EngSubtitleFileForMALId ESub { get; set; }

        public List<SubtitleItem?> SubListJ { get; set; }
    }
}
