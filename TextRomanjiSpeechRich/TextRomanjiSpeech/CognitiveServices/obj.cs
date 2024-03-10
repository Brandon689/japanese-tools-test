using System.Collections.Generic;

namespace MPV_LAUNCHER
{
    public class Recording
    {
        public Recording()
        {
            //if (!string.IsNullOrEmpty(MorphemesStr))
            //{
            //    Morphemes = JsonSerializer.Deserialize<List<string>>(MorphemesStr);
            //    ;
            //}
            //if (!string.IsNullOrEmpty(MillisecondTimeStr))
            //{
            //    MillisecondTime = JsonSerializer.Deserialize<List<ulong>>(MillisecondTimeStr);
            //    ;

            //}
        }

        public string? Text { get; set; }
        public int FileId { get; set; }
        //public IEnumerable<ulong> MillisecondStarts { get; set; }
        //public IEnumerable<string> Morphemes { get; set; }

        //public ICollection<ulong>? MillisecondTime { get; set; }
        //public ICollection<string>? Morphemes { get; set; }

        public string? MillisecondTimeStr { get; set; }
        public string? MorphemesStr { get; set; }

        public List<ulong>? MillisecondTime { get; set; }
        public List<ulong>? MillisecondTimez { get; set; }
        public List<uint>? Offsets { get; set; }
        public List<string>? Morphemes { get; set; }
        public string? Speaker { get; set; }
        public string? VoiceStyle { get; set; }
        public string? Pitch { get; set; }
        public string? Volume { get; set; }
        public string? Rate { get; set; }
    }
}