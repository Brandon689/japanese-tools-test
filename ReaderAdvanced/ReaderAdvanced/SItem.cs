using SQLite;

namespace ReaderAdvanced
{
    public class SItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string RawText { get; set; }
        public string? Breaks { get; set; }

        public bool IsAccent { get; set; }
        public string? Tag { get; set; }

        public string? Color { get; set; }

        public int Index { get; set; }
        public int TextId { get; set; }
    }

    public class TextItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Text { get; set; }
    }
}