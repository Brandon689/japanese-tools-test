using SQLite;
using System.Collections.Generic;

namespace whatever
{
    public class MALDataContext
    {
        private SQLiteConnection db;

        public MALDataContext()
        {
            db = new(@"C:\なじみ\bluelock\Animesan\Anime\myLists.db");
            db.CreateTable<AnimeMAL>();
            db.CreateTable<VideoFileForMALId>();
            db.CreateTable<EngSubtitleFileForMALId>();
            db.CreateTable<JpSubtitleFileForMALId>();
        }

        public void Add(AnimeMAL amal)
        {
            db.Insert(amal);
        }
        public void Update(AnimeMAL amal)
        {
            db.Update(amal);
        }

        public bool Ex(AnimeMAL amal)
        {
            return db.Table<AnimeMAL>().Where(x => x.MalId == amal.MalId).FirstOrDefault() != null;
        }

        public IEnumerable<AnimeMAL> Get()
        {
            return db.Table<AnimeMAL>();
        }


        public void AddV(VideoFileForMALId amal)
        {
            db.Insert(amal);
        }
        public void Update(VideoFileForMALId amal)
        {
            db.Update(amal);
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

        public void AddS(JpSubtitleFileForMALId amal)
        {
            db.Insert(amal);
        }
        public void Update(JpSubtitleFileForMALId amal)
        {
            db.Update(amal);
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

        public void AddSE(EngSubtitleFileForMALId amal)
        {
            db.Insert(amal);
        }
        public void Update(EngSubtitleFileForMALId amal)
        {
            db.Update(amal);
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
        public void DeleV(long? malid)
        {
            var sheem = GetV(malid);
            foreach (var item in sheem)
            {
                db.Delete(item);
            }
        }
        public void DeleSE(long? malid)
        {
            var sheem = GetSE(malid);
            foreach (var item in sheem)
            {
                db.Delete(item);
            }
        }
        public void DeleS(long? malid)
        {
            var sheem = GetS(malid);
            foreach (var item in sheem)
            {
                db.Delete(item);
            }
        }
    }




    public class VideoFileForMALId : BaseEntity
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public string FileName { get; set; }

        public int Episode { get; set; }
        public int Season { get; set; }
        public string? Title { get; set; }

        //private string _title;
        //public string Title
        //{
        //    get => _title;
        //    set
        //    {
        //        if (value == _title) return;
        //        _title = value;
        //        OnPropertyChanged();
        //    }
        //}

        public long? MalId { get; set; }
    }




}
