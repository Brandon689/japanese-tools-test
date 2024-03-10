using SubtitlesParser.Classes;

namespace whatever
{
    public class snow
    {
        public SubtitleItem x { get; set; }
        public string info { get; set; }

        public snow()
        {

        }

        public snow(SubtitleItem x, string info)
        {
            this.x = x;
            this.info = info;
        }
    }
}
