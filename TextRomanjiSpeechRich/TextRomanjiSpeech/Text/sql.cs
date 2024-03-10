using SQLite;
using System.Collections.Generic;

namespace FlexTextLib
{
    public class sql
    {
        SQLiteConnection s = new(@"F:\New folder\annotations.db");
        public sql()
        {
            s.CreateTable<SItem>();
            s.CreateTable<TextItem>();
        }
        public void i(SItem m)
        {
            s.Insert(m);
        }
        public void u(SItem m)
        {
            s.Update(m);
        }
        public IEnumerable<SItem> Load()
        {
            return s.Table<SItem>();
        }
        public IEnumerable<TextItem> LoadT()
        {
            return s.Table<TextItem>();
        }
        public int IQ(TextItem tex)
        {
            return s.Insert(tex);
        }
    }
}
