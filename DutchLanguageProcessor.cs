using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using Newtonsoft.Json;


namespace Lang
{
    public class DutchLanguageProcessor : LanguageProcessor
    {
        public DutchLanguageProcessor(int count) : base(count)
        {
            this.version = "nl";
            this.language = "Dutch";
        }


        public override void ExportDictionary()
        {
            Language rl = new Language(language);

            StringBuilder res1 = new StringBuilder();
            StringBuilder res2 = new StringBuilder();
            StringBuilder res3 = new StringBuilder();
            string dir3 = GetDir() + "html//searchlist.txt";

            Dictionary<string, int> frequences = new Dictionary<string, int>();


            for (int i = 0; i < rl.words.Count; i++)
            {
                frequences.Add(rl.words[i].spelling, i);
            }

            /*
            Dictionary<string, List<Tuple<int, string>>> apps = new Dictionary<string, List<Tuple<int, string>>>();
            for (int i = 0; i < rl.words.Count; i++)
            {
                for (int j = 0; j<rl.words[i].englishForms.Count; j++)
                {
                    string form = rl.words[i].englishForms[j];
                    int rank = rl.words[i].frequencyRank;
                    string original = rl.words[i].spelling;

                    if (!apps.ContainsKey(form))
                    {
                        apps.Add(form, new List<Tuple<int, string>>());
                    }

                    apps[form].Add(new Tuple<int, string>(rank, original));
                }
            }
            */

            HashSet<string> spellings = new HashSet<string>();
            for (int i = 0; i < rl.words.Count; i++)
            {
                spellings.Add(rl.words[i].spelling);
            }

            for (int i = 0; i < rl.words.Count; i++)
            {
                Word rw = rl.words[i];


                if (!rw.nativeWord)
                {
                    continue;
                }

                if (rw.spelling != null)
                {
                    string word = rw.spelling;
                    int rank = rw.frequencyRank;
                    string original = word;

                    if (rw.nativeWord) {
                        res3.Append(rw.spelling + "\n");
                    }

                    /*
                    if (apps.ContainsKey(word))
                    {
                        //if (apps[word][0].Item1 <= rank)
                        {
                            rank = apps[word][0].Item1;
                            original = apps[word][0].Item2;
                        }
                    }
                    */

                    res1.Append(word + ";" + word + ";" + "normal;" + original + ";" + original + ";" + rank + "\n");
                    res2.Append(word + ";" + word + ";" + original + "\n");

                    for (int j = 0; j < rl.words[i].englishForms.Count; j++)
                    {
                        string form = rl.words[i].englishForms[j];
                        string _original = rw.spelling;

                        if (!spellings.Contains(form)) {
                            if (form.Length > 0) {
                                res1.Append(form + ";" + form + ";" + "form;" + _original + ";" + _original + ";" + rank + "\n");
                                res2.Append(form + ";" + form + ";" + _original + "\n");
                            }
                        }
                    }
                }
            }


            //string dir = GetDir() + "html//dictionaries//"+dictionary.txt";
            string dir2 = "E:\\projects\\Lang\\Lang\\bin\\Debug\\data\\lang\\Russian\\html\\dictionaries\\" + this.version + ".txt";
            //string dir3 = GetDir() + "html//dictionaries//" + searchlist.txt";


            string dir = GetDir() + "html//dictionary.txt";
            //File.WriteAllText(dir, res1.ToString());
            File.WriteAllText(dir2, res2.ToString());
            //File.WriteAllText(dir3, res3.ToString());
        }

        public override void MakeJson()
        {
            WordList dutch = WordlistConstructor.CreateFromTxt("Dutch.txt", ' ', 0, 0, 1000000, true);
            //WordList dutchEnglish = WordlistConstructor.CreateFromTxt("English.txt", ' ', 0, 0, 10000000, false);

            WordList common2 = new WordList();
            WordList common = new WordList();
            common2.dictionary.AddRange(dutch.dictionary);
            //common2.dictionary.AddRange(dutchEnglish.dictionary);
            common2.dictionary.Sort((x, y)=>x.frequencyRank.CompareTo(y.frequencyRank));


            HashSet<string> ws = new HashSet<string>();
            for (int i = 0; i < common2.dictionary.Count; i++)
            {
                if (!ws.Contains(common2.dictionary[i].word))
                {
                    common.dictionary.Add(common2.dictionary[i]);
                    ws.Add(common2.dictionary[i].word);
                }
            }

            /*
            string[] wss = new string[] { "katten", "kat" };
            common.dictionary.Sort(delegate(BasicWord x, BasicWord y) {
                int i1 = x.frequencyRank;
                int i2 = y.frequencyRank;
                if (wss.Contains(x.mainForm))
                {
                    i1 = -4;
                }
                if (wss.Contains(y.mainForm))
                {
                    i2 = -4;
                }
                return i1.CompareTo(i2);
            }
            );
            */

            string rusLang = JsonConvert.SerializeObject(common, Formatting.Indented);
            File.WriteAllText("data/lang/" + language + ".json", rusLang, Encoding.UTF8);
        }

        public override Word Merge(Word englishWord, Word nationalWord)
        {
            if (englishWord == null) {
                return null;
            }

            if (!englishWord.nativeWord)
            {
                englishWord.chunks = new List<HTMLExtractor.Chunk>();
            }

            englishWord.chunks.AddRange(nationalWord.chunks);
            

            englishWord.spelling = nationalWord.spelling;
            englishWord.frequencyRank = nationalWord.frequencyRank;

            if (englishWord.chunks.Count == 0)
            {
                return null;
            }


            return englishWord;
        }

        public override Word ParseNational(BasicWord bw)
        {
            string file = "data/" + version + "_wiktionary/" + bw.word + ".html";
            if (!File.Exists(file))
            {
                Word res = new Word();
                res.spelling = bw.word;
                res.frequencyRank = bw.frequencyRank;
                res.chunks = new List<HTMLExtractor.Chunk>();
                return res;
            }
            else
            {
                string text = File.ReadAllText(file);
                Word res = new Word();
                if (bw.native)
                {
                    var chunks1 = HTMLExtractor.ExtractBlock(text, "Nederlands", "<h2>", "nl");
                    res.chunks.AddRange(chunks1);
                }
                res.spelling = bw.word;
                res.frequencyRank = bw.frequencyRank;
                return res;
            }
        }
    }
}
