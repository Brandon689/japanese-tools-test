using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cmdlib
{
    public static class SubtitleFileParser
    {
        public static string SubBodyAllRemNames(List<SubtitleModel> sc)
        {
            string r = "";
            var pattern = @"\（.*\）";


            for (int i = 0; i < sc.Count; i++)
            {
                // do this properly with punctuation todo
                sc[i].Text = sc[i].Text.Replace("　", "");
                sc[i].Text = sc[i].Text.Replace("～", "");
                sc[i].Text = sc[i].Text.Replace("？", "");
                sc[i].Text = sc[i].Text.Replace("！", "");
                sc[i].Text = sc[i].Text.Replace("…", "");
                sc[i].Text = sc[i].Text.Replace("１", "");
                sc[i].Text = sc[i].Text.Replace("１", "");
                sc[i].Text = sc[i].Text.Replace("“", "");
                sc[i].Text = sc[i].Text.Replace("”", "");
                // end


                if (sc[i].Text[0] == '（')
                {
                    sc[i].Text = Regex.Replace(sc[i].Text, pattern, string.Empty);
                }
                r += sc[i].Text.Replace("\r\n", "").Replace(" ", "").Replace("  ", "").Replace("   ", "").Replace("(", "").Replace(")", "").Trim();
                //r = r.Replace("（", "");
                //r = r.Replace("）", "");
            }
            return r;
        }

        public static string SubBodyAll(List<SubtitleModel> sc)
        {
            string r = "";
            for (int i = 0; i < sc.Count; i++)
            {
                r += sc[i].Text.Replace("\r\n", "").Replace(" ", "").Replace("  ", "").Replace("   ", "").Replace("(", "").Replace(")", "").Trim();
                r = r.Replace("（", "");
                r = r.Replace("）", "");
            }
            return r;
        }

        public static List<SubtitleModel> ReadSubEngFile(string srtFile)
        {
            int subnum = 1;
            List<SubtitleModel> envy = new();
            List<string> lines = new();
            int cnt = 0;
            int blockcounter = 0;
            foreach (string line in File.ReadLines(srtFile))
            {
                lines.Add(line);
                ++blockcounter;
                if (line == "")
                {
                    SubtitleModel subtit = new();
                    subtit.SubNumber = subnum;
                    int sublines = blockcounter - 3;
                    int sublinesread = blockcounter - 3;
                    while (sublines > 0)
                    {
                        subtit.Text += lines[cnt - sublines] + "\r\n";
                        --sublines;
                    }
                    blockcounter = 0;
                    envy.Add(subtit);
                    ++subnum;
                }
                ++cnt;
            }
            return envy;
        }

        public static List<SubtitleModel> ReadSubFile(string srtFileLoc)
        {
            int subnum = 1;
            List<SubtitleModel> envy = new();
            List<string> lines = new();
            int cnt = 0;
            int blockcounter = 0;
            foreach (string line in File.ReadLines(srtFileLoc))
            {
                lines.Add(line);
                ++blockcounter;
                if (line == "")
                {
                    SubtitleModel subtit = new();
                    subtit.SubNumber = subnum;
                    int sublines = blockcounter - 3;
                    int sublinesread = blockcounter - 3;
                    while (sublines > 0)
                    {
                        subtit.Text += lines[cnt - sublines] + "\r\n";
                        --sublines;
                    }
                    blockcounter = 0;
                    subtit.RawDateString = lines[cnt - sublinesread - 1];
                    DateString(lines[cnt - sublinesread - 1], subtit);

                    if (subtit.Text.Contains("?"))
                        subtit.SentenceType = 2;

                    envy.Add(subtit);
                    ++subnum;
                }
                ++cnt;
            }
            return envy;
        }

        public static long DateStringToMilli(string date)
        {
            SubtitleModel sub = new();
            DateStringOne(date, sub);
            sub.MillisecondTime = sub.Hour * 3600000;
            sub.MillisecondTime += sub.Minute * 60000;
            sub.MillisecondTime += sub.Second * 1000;
            sub.MillisecondTime += sub.Millisecond;
            return sub.MillisecondTime;
        }

        public static string SubToDateMPVString(SubtitleModel s)
        {
            string hour = "0" + s.Hour.ToString();
            string minute = s.Minute.ToString();
            if (minute.Length == 1)
            {
                minute = "0" + minute;
            }
            string second = s.Second.ToString();
            if (second.Length == 1)
            {
                second = "0" + second;
            }
            string millisecond = s.Millisecond.ToString();
            if (millisecond.Length == 1)
            {
                millisecond = "00" + millisecond;
            }
            else if (millisecond.Length == 2)
            {
                millisecond = "0" + millisecond;
            }
            return $"{hour}:{minute}:{second}.{millisecond}";
        }

        public static string SubToDateMPVStringtest(SubtitleModel s)
        {
            string hour = "0" + s.Hour.ToString();
            string minute = s.Minute.ToString();
            if (minute.Length == 1)
            {
                minute = "0" + minute;
            }
            string second = (s.Second - 1).ToString();
            if (second.Length == 1)
            {
                second = "0" + second;
            }
            string millisecond = s.Millisecond.ToString();
            if (millisecond.Length == 1)
            {
                millisecond = "00" + millisecond;
            }
            else if (millisecond.Length == 2)
            {
                millisecond = "0" + millisecond;
            }
            return $"{hour}:{minute}:{second}.{millisecond}";
        }

        public static string SubToDateMPVStringtestto(SubtitleModel s)
        {
            string hour = "0" + s.ToHour.ToString();
            string minute = s.ToMinute.ToString();
            if (minute.Length == 1)
            {
                minute = "0" + minute;
            }
            string second = (s.ToSecond).ToString();
            if (second.Length == 1)
            {
                second = "0" + second;
            }
            string millisecond = s.Millisecond.ToString();
            if (millisecond.Length == 1)
            {
                millisecond = "00" + millisecond;
            }
            else if (millisecond.Length == 2)
            {
                millisecond = "0" + millisecond;
            }
            return $"{hour}:{minute}:{second}.{millisecond}";
        }

        public static TimeSpan Subspanfrom(SubtitleModel s)
        {
            return new TimeSpan(0, s.Hour, s.Minute, s.Second, s.Millisecond);
        }

        public static TimeSpan Subspanto(SubtitleModel s)
        {
            return new TimeSpan(0, s.ToHour, s.ToMinute, s.ToSecond, s.ToMillisecond);
        }

        public static string SubToDat(TimeSpan interval)
        {
            string hour = "0" + interval.Hours.ToString();
            string minute = interval.Minutes.ToString();
            if (minute.Length == 1)
            {
                minute = "0" + minute;
            }
            string second = (interval.Seconds).ToString();
            if (second.Length == 1)
            {
                second = "0" + second;
            }
            string millisecond = interval.Milliseconds.ToString();
            if (millisecond.Length == 1)
            {
                millisecond = "00" + millisecond;
            }
            else if (millisecond.Length == 2)
            {
                millisecond = "0" + millisecond;
            }
            return $"{hour}:{minute}:{second},{millisecond}";
        }


        public static string Flip(SubtitleModel s, bool from)
        {
            TimeSpan interval;
            if (from)
            {
                interval = new TimeSpan(0, s.Hour, s.Minute, s.Second, s.Millisecond);
                interval = interval.Subtract(new TimeSpan(0, 0, 0, 0, 150));
                ;
            }
            else
            {
                interval = new TimeSpan(0, s.ToHour, s.ToMinute, s.ToSecond, s.ToMillisecond);
                interval = interval.Add(new TimeSpan(0, 0, 0, 0, 150));
            }

            string hour = "0" + interval.Hours.ToString();
            string minute = interval.Minutes.ToString();
            if (minute.Length == 1)
            {
                minute = "0" + minute;
            }
            string second = interval.Seconds.ToString();
            if (second.Length == 1)
            {
                second = "0" + second;
            }
            string millisecond = interval.Milliseconds.ToString();
            if (millisecond.Length == 1)
            {
                millisecond = "00" + millisecond;
            }
            else if (millisecond.Length == 2)
            {
                millisecond = "0" + millisecond;
            }
            return $"{hour}:{minute}:{second}.{millisecond}";
        }


        //***************************** parse date string
        public static void DateString(string date, SubtitleModel subtit)
        {
            var str = date.Split(" --> ");
            subtit.DateStringFrom = str[0];
            subtit.DateStringTo = str[1];
            var nex = str[0].Split(",");
            subtit.Millisecond = int.Parse(nex[1]);

            var tree = nex[0].Split(":");

            if (tree[0][0] == '0')
            {
                tree[0] = tree[0][1].ToString();
            }
            if (tree[1][0] == '0')
            {
                tree[1] = tree[1][1].ToString();
            }
            if (tree[2][0] == '0')
            {
                tree[2] = tree[2][1].ToString();
            }
            subtit.Hour = int.Parse(tree[0]);
            subtit.Minute = int.Parse(tree[1]);
            subtit.Second = int.Parse(tree[2]);

            var nex2 = str[1].Split(",");
            subtit.ToMillisecond = int.Parse(nex2[1]);

            var tree2 = nex2[0].Split(":");

            if (tree2[0][0] == '0')
            {
                tree2[0] = tree2[0][1].ToString();
            }
            if (tree2[1][0] == '0')
            {
                tree2[1] = tree2[1][1].ToString();
            }
            if (tree2[2][0] == '0')
            {
                tree2[2] = tree2[2][1].ToString();
            }
            subtit.ToHour = int.Parse(tree2[0]);
            subtit.ToMinute = int.Parse(tree2[1]);
            subtit.ToSecond = int.Parse(tree2[2]);
        }

        public static void DateStringOne(string date, SubtitleModel subtit) // date form = 00:27:13,800
        {
            var nex = date.Split(",");
            subtit.Millisecond = int.Parse(nex[1]);
            var tree = nex[0].Split(":");

            if (tree[0][0] == '0')
            {
                tree[0] = tree[0][1].ToString();
            }
            if (tree[1][0] == '0')
            {
                tree[1] = tree[1][1].ToString();
            }
            if (tree[2][0] == '0')
            {
                tree[2] = tree[2][1].ToString();
            }
            subtit.Hour = int.Parse(tree[0]);
            subtit.Minute = int.Parse(tree[1]);
            subtit.Second = int.Parse(tree[2]);
        }
        //**************************************
    }
}
