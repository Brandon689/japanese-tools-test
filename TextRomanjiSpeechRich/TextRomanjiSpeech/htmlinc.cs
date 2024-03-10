using AngleSharp.Html.Parser;
using FlexTextLib;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TextRomanjiSpeech
{
    public class htmlinc
    {
        public string FullText;
        public async Task<List<List<Block>>> f(string h, GlyphTypeface glyphTypeFace)
        {
            FullText = "";
            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(h);
            List<List<Block>> blockss = new List<List<Block>>();
            var f = document.QuerySelector(".custom-select-wrapper");

            List<Block> blocks = new List<Block>();

            int cnt = 0;
            foreach (var item in f.Children)
            {

                //if (item.LocalName == "h2")
                //{
                //    blockss.Add(blocks);
                //    blocks = new List<Block>();
                //}
                //cnt++;
                //if (cnt == 180)
                //{
                //    List<Block> ctn = new();
                //    foreach (var itm in blocks)
                //    {
                //        ctn.Add(itm);
                //    }
                //    blocks.Clear();
                //    blockss.Add(ctn);
                //    cnt = 0;
                //}
                Block block = new Block();
                block.RawText = item.TextContent;
                if (block.RawText.Length == 0)
                {
                    continue;
                }
                if (string.IsNullOrWhiteSpace(block.RawText)) continue;
                foreach (var cha in block.RawText)
                {
                    if (!glyphTypeFace.CharacterToGlyphMap.ContainsKey(cha))
                    {
                        block.RawText = block.RawText.Replace(cha.ToString(), "");
                    }
                }

                if (item.LocalName == "div")
                {
                    var d = From(item.TextContent, glyphTypeFace, "");
                    block.ItemList.Add(d);
                    block.fontsize = 16;
                    blocks.Add(block);
                    block.StartIndex = FullText.Length;
                    FullText += d.GlyphRunString;
                    block.EndIndex = FullText.Length;
                }
                else if (item.LocalName == "p")
                {
                    var d = From(item.TextContent, glyphTypeFace, "");
                    block.ItemList.Add(d);
                    block.fontsize = 16;
                    blocks.Add(block);
                    block.StartIndex = FullText.Length;
                    FullText += d.GlyphRunString;
                    block.EndIndex = FullText.Length;
                }
                else if (item.LocalName == "h1")
                {
                    blocks.Add(new Block(1));
                    var d = From(item.TextContent, glyphTypeFace, "h");
                    block.ItemList.Add(d);
                    block.fontsize = 22;
                    blocks.Add(block);
                    block.StartIndex = FullText.Length;
                    FullText += d.GlyphRunString;
                    block.EndIndex = FullText.Length;
                    blocks.Add(new Block(1));
                }
                else if (item.LocalName == "h2")
                {
                    blocks.Add(new Block(1));

                    var d = From(item.TextContent, glyphTypeFace, "h");
                    block.ItemList.Add(d);
                    block.fontsize = 20;
                    blocks.Add(block);
                    block.StartIndex = FullText.Length;
                    FullText += d.GlyphRunString;
                    block.EndIndex = FullText.Length;
                    blocks.Add(new Block(1));
                }
                else if (item.LocalName == "h3")
                {
                    blocks.Add(new Block(1));

                    var d = From(item.TextContent, glyphTypeFace, "h");
                    block.ItemList.Add(d);
                    block.fontsize = 19;
                    blocks.Add(block);
                    block.StartIndex = FullText.Length;
                    FullText += d.GlyphRunString;
                    block.EndIndex = FullText.Length;
                    blocks.Add(new Block(1));
                }
                else if (item.LocalName == "h4")
                {
                    blocks.Add(new Block(1));

                    var d = From(item.TextContent, glyphTypeFace, "h");
                    block.ItemList.Add(d);
                    block.fontsize = 18;
                    blocks.Add(block);
                    block.StartIndex = FullText.Length;
                    FullText += d.GlyphRunString;
                    block.EndIndex = FullText.Length;
                    blocks.Add(new Block(1));
                }
                else if (item.LocalName == "ul")
                {
                    int step = 1;
                    foreach (var itm in item.Children)
                    {
                        Block bloc = new Block();
                        if (itm.TextContent.Length == 0)
                        {
                            continue;
                        }
                        bloc.RawText = $"....{step}. " + itm.TextContent;

                        foreach (var cha in block.RawText)
                        {
                            if (!glyphTypeFace.CharacterToGlyphMap.ContainsKey(cha))
                            {
                                block.RawText = block.RawText.Replace(cha.ToString(), "");
                            }
                        }
                        var dk = From(itm.TextContent, glyphTypeFace, "", step);
                        bloc.StartIndex = FullText.Length;
                        FullText += dk.GlyphRunString;
                        bloc.EndIndex = FullText.Length;

                        bloc.ItemList.Add(dk);
                        bloc.fontsize = 15;
                        blocks.Add(bloc);
                    }
                }
                else
                {
                    var d = From(item.TextContent, glyphTypeFace, "");
                    block.ItemList.Add(d);
                    block.fontsize = 16;
                    blocks.Add(block);
                    block.StartIndex = FullText.Length;
                    FullText += d.GlyphRunString;
                    block.EndIndex = FullText.Length;
                }





            }
            //for (int i = 0; i < blocks.Count; i++)
            //{
            //    for (int j = 0; j < blocks[i].ItemList.Count; j++)
            //    {
            //        blocks[i].ItemList[j].
            //    }
            //}
            for (int i = 0; i < blocks.Count - 400; i += 200)
            {
                blockss.Add(blocks.GetRange(i, 200));
            }
            //blockss.Add(blocks);
            return blockss;
        }

        Chunk From(string item, GlyphTypeface glyphTypeFace, string tag, int step = -1)
        {
            foreach (var cha in item)
            {
                if (!glyphTypeFace.CharacterToGlyphMap.ContainsKey(cha))
                {
                    item = item.Replace(cha.ToString(), "");
                }
            }
            Chunk g = new();
            if (step == -1)
            {
                g.GlyphRunString = item;

                if (tag == "h")
                {
                    //g.GlyphRunString = $"\r\n{g.GlyphRunString}\r\n";
                }
            }
            else
            {
                g.GlyphRunString = "      " + step + ". " + item;
            }
            g.StartIndex = FullText.Length;
            g.EndIndex = FullText.Length + item.Length;
            return g;
        }
    }
}
