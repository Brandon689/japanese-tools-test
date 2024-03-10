using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clipSearchEngine
{
    public class DataContext
    {
        private SQLiteConnection db;

        public DataContext()
        {
            db = new(@"C:\GITHUB\myLists.db");
        }

        public bool Ex(AnimeMAL amal)
        {
            return db.Table<AnimeMAL>().Where(x => x.MalId == amal.MalId).FirstOrDefault() != null;
        }

        public IEnumerable<AnimeMAL> Get()
        {
            return db.Table<AnimeMAL>();
        }

        public bool ExV(long? malid)
        {
            return db.Table<VideoFileForMALId>().Where(x => x.MalId == malid).FirstOrDefault() != null;
        }

        public IEnumerable<VideoFileForMALId> GetV()
        {
            return db.Table<VideoFileForMALId>();
        }

        public IEnumerable<VideoFileForMALId> GetV(long? malid)
        {
            return db.Table<VideoFileForMALId>().Where(x => x.MalId == malid);
        }

        public bool ExS(long? malid)
        {
            return db.Table<JpSubtitleFileForMALId>().Where(x => x.MalId == malid).FirstOrDefault() != null;
        }

        public IEnumerable<JpSubtitleFileForMALId> GetS()
        {
            return db.Table<JpSubtitleFileForMALId>();
        }

        public IEnumerable<JpSubtitleFileForMALId> GetS(long? malid)
        {
            return db.Table<JpSubtitleFileForMALId>().Where(x => x.MalId == malid);
        }

        public bool ExSE(long? malid)
        {
            return db.Table<EngSubtitleFileForMALId>().Where(x => x.MalId == malid).FirstOrDefault() != null;
        }

        public IEnumerable<EngSubtitleFileForMALId> GetSE()
        {
            return db.Table<EngSubtitleFileForMALId>();
        }

        public IEnumerable<EngSubtitleFileForMALId> GetSE(long? malid)
        {
            return db.Table<EngSubtitleFileForMALId>().Where(x => x.MalId == malid);
        }
    }
}