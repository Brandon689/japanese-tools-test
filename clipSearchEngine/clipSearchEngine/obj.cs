using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace clipSearchEngine
{
    public class AnimeMAL : BaseEntity
    {

        public bool RETIMED { get; set; }


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
        public int? Episodes { get; set; }
        public int? Year { get; set; }
        public long MalId { get; set; }

        public AnimeMAL()
        {

        }
    }

    public class SubtitleFileForMALId : BaseEntity
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string FileName { get; set; }
        public int Episode { get; set; }
        public int Season { get; set; }
        public string? Title { get; set; }
        private string _title;
        public long MalId { get; set; }
    }

    public class EngSubtitleFileForMALId : SubtitleFileForMALId
    {
    }

    public class JpSubtitleFileForMALId : SubtitleFileForMALId
    {
    }

    public class VideoFileForMALId : BaseEntity
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string FileName { get; set; }
        public int Episode { get; set; }
        public int Season { get; set; }
        public string? Title { get; set; }
        public long MalId { get; set; }
        public bool HasOnlyEnglishSub { get; set; }
        public bool JPSubsTimed { get; set; }
    }

    public class BaseEntity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}