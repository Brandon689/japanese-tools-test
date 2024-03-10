using System.Collections.Generic;

namespace ReaderAdvanced
{
    internal interface IFrameEl
    {
        bool Freeze { get; set; }
        string FullText { get; set; }
        string HoveredText { get; }

        void Add(string item, bool newLine = false);
        void AddRange(IEnumerable<string> items);
        void Clear();
        void Invalidate();
    }
}