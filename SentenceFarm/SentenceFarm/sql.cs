using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentenceFarm
{
    internal class sql
    {
        SQLiteConnection s = new(@"C:\GITHUB\SentenceFarm\farm.db");
        //SQLiteConnection s = new("../../../farm.db");
        public sql()
        {
            s.CreateTable<koi>();
        }

        public void u(koi m)
        {
            s.Update(m);
        }
        public IEnumerable<koi> Load()
        {
            return s.Table<koi>();
        }

        public void i(koi m)
        {
            s.Insert(m);
        }
    }
}
