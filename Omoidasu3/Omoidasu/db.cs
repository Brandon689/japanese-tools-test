using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omoidasu
{
    public class DataContext
    {
        private SQLiteConnection db;

        public DataContext()
        {
            db = new("..\\..\\pack.db");
            db.CreateTable<pack>();
            //db.CreateTable<VideoFileForMALId>();
            //db.CreateTable<EngSubtitleFileForMALId>();
            //db.CreateTable<JpSubtitleFileForMALId>();
            //db.CreateTable<ManualInfoMAL>();
        }
        public void Add(IEnumerable<pack> amal)
        {
            db.InsertAll(amal);
        }
        public void UpdateAll(IEnumerable<pack> amal)
        {
            db.UpdateAll(amal);
        }
        public void Add(pack amal)
        {
            db.Insert(amal);
        }
        //public void Add(AnimeFile amal)
        //{
        //    db.Insert(amal);
        //}
        //public void Update(AnimeMAL amal)
        //{
        //    db.Update(amal);
        //}
        public pack[] GetFiles()
        {
            var cg = db.Table<pack>().ToArray();
            return cg;
        }
    }
}
