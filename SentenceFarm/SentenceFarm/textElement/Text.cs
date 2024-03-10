﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Baby
{
    internal class Text
    {
        public List<Glyph> _glyphs;
        //private Glyph[] _glyphs;

        private GlyphTypeface _glyphTypeFace;
        private string _highlightTextString;
        private int _cellIndex1 = -1;
        private int _cellIndex2 = -1;
        private bool _holdingMouseDown;
        private double _fontSize;
        private double scrollbarWidth;
        public Size _renderSize;
        private double _tategakiCellHeight;
        private double _tategakiColumnWidth;
        private double _yokogakiLineSpacing;

        public int HoveredChunkIndex { get; set; } = -1;

        public Text(double fontSize, double lineHeight, double lineWidth)
        {
            _fontSize = fontSize;
            _tategakiCellHeight = lineHeight;
            _tategakiColumnWidth = lineWidth;
            _yokogakiLineSpacing = fontSize * 0.7;
            _glyphs = new();
            var type = new Typeface("Noto Sans JP");
            type.TryGetGlyphTypeface(out _glyphTypeFace);

            _renderSize = new Size(200, 400);
        }

        public List<GlyphRun> Calculate(bool nw)
        {
            List<GlyphRun> list = new();
            double PosX = 0;
            double PosY = _fontSize;
            int glyphIndex = 0;
            double x = 0;
            double y = 0;
            double n = 0;
            string run = string.Empty;
            for (int i = 0; i < _glyphs.Count; i++)
            {
                if (i > 0 && _glyphs[i].ChunkIndex != _glyphs[i - 1].ChunkIndex)
                {
                    makerun(ref run, list, ref PosX, ref PosY, ref n, ref x, ref y, ref glyphIndex, nw);
                }
                if (i > 0 && _glyphs[i].BlockIndex != _glyphs[i - 1].BlockIndex)
                {
                    PosX = 0;
                    if (nw)
                        PosY += _fontSize + _yokogakiLineSpacing;
                    n = 0;
                    x = 0;
                    y = 0;
                }
                run += _glyphs[i].Letter;

                if (i == _glyphs.Count - 1)
                {
                    makerun(ref run, list, ref PosX, ref PosY, ref n, ref x, ref y, ref glyphIndex, nw);
                }

            }
            return list;
        }

        private void makerun(ref string run, List<GlyphRun> list, ref double PosX, ref double PosY, ref double n,
            ref double x, ref double y, ref int glyphIndex, bool newliner)
        {
            if (run.Length == 0) return;
            Point[] glyphOffsets = new Point[run.Length];
            ushort[] glyphIndexes = new ushort[run.Length];
            double[] ZeroAdvanceWidths = new double[run.Length];
            char[] glyphRunCharacters = new char[run.Length];
            Point origin = new(PosX, PosY);
            int meo = 0;
            for (int j = 0; j < run.Length; j++)
            {
                try
                {
                    glyphIndexes[j] = _glyphTypeFace.CharacterToGlyphMap[run[j]];
                }
                catch
                {
                    var free = run[j];
                    ;
                }
                glyphOffsets[j] = new Point(x, y);
                ZeroAdvanceWidths[j] = _glyphTypeFace.AdvanceWidths[glyphIndexes[j]] * _fontSize;
                glyphRunCharacters[j] = run[j];
                int pad = (int)_fontSize / 3;

                Rect bounds = new(
                    n,
                    -y + PosY - (pad / 2) - _fontSize,
                    ZeroAdvanceWidths[j],
                    _fontSize + pad);
                PosX += ZeroAdvanceWidths[j];
                n += ZeroAdvanceWidths[j];
                if (n + _fontSize + scrollbarWidth > _renderSize.Width && j != run.Length - 1)
                {
                    x -= n;
                    n = 0;
                    y -= _fontSize + _yokogakiLineSpacing;
                    ++meo;
                }
                _glyphs[glyphIndex].BoundingBox = bounds;
                ++glyphIndex;
            }
            if (newliner)
                PosY += meo * (_fontSize + _yokogakiLineSpacing);
            list.Add(NewGlyphRun(glyphIndexes, origin, ZeroAdvanceWidths, glyphOffsets, glyphRunCharacters));
            run = string.Empty;
        }

        private double PosYY()
        {
            return _fontSize - ((_glyphTypeFace.Baseline - 1) * _fontSize) + 20;
        }

        public string high()
        {
            _highlightTextString = "";
            var goffset = Math.Min(_cellIndex1, _cellIndex2);
            if (goffset == -1) return "";
            if (_cellIndex1 != -1 && _cellIndex2 != -1)
            {
                for (int i = goffset; i <= Math.Max(_cellIndex1, _cellIndex2); i++)
                {
                    _highlightTextString += _glyphs[i].Letter;
                }
            }
            return _highlightTextString;
        }

        public List<GlyphRun> CalculateV(bool nw)
        {
            List<GlyphRun> list = new();
            double PosX = _renderSize.Width - _tategakiCellHeight;
            double PosY = PosYY();
            int glyphIndex = 0;
            double x = 0;
            double y = 0;
            double n = 0;
            string run = string.Empty;
            for (int i = 0; i < _glyphs.Count; i++)
            {
                if (i > 0 && _glyphs[i].ChunkIndex != _glyphs[i - 1].ChunkIndex)
                {
                    makerunV(ref run, list, ref PosX, ref PosY, ref n, ref x, ref y, ref glyphIndex, nw);
                }
                if (i > 0 && _glyphs[i].BlockIndex != _glyphs[i - 1].BlockIndex)
                {
                    PosX -= _tategakiColumnWidth;
                    PosY = PosYY();
                    n = 0;
                    x = 0;
                    y = 0;
                }
                run += _glyphs[i].Letter;

                if (i == _glyphs.Count - 1)
                {
                    makerunV(ref run, list, ref PosX, ref PosY, ref n, ref x, ref y, ref glyphIndex, nw);
                }
            }
            return list;
        }

        private void makerunV(ref string run, List<GlyphRun> list, ref double PosX, ref double PosY, ref double n,
            ref double x, ref double y, ref int glyphIndex, bool newliner)
        {
            if (run.Length == 0) return;

            Point[] glyphOffsets = new Point[run.Length];
            ushort[] glyphIndexes = new ushort[run.Length];
            double[] ZeroAdvanceWidths = new double[run.Length];
            char[] characters = new char[run.Length];
            Point origin = new(PosX, PosY);
            for (int j = 0; j < run.Length; j++)
            {
                glyphIndexes[j] = _glyphTypeFace.CharacterToGlyphMap[run[j]];
                glyphOffsets[j] = new Point(x, y);
                ZeroAdvanceWidths[j] = 0;
                characters[j] = run[j];
                int pad = 5;
                Rect bounds = new(
                            x + origin.X - (pad / 2),
                            -y + PosY - _fontSize,
                            _glyphTypeFace.AdvanceWidths[glyphIndexes[j]] * _fontSize + pad,
                            _tategakiCellHeight);

                y -= _tategakiCellHeight;
                if (-y + _fontSize + scrollbarWidth > _renderSize.Height - _tategakiCellHeight && j != run.Length - 1)
                {
                    x -= _tategakiColumnWidth;
                    if (newliner)
                        PosX -= _tategakiColumnWidth;
                    y = 0;
                    PosY = PosYY();
                }
                _glyphs[glyphIndex].BoundingBox = bounds;
                ++glyphIndex;
            }
            list.Add(NewGlyphRun(glyphIndexes, origin, ZeroAdvanceWidths, glyphOffsets, characters));
            run = string.Empty;
        }

        public void Append(string item, bool newLine = false)
        {
            int blockIndex = 0;
            int chunkIndex = 0;
            int index = 0;
            if (_glyphs.Count != 0)
            {
                blockIndex = _glyphs.Last().BlockIndex;
                chunkIndex = _glyphs.Last().ChunkIndex + 1;
                index = _glyphs.Last().Index + 1;
                if (newLine)
                {
                    ++blockIndex;
                }
            }
            for (int i = 0; i < item.Length; i++)
            {
                Glyph g = new(item[i], index, chunkIndex, blockIndex);
                _glyphs.Add(g);
                ++index;
            }
        }

        public void MouseDown(Point p)
        {
            _cellIndex2 = -1;
            _cellIndex1 = Contains(p);
            _holdingMouseDown = true;
        }

        public bool MouseMove(Point p)
        {
            var glyph = Containing(p);
            if (glyph == null)
            {
                if (HoveredChunkIndex != -1)
                {
                    HoveredChunkIndex = -1;
                    return true;
                }
                return false;
            }
            bool chng = false;
            if (_holdingMouseDown)
            {
                if (_cellIndex2 != glyph.Index && glyph.Index != -1)
                {
                    _cellIndex2 = glyph.Index;
                    chng = true;
                }
            }
            if (glyph.ChunkIndex != HoveredChunkIndex || chng)
            {
                HoveredChunkIndex = glyph.ChunkIndex;
                return true;
            }
            return false;
        }

        public void MouseUp(Point p)
        {
            _holdingMouseDown = false;
        }

        public int Contains(Point p)
        {
            for (int i = 0; i < _glyphs.Count; i++)
            {
                var r = _glyphs[i].Contains(p);
                if (r)
                {
                    return _glyphs[i].Index;
                }
            }
            return -1;
        }

        public Glyph? Containing(Point p)
        {
            for (int i = 0; i < _glyphs.Count; i++)
            {
                var r = _glyphs[i].Contains(p);
                if (r)
                {
                    return _glyphs[i];
                }
            }
            return null;
        }

        public List<Rect> HighlightedTextBoundingBoxes()
        {
            _highlightTextString = "";
            List<Rect> r = new();
            var goffset = Math.Min(_cellIndex1, _cellIndex2);
            if (goffset == -1) return r;
            //Offset = Glyphs[goffset];
            if (_cellIndex1 != -1 && _cellIndex2 != -1)
            {
                for (int i = goffset; i <= Math.Max(_cellIndex1, _cellIndex2); i++)
                {
                    r.Add(_glyphs[i].BoundingBox);
                    _highlightTextString += _glyphs[i].Letter;
                }
            }
            return r;
        }

        private GlyphRun NewGlyphRun(ushort[] glyphIndexes, Point origin, double[] advanceWidths, Point[] offsets,
            char[] characters, bool bold = false)
        {
            return new GlyphRun(_glyphTypeFace,
                0,
                false,
                _fontSize,
                2.5f,
                glyphIndexes,
                origin,
                advanceWidths,
                offsets,
                characters, null, null, null, null);
        }

        public void Key()
        {
            for (int i = 4; i < _glyphs.Count; i++)
            {
                _glyphs[i].ChunkIndex++;
            }
        }

        public void Log()
        {
            for (int i = 0; i < _glyphs.Count; i++)
            {
                Console.WriteLine(_glyphs[i].ChunkIndex);
            }
        }

        public void Breakdown(int i)
        {
            Breaker.Break(_glyphs, i);
        }

        public void Whole()
        {
            Breaker.Whole(_glyphs);
        }
    }
}
