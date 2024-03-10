using System.Collections.Generic;
using System.Linq;

namespace whatever
{
    public static class Class3
    {
        public static void A(Sub s, List<string> reject, IEnumerable<SubtitleFileForMALId> a)
        {
            foreach (var item in a)
            {
                var b = s.ParseFileAny(item.FileName);
                reject.AddRange(b.SelectMany(x => x.PlaintextLines));
            }
        }

        public static void A2(List<snow> jo, Sub s, List<string> r, IEnumerable<SubtitleFileForMALId> a)
        {
            foreach (var item in a)
            {
                var b = s.ParseFileAny(item.FileName);
                jo.AddRange(b.Select(x => new snow(x, item.FileName)));
                r.AddRange(b.SelectMany(x => x.PlaintextLines));
            }
        }


        public static List<string> S(List<string> line, int look)
        {
            List<string> ret = new();
            if (line.Count < look) return ret;

            for (int i = 0; i < line.Count; i++)
            {
                if (i == line.Count - look) break;
                string way = line[i];

                for (int j = 1; j < look; j++)
                {
                    way += line[j];
                }

                ret.Add(way);
            }
            return ret;
        }
    }
}
