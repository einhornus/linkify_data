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
    public class GenericLanguageProcessor : LanguageProcessor
    {
        public GenericLanguageProcessor(int count, string full, string _short) : base(count)
        {
            this.version = _short;
            this.language = full;
            //ParseBadLemmas();
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


            Dictionary<string, List<Tuple<int, string>>> apps = new Dictionary<string, List<Tuple<int, string>>>();


            for (int i = 0; i < rl.words.Count; i++)
            {
                for (int j = 0; j < rl.words[i].englishForms.Count; j++)
                {
                    string form = rl.words[i].englishForms[j];
                    int rank = rl.words[i].frequencyRank;
                    string original = rl.words[i].spelling;

                    /*
                    if (frequences.ContainsKey(form))
                    {
                        int index = frequences[form];
                        appearances[index].Add(rl.words[i].spelling);
                    }
                    */

                    if (!apps.ContainsKey(form))
                    {
                        apps.Add(form, new List<Tuple<int, string>>());
                    }

                    apps[form].Add(new Tuple<int, string>(rank, original));
                }
            }

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

                    if (rw.nativeWord)
                    {
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

                        if (!form.Equals("-"))
                        {
                            if (!spellings.Contains(form))
                            {
                                if (form.Length > 0)
                                {
                                    res1.Append(form + ";" + form + ";" + "form;" + _original + ";" + _original + ";" + rank + "\n");
                                    res2.Append(form + ";" + form + ";" + _original + "\n");
                                }
                            }
                        }
                    }
                }
            }

            string dir = "E:\\unilang_server\\data\\dictionaries\\" + this.version + ".txt";

            string dir2 = "E:\\projects\\Lang\\Lang\\bin\\Debug\\data\\lang\\Russian\\html\\dictionaries\\" + this.version + ".txt";


            //string dir2 = GetDir() + "html//dictionary_simple.txt";
            File.WriteAllText(dir, res1.ToString());
            File.WriteAllText(dir2, res2.ToString());
            File.WriteAllText(dir3, res3.ToString());
        }

        public override void MakeJson()
        {
            WordList nat = WordlistConstructor.CreateFromTxt(this.language+".txt", ' ', 0, 0, 1000000000, true);
            WordList lemmas = WordlistConstructor.CreateFromTxt(this.language+"_lemmas.txt", '|', 0, 0, 10000000, true);


            Dictionary<string, int> nn = new Dictionary<string, int>();
            for (int i = 0; i < nat.dictionary.Count; i++)
            {
                nn.Add(nat.dictionary[i].word, i);
            }

            lemmas.dictionary.Sort((x, y) =>
            {
                int f1 = 100000000;
                int f2 = 100000000;

                if (nn.ContainsKey(x.word))
                {
                    f1 = nn[x.word];
                }

                if (nn.ContainsKey(y.word))
                {
                    f2 = nn[y.word];
                }

                return f1.CompareTo(f2);
            });

            /*
            HashSet<string> lem = new HashSet<string>();
            for (int i = 0; i<lemmas.dictionary.Count; i++)
            {
                lem.Add(lemmas.dictionary[i].word);
            }
            */

            if (this.version.Equals("en")) {

                WordList nnn = new WordList();
                for (int i = 0; i < lemmas.dictionary.Count; i++)
                {
                    if (nn.ContainsKey(lemmas.dictionary[i].word))
                    {
                        int index = nn[lemmas.dictionary[i].word];
                        if (index <= 100000000) {
                            nnn.dictionary.Add(lemmas.dictionary[i]);
                            nnn.dictionary[nnn.dictionary.Count - 1].frequencyRank = nn[lemmas.dictionary[i].word];
                        }
                    }
                    else
                    {
                        //nnn.dictionary.Add(lemmas.dictionary[i]);
                    }
                }

                string rusLang = JsonConvert.SerializeObject(nnn, Formatting.Indented);
                File.WriteAllText("data/lang/" + language + ".json", rusLang, Encoding.UTF8);
            }
            else
            {
                string rusLang = JsonConvert.SerializeObject(lemmas, Formatting.Indented);
                File.WriteAllText("data/lang/" + language + ".json", rusLang, Encoding.UTF8);
            }


        }

        public override Word Merge(Word englishWord, Word nationalWord)
        {
            if (englishWord == null)
            {
                return null;
            }

            if (englishWord.chunks == null)
            {
                return null;
            }

            if (englishWord.chunks.Count == 0)
            {
                return null;
            }

            return englishWord;
        }

        public override Word ParseNational(BasicWord bw)
        {
            return null;
        }
    }
}
