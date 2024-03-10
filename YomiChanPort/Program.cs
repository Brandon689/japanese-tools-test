using System.Text;
using YomiChanPort;
Console.OutputEncoding = Encoding.UTF8;


range r = new();

var a = r.isStringEntirelyKana("かたかな");
a = r.isStringPartiallyJapanese("AAか");
Console.WriteLine(a);

var p = r.convertHiraganaToKatakana("かたか");
Console.WriteLine(p);
//char c = 'ひ';


//Console.WriteLine((int)c);
//var e = (char)0;

//var k = (int)e;
//var d = (char)2;
//Console.WriteLine((char)0);