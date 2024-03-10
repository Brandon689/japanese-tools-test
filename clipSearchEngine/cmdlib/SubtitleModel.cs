using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmdlib
{
    public class SubtitleModel
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? Translation { get; set; }
        public int SentenceType { get; set; }
        public string? AudioFileSrc { get; set; } // subtitle read
        public string? SrcMedia { get; set; }
        public string? SrcMediaLocation { get; set; }
        public string? ScreenshotLocation { get; set; }
        public string? DateStringFrom { get; set; }
        public string? DateStringTo { get; set; }

        public string? RawDateString { get; set; }

        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public int Millisecond { get; set; }

        public int ToHour { get; set; }
        public int ToMinute { get; set; }
        public int ToSecond { get; set; }
        public int ToMillisecond { get; set; }
        public int SubNumber { get; set; }

        public long MillisecondTime { get; set; }
        public long MillisecondTimeTo { get; set; }
        public long MillisecondTimeRange { get; set; }
        public long MillisecondMid { get; set; }

        public long MillisecondToNext { get; set; }

        public TimeSpan FromSpan { get; set; }
        public TimeSpan ToSpan { get; set; }

        public double StartOfDuration { get; set; }// percent where mid of subtitle is of video duration

        public override string ToString()
        {
            string write = "";
            write += SubNumber + "\r\n";
            write += RawDateString + "\r\n";
            write += Text + "\r\n";
            return write;
        }
    }
}
