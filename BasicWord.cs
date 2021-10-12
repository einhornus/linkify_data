using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lang
{
    public class BasicWord
    {
        public string word;
        public int frequencyRank;
        public bool native;

        public BasicWord(string w)
        {
            word = w;
            frequencyRank = 0;
            native = true;
        }
    }
}
