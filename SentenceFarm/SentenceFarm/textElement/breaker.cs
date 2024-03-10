using Baby;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baby
{
    public static class Breaker
    {
        public static void Break(List<Glyph> body, int index)
        {
            if (Check(body, index)) return;
            if (Check2(body, index)) return;
            for (int i = index; i < body.Count; i++)
            {
                body[i].ChunkIndex++;
            }
        }
        public static void Whole(List<Glyph> body)
        {
            for (int i = 0; i < body.Count; i++)
            {
                body[i].ChunkIndex = 0;
            }
        }
        public static void Shouri(List<Glyph> body, List<uint> skeleton)
        {
            for (int i = 0; i < body.Count; i++)
            {
                body[i].ChunkIndex = (int)skeleton[i];
            }
        }
        private static bool Check(List<Glyph> body, int index)
        {
            if (index >= body.Count || index <= 0) return true;
            return false;
        }
        private static bool Check2(List<Glyph> body, int index)
        {
            if (body[index - 1].ChunkIndex < body[index].ChunkIndex)
            {
                return true;
            }
            return false;
        }
        // if we make insert at index go BEFORE <INDEX> that would work same as string insert
        // but it make this implementation messy
        // so not go there yet
        //public static void Add(List<Glyph> body, int index, string addition, bool newBlock = false)
        //{
        //    //if (Check(body, index)) return;
        //    if (Check(body, index - 1)) return;
        //    Stopwatch ss = new();
        //    ss.Start();

        //    int blockIndex = 0;

        //    if (index >= body.Count)
        //    {
        //        blockIndex = body.Last().BlockIndex;
        //    }
        //    else
        //    {
        //        blockIndex = body[index].BlockIndex;
        //    }
        //    if (newBlock) ++blockIndex;

        //    int st = index;
        //    List<Glyph> newbee = new();
        //    for (int i = 0; i < addition.Length; i++)
        //    {
        //        var c = new Glyph(addition[i], st + 1, blockIndex);
        //        ++st;
        //        newbee.Add(c);
        //    }
        //    if (index! >= body.Count)
        //    //{

        //    // }
        //    //else
        //    {
        //        if (newBlock)
        //        {
        //            for (int i = index; i < body.Count; i++)
        //            {
        //                body[i].Index += addition.Length;
        //                body[i].BlockIndex++;
        //            }
        //        }
        //        else
        //        {
        //            for (int i = index; i < body.Count; i++)
        //            {
        //                body[i].Index += addition.Length;
        //            }
        //        }
        //    }
        //    // this is much slower
        //    //body.AddRange(newbee);
        //    //Sort(body);
        //    //if (index == body.Count - 1)
        //    //{
        //    //    body.AddRange(newbee);
        //    //}
        //    //else
        //    //{
        //    body.InsertRange(index, newbee);
        //    //}

        //    ss.Stop();
        //    Console.WriteLine(ss.ElapsedTicks);
        //}

        //public static void Swap(List<Glyph> body, int blockA, int blockB)
        //{
        //    var a = body.Where(x => x.BlockIndex == blockA).ToList();
        //    var b = body.Where(x => x.BlockIndex == blockB).ToList();
        //    if (a.Count == 0 || b.Count == 0) return;
        //    int trn = b.First().Index;
        //    int trnT = a.First().Index;
        //    int checkA = trn;
        //    int checkB = trnT;
        //    foreach (Glyph item in a)
        //    {
        //        item.BlockIndex = blockB;
        //        item.Index = trn;
        //        ++trn;
        //    }
        //    foreach (Glyph item in b)
        //    {
        //        item.BlockIndex = blockA;
        //        item.Index = trnT;
        //        ++trnT;
        //    }

        //    if (a[0].Index != checkA)
        //    {
        //        throw new Exception("he");
        //    }
        //    if (b[0].Index != checkB)
        //    {
        //        throw new Exception("he");
        //    }
        //    Sort(body);
        //}

        //public static void Swap2(List<Glyph> body, int blockA, int blockB)
        //{
        //    var a = body.Where(x => x.BlockIndex == blockA).ToList();
        //    var b = body.Where(x => x.BlockIndex == blockB).ToList();
        //    if (a.Count == 0 || b.Count == 0) return;
        //    int trn = b.First().Index;
        //    int trnT = a.First().Index;
        //    int checkA = trnT;
        //    int checkB = trn;

        //    foreach (Glyph item in a)
        //    {
        //        item.BlockIndex = blockB;
        //        item.Index = trn;
        //        ++trn;
        //    }
        //    foreach (Glyph item in b)
        //    {
        //        item.BlockIndex = blockA;
        //        item.Index = trnT;
        //        ++trnT;
        //    }

        //    body.RemoveRange(checkA, a.Count);

        //    body.InsertRange(checkB, a);

        //    body.RemoveRange(checkB - a.Count, b.Count);

        //    body.InsertRange(checkA, b);

        //    //Sort(body);
        //}

        // wip PUT
        // puts block in place (after) of block b
        //public static void Put(List<Glyph> body, int blockA, int blockB)
        //{
        //    var a = body.Where(x => x.BlockIndex == blockA).ToList();
        //    var b = body.Where(x => x.BlockIndex == blockB).ToList();
        //    if (a.Count == 0 || b.Count == 0) return;
        //    int trn = b.First().Index;
        //    int trnT = a.First().Index;
        //    int checkA = trnT;
        //    int checkB = trn;

        //    foreach (Glyph item in a)
        //    {
        //        item.BlockIndex = blockB;
        //        item.Index = trn;
        //        ++trn;
        //    }
        //    foreach (Glyph item in b)
        //    {
        //        item.BlockIndex = blockA;
        //        item.Index = trnT;
        //        ++trnT;
        //    }

        //    body.RemoveRange(checkA, a.Count);

        //    body.InsertRange(checkB, a);

        //    // body.RemoveRange(trn, b.Count);

        //    // body.InsertRange(trnT , b);

        //    //Sort(body);
        //}

        private static void Sort(List<Glyph> body)
        {
            body.Sort((x, y) => x.Index.CompareTo(y.Index));
        }
    }
}