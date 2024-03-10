using SQLite;
using System.Collections.Generic;

namespace TextRomanjiSpeech
{
    public class audiodb
    {
        SQLiteConnection s = new(@"..\..\..\audio.db");
        public audiodb()
        {
            s.CreateTable<audiopart>();
        }
        public void i(audiopart m)
        {
            s.Insert(m);
        }
        public void u(audiopart m)
        {
            s.Update(m);
        }
        public IEnumerable<audiopart> Load()
        {
            return s.Table<audiopart>();
        }
    }

    public class audiopart
    {
        public string text { get; set; }
        public string file { get; set; }
    }
}
