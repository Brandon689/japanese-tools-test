namespace whatever
{
    public class FreqData
    {
        public string text { get; set; }
        public int freq { get; set; }
        public FreqData()
        {

        }
        public FreqData(string m)
        {
            this.text = m;
            this.freq = 1;
        }
        public FreqData(string text, int free)
        {
            this.text = text;
            this.freq = free;
        }
        public override string ToString()
        {
            return text + " " + freq;
        }
    }
}
