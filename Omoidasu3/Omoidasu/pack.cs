using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omoidasu
{
    public class pack
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int card { get; set; }
        public int deck { get; set; }
        public int misscount { get; set; }
        public int wincount { get; set; }
        public int skipcount { get; set; }
        public int impressioncount { get; set; }
        public double timespentseconds { get; set; } // tracks amount of time u spent on card
    }
}
