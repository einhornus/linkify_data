using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lang
{
    public class WordlistConstructor
    {
        public static List<string> Split(string line)
        {
            char comma = ',';
            char quote = '"';
            bool quoted = false;
            List<string> res = new List<string>();
            string ct = "";
            int last = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == quote)
                {
                    quoted = !quoted;
                }

                if (line[i] == comma && !quoted)
                {
                    res.Add(line.Substring(last, i - last));
                    last = i + 1;
                }
            }
            res.Add(line.Substring(last));

            for (int i = 0; i < res.Count; i++)
            {
                res[i] = res[i].Replace("\\", "");
                res[i] = res[i].Replace("\"", "");
            }
            return res;
        }

        public static bool Check(string w)
        {
            if (w.Length == 0)
            {
                return false;
            }

            if (w[w.Length - 1] == 'ъ')
            {
                return false;
            }

            if (w.Equals("jus") || w.Equals("con") || w.Equals("aux") || w.Equals("cmd") || w.Equals("nul"))
            {
                return false;
            }

            for (int i = 0; i < w.Length; i++)
            {
                bool good = true;

                if (w[i] == ')' || w[i] == '(')
                {
                    return false;
                }

                if (w[i] == ':')
                {
                    return false;
                }

                if (w[i] == '@')
                {
                    return false;
                }

                if (w[i] == '/')
                {
                    return false;
                }

                if ((w[i] >= 'a' && w[i] <= 'z') || (w[i] >= 'а' && w[i] <= 'я') || (w[i] >= '0' && w[i] <= '9') || w[i] == ' ' || w[i] == '.' || w[i] == '-' || w[i] == 'ё')
                {
                    //return false;
                }
                else
                {
                    //return false;
                }

                //if (w[i] >= '0' && w[i] <= '9')
                {
                    //return false;
                }

                //if (w[i] == ' ')
                //{
                //    return false;
                //}
            }

            return true;
        }

        public static WordList CreateFromTxt2(string fileName, char sep, int wi, int fi, int from, int to, bool native)
        {
            WordList res = new WordList();
            string[] lines = File.ReadAllLines(fileName, Encoding.UTF8);

            for (int i = from; i < Math.Min(to, lines.Length); i++)
            {
                string[] lineSep = lines[i].Split(sep);
                string spell = lineSep[wi];
                int freq = int.Parse(lineSep[fi]);

                if (Check(spell))
                {
                    BasicWord bs = new BasicWord("");

                    bs.frequencyRank = freq;
                    bs.word = spell;
                    bs.native = native;


                    res.dictionary.Add(bs);

                }
            }
            return res;
        }


        public static WordList CreateFromTxt(string fileName, char sep, int wi, int from, int to, bool native)
        {
            WordList res = new WordList();
            string[] lines = File.ReadAllLines(fileName, Encoding.UTF8);

            for (int i = from; i < Math.Min(to, lines.Length); i++)
            {
                string[] lineSep = lines[i].Split(sep);
                string spell = lineSep[wi];
                if (Check(spell))
                {
                    BasicWord bs = new BasicWord("");

                    bs.frequencyRank = i - 1;
                    bs.word = spell;
                    bs.native = native;


                    res.dictionary.Add(bs);

                }
            }
            return res;
        }



    }
}
