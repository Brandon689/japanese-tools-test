using SQLite;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace whatever
{
    public class SubtitleFileForMALId : BaseEntity
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

    public class EngSubtitleFileForMALId : SubtitleFileForMALId
    {
    }
    public class JpSubtitleFileForMALId : SubtitleFileForMALId
    {
    }



    public class BaseEntity : INotifyPropertyChanged
    {
        private DateTime _createTime;
        private DateTime _updateTime;

        public DateTime CreateTime
        {
            get => _createTime;
            set
            {
                if (value == _createTime) return;
                _createTime = value;
                OnPropertyChanged();
            }
        }

        public DateTime UpdateTime
        {
            get => _updateTime;
            set
            {
                if (value == _updateTime) return;
                _updateTime = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class AnimeMAL : BaseEntity
    {

        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        [Ignore]
        public bool InDB { get; set; }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged();
            }
        }
        public string Url { get; set; }
        private string _picPath;
        public string PicPath
        {
            get => _picPath;
            set
            {
                if (value == _picPath) return;
                _picPath = value;
                OnPropertyChanged();
            }
        }

        public string Background { get; set; }
        public string TitleJapanese { get; set; }
        public double? Score { get; set; }
        public int? Members { get; set; }
        public string Type { get; set; }

        public int? Popularity { get; set; }

        public int? Favorites { get; set; }
        public string Synopsis { get; set; }
        public bool Watching { get; set; }
        //public Season? Season { get; set; }

        //public ICollection<MalUrl> Producers { get; set; }
        //public ICollection<MalUrl> Licensors { get; set; }
        //public ICollection<MalUrl> Studios { get; set; }
        //public ICollection<MalUrl> Genres { get; set; }
        //public ICollection<MalUrl> ExplicitGenres { get; set; }
        //public ICollection<MalUrl> Themes { get; set; }
        //public ICollection<MalUrl> Demographics { get; set; }

        public int? Episodes { get; set; }
        public int? Year { get; set; }
        public long? MalId { get; set; }


        public AnimeMAL()
        {

        }

        public AnimeMAL(JikanDotNet.Anime copy, string img)
        {
            MalId = copy.MalId;

            Url = copy.Url;
            Episodes = copy.Episodes;
            Title = copy.Title;
            Year = copy.Year;
            Score = copy.Score;
            Members = copy.Members;
            TitleJapanese = copy.TitleJapanese;
            Background = copy.Background;
            PicPath = img;
        }
    }
}