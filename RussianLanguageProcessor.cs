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
    public class RussianLanguageProcessor : LanguageProcessor
    {
        public static string vvv = "правило";

        public static string[] LANGUAGES = new string[] { "Русский" };
        public static string[] grammarKeywords = new string[]
        {
            "Существительное",
            "Прилагательное",
            "Глагол",
        };

        public RussianLanguageProcessor(int count) : base(count)
        {
            this.version = "ru";
            this.language = "Russian";

            //ParseBadLemmas();
        }


        public void SaveToAssets()
        {
            string dir = "C:\\Users\\Einhorn\\AndroidStudioProjects\\UniLang\\app\\src\\main\\assets";
            string[] existingFiles = Directory.GetFiles(dir);
        }

        static string ShowStress(RussianDerivedWord rdw)
        {
            return ShowStress(rdw.spelling, rdw.stress);
        }

        static string ShowStress(string s, int stress)
        {
            if (stress >= s.Length)
            {
                Console.WriteLine(s + " " + stress);
                return null;
            }

            if (stress == -1)
            {
                return s;
            }
            else
            {
                string ww = s;
                return ww.Substring(0, stress + 1) + (char)(769) + ww.Substring(stress + 1);
            }
        }

        public override void ExportDictionary()
        {
            string[] lns = File.ReadAllLines("E:\\unilang_server\\data\\dictionaries\\_ru.txt");

            HashSet<string> st = new HashSet<string>();
            for (int i = 0; i < lns.Length; i++)
            {
                string[] spl = lns[i].Split(';');
                st.Add(spl[0]);
            }

            Language rl = new Language(language);

            List<string> allStrings = new List<string>();
            allStrings.AddRange(lns);

            for (int i = 0; i < rl.words.Count; i++)
            {
                RussianWord rw = rl.words[i];

                if (rw.spelling != null && rw.nativeWord)
                {
                    string word = rw.yospelling == null ? rw.spelling : rw.yospelling;

                    if (rw.forms != null)
                    {
                        for (int q = 0; q < rw.forms.Count; q++)
                        {
                            string form = rw.forms[q].spelling;
                            string formView = ShowStress(rw.forms[q].spelling, rw.forms[q].stress);

                            if (!st.Contains(form)) {
                                allStrings.Add(form + ";" + formView + ";a form;" + word);
                            }

                            if (form.Contains("ё"))
                            {
                                string nf = form.Replace("ё", "е");
                                if (!st.Contains(nf)) {
                                    allStrings.Add(nf + ";" + formView + ";a form;" + word);
                                }
                            }
                        }
                    }
                }
            }

            File.WriteAllLines("E:\\unilang_server\\data\\dictionaries\\_ru.txt", allStrings);

            /*
            StringBuilder res = new StringBuilder();
            StringBuilder res3 = new StringBuilder();


            for (int i = 0; i < rl.words.Count; i++)
            {
                RussianWord rw = rl.words[i];

                if (rw.spelling != null && rw.nativeWord)
                {
                    string word = rw.yospelling == null ? rw.spelling : rw.yospelling;
                    string wordView = ShowStress(rw.yospelling == null ? rw.spelling : rw.yospelling, rw.stressIndex);
                    res3.Append(word + "\n");
                    res.Append(word + ";" + wordView + ";" + "normal;" + rw.spelling + ";" + wordView + ";" + rw.frequencyRank + "\n");

                    if (word.Contains("ё"))
                    {
                        res.Append(word.Replace('ё', 'е') + ";" + wordView + ";" + "normal;" + rw.spelling + ";" + wordView + ";" + rw.frequencyRank + "\n");
                    }

                    if (rw.forms != null)
                    {
                        for (int q = 0; q < rw.forms.Count; q++)
                        {
                            string form = rw.forms[q].spelling;
                            string formView = ShowStress(rw.forms[q].spelling, rw.forms[q].stress);

                            if (formView != null)
                            {
                                res.Append(form + ";" + formView + ";" + "form;" + rw.spelling + ";" + wordView + ";" + rw.frequencyRank + "\n");

                                if (form.Contains("ё"))
                                {
                                    res.Append(form.Replace('ё', 'е') + ";" + formView + ";" + "normal;" + rw.spelling + ";" + wordView + ";" + rw.frequencyRank + "\n");
                                }
                            }
                            else
                            {
                                Console.WriteLine("From " + rw.spelling);
                            }
                        }
                    }
                }
            }

            string[] lines = res.ToString().Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            //string dir = GetDir() + "html//dictionaries//"+dictionary.txt";
            string dir2 = "E:\\projects\\Lang\\Lang\\bin\\Debug\\data\\lang\\Russian\\html\\dictionaries\\" + this.version + ".txt";
            //string dir3 = GetDir() + "html//dictionaries//" + searchlist.txt";


            string dir = "E:\\unilang_server\\data\\dictionaries\\" + this.version + ".txt";

            //string dir2 = "E:\\projects\\Lang\\Lang\\bin\\Debug\\data\\lang\\Russian\\html\\dictionaries\\" + this.version + ".txt";


            List<Tuple<string, Tuple<string, string>>> simplified = new List<Tuple<string, Tuple<string, string>>>();

            Dictionary<string, HashSet<string>> al = new Dictionary<string, HashSet<string>>();
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] parts = line.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                string form = parts[0];
                string vis = parts[1];

                //vis = vis.Replace("ё", "е");

                form = form.Replace("ё" + (char)(769), "ё");
                parts[1] = parts[1].Replace("ё" + (char)(769), "ё");

                if (!al.ContainsKey(vis))
                {
                    al.Add(vis, new HashSet<string>());
                }

                if (!al[vis].Contains(parts[3]))
                {
                    simplified.Add(new Tuple<string, Tuple<string, string>>(form, new Tuple<string, string>(parts[1], parts[3])));

                    if (form.Contains("ё"))
                    {
                        simplified.Add(new Tuple<string, Tuple<string, string>>(form.Replace("ё", "е"), new Tuple<string, string>(parts[1], parts[3])));
                    }
                }
                al[vis].Add(parts[3]);

            }

            StringBuilder res2 = new StringBuilder();
            foreach (var q in simplified)
            {
                res2.Append(q.Item1 + ";" + q.Item2.Item1 + ";" + q.Item2.Item2);
                res2.Append("\n");
            }


            File.WriteAllText(dir, res.ToString());
            //File.WriteAllText(dir3, res3.ToString());
            */
        }

        public class State
        {
            public bool parsingMeanings = false;
            public bool parsingGrammar = false;
            public bool parsingSynonyms = false;
            public bool observed = false;

            public bool isEnded = false;
            public bool isStarted = true;
            public string mainForm;
        }


        public void MakeParticiples()
        {
            string file = "russian_participles.txt";
            Language lang = new Language(this.language);

            List<string> res = new List<string>();

            HashSet<string> words = new HashSet<string>();
            for (int i = 0; i < lang.words.Count; i++)
            {
                words.Add(lang.words[i].spelling);
                words.Add(lang.words[i].yospelling);
            }

            for (int i = 0; i < lang.words.Count; i++)
            {
                RussianWord rd = lang.words[i];
                //if (rd.verbGrammar != null)
                {
                    int rank = lang.words[i].frequencyRank;
                    for (int j = 0; j < lang.words[i].forms.Count; j++)
                    {
                        string form = lang.words[i].forms[j].spelling;

                        bool isParticiple = false;

                        if (form.EndsWith("ший") || form.EndsWith("щий"))
                        {
                            isParticiple = true;
                        }

                        if (form.EndsWith("мый") || form.EndsWith("ный") || form.EndsWith("тый"))
                        {
                            isParticiple = true;
                        }

                        if (form.EndsWith("шийся") || form.EndsWith("щийся"))
                        {
                            isParticiple = true;
                        }

                        if (isParticiple)
                        {
                            if (!words.Contains(form))
                            {
                                res.Add(rank + "\t" + form);
                                words.Add(form);
                            }
                        }
                    }
                }
            }
            File.WriteAllLines(file, res.ToArray());
        }




        public void MakeForgotten()
        {
            string file = "forgotten.txt";
            Language lang = new Language(this.language);

            List<string> res = new List<string>();

            HashSet<string> words = new HashSet<string>();
            for (int i = 0; i < lang.words.Count; i++)
            {
                words.Add(lang.words[i].spelling);
                words.Add(lang.words[i].yospelling);

                for (int j = 0; j < lang.words[i].forms.Count; j++)
                {
                    words.Add(lang.words[i].forms[j].spelling);
                }
            }


            WordList brute = WordlistConstructor.CreateFromTxt(this.language + ".txt", ' ', 0, 0, 1000000000, true);


            for (int i = 0; i < brute.dictionary.Count; i++)
            {
                //RussianWord rd = lang.words[i];
                if (!words.Contains(brute.dictionary[i].word))
                {
                    res.Add(brute.dictionary[i].frequencyRank + "\t" + brute.dictionary[i].word);
                }
            }
            File.WriteAllLines(file, res.ToArray());
        }


        public override void MakeJson()
        {
            WordList russian = WordlistConstructor.CreateFromTxt("lemma.num", ' ', 2, 0, 100000, true);

            /*
            //WordList russianEnglish = WordlistConstructor.CreateFromTxt("english.txt", ' ', 2, 0, 100000, false);
            WordList participles = WordlistConstructor.CreateFromTxt2("russian_participles.txt", '\t', 1, 0, 0, 100000, true);
            WordList forgotten = WordlistConstructor.CreateFromTxt2("forgotten.txt", '\t', 1, 0, 0, 100, true);

            WordList common = new WordList();
            //common.dictionary.AddRange(participles.dictionary);

            BasicWord bbbb = new BasicWord();
            bbbb.mainForm = vvv;
            bbbb.word = vvv;
            bbbb.native = true;

            common.dictionary.Add(bbbb);
            common.dictionary.AddRange(russian.dictionary);
            //common.dictionary.AddRange(russianEnglish.dictionary);
            common.dictionary.AddRange(forgotten.dictionary);

            string rusLang = JsonConvert.SerializeObject(common, Formatting.Indented);
            File.WriteAllText("data/lang/" + language + ".json", rusLang, Encoding.UTF8);
            */

            WordList participles = WordlistConstructor.CreateFromTxt2("russian_participles.txt", '\t', 1, 0, 0, 100000, true);
            russian.dictionary.AddRange(participles.dictionary);

            WordList lemmas = WordlistConstructor.CreateFromTxt("Russian_lemmas.txt", '|', 0, 0, 100000, true);


            Dictionary<string, int> set = new Dictionary<string, int>();
            for (int i = 0; i < russian.dictionary.Count; i++)
            {
                if (!set.ContainsKey(russian.dictionary[i].word))
                {
                    set.Add(russian.dictionary[i].word, russian.dictionary[i].frequencyRank);
                }
            }

            WordList wl2 = new WordList();

            WordList common = new WordList();

            for (int i = 0; i < lemmas.dictionary.Count; i++)
            {
                if (set.ContainsKey(lemmas.dictionary[i].word))
                {
                    wl2.dictionary.Add(lemmas.dictionary[i]);
                    lemmas.dictionary[i].frequencyRank = set[lemmas.dictionary[i].word];
                }
                else
                {
                    //if (lemmas.dictionary[i].mainForm.Equals("боль"))
                    {
                        //Console.WriteLine("Extra " + lemmas.dictionary[i].mainForm);
                    }
                    wl2.dictionary.Add(lemmas.dictionary[i]);
                    lemmas.dictionary[i].frequencyRank = -1;
                }
            }

            wl2.dictionary.Sort(
                delegate (BasicWord x, BasicWord y)
                {
                    int v1 = x.frequencyRank;
                    int v2 = y.frequencyRank;

                    if (v1 == -1)
                    {
                        v1 = 100000000;
                    }

                    if (v2 == -1)
                    {
                        v2 = 100000000;
                    }

                    return v1.CompareTo(v2);
                }
            );


            string rusLang = JsonConvert.SerializeObject(wl2, Formatting.Indented);
            File.WriteAllText("data/lang/" + language + ".json", rusLang, Encoding.UTF8);

        }

        public static char[] vowels = new char[] { 'а', 'е', 'ё', 'о', 'у', 'ю', 'я', 'и', 'ы', 'э' };

        public override Word Merge(Word englishWord, Word nationalWord)
        {
            if (nationalWord == null)
            {
                return englishWord;
            }

            RussianWord russian = (RussianWord)nationalWord;

            RussianWord res = new RussianWord();

            if (englishWord != null)
            {
                List<HTMLExtractor.Chunk> chunks = new List<HTMLExtractor.Chunk>();
                chunks.AddRange(englishWord.chunks);
                if (russian != null && russian.chunks != null)
                {
                    chunks.AddRange(russian.chunks);
                }
                //res.externalMeanings = englishWord.meanings;
                //res.etymologies = englishWord.etymologies;
                res.spelling = nationalWord.spelling;
                res.yospelling = russian.yospelling;
                res.frequencyRank = nationalWord.frequencyRank;

                if (russian != null && russian.forms != null)
                {
                    res.forms = russian.forms;
                }
                res.chunks = chunks;
                res.stressIndex = russian.stressIndex;

                if (res.spelling != null)
                {
                    for (int i = 0; i < res.spelling.Length; i++)
                    {
                        if (res.spelling[i] >= 'a' && res.spelling[i] <= 'z')
                        {
                            res.stressIndex = -1;
                        }
                    }
                }

                if (res.yospelling == null)
                {
                    res.yospelling = res.spelling;
                }
            }

            if (englishWord == null && nationalWord.chunks.Count == 0)
            {
                return null;
            }

            if (englishWord == null && nationalWord != null)
            {
                return nationalWord;
            }


            if (res.yospelling == null)
            {
                return null;
            }


            if (res != null)
            {
                if (res.frequencyRank < 1000 && false)
                {
                    res.engForms = new List<RussianDerivedWord>();
                }
                else
                {
                    res.engForms = HTMLExtractor.GetFormsRussian(res.chunks);
                }

                for (int j = 0; j < res.engForms.Count; j++)
                {
                    SingleVowel(res.engForms[j]);
                }

                for (int j = 0; j < res.forms.Count; j++)
                {
                    SingleVowel(res.forms[j]);
                }


                for (int i = 0; i < res.engForms.Count; i++)
                {
                    RussianDerivedWord dw = res.engForms[i];
                    bool exists = false;

                    for (int j = 0; j < res.forms.Count; j++)
                    {
                        if (dw.spelling.Equals(res.forms[j].spelling) && dw.stress == res.forms[j].stress)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        res.forms.Add(dw);
                    }
                }
            }


            return res;
        }

        public static void SingleVowel(RussianWord res)
        {
            int vc = 0;
            for (int i = 0; i < res.spelling.Length; i++)
            {
                if (vowels.Contains(res.spelling[i]))
                {
                    vc++;
                }
            }

            if (vc < 2)
            {
                if (res.stressIndex != -1)
                {
                    res.stressIndex = -1;
                }
            }
        }

        public static void SingleVowel(RussianDerivedWord res)
        {
            int vc = 0;
            for (int i = 0; i < res.spelling.Length; i++)
            {
                if (vowels.Contains(res.spelling[i]))
                {
                    vc++;
                }
            }

            if (vc < 2)
            {
                if (res.stress != -1)
                {
                    res.stress = -1;
                }
            }
        }

        public override Word ParseNational(BasicWord word)
        {
            string version = "ru";
            string wordForm = word.word;

            string link = "https://ru.wiktionary.org/wiki/" + word.word;
            string text = WiktionaryCollector.Collect(link);

            if (text == null)
            {
                return null;
            }

            /*
            string file = "data/" + version + "_wiktionary/" + wordForm + ".html";
            if (!File.Exists(file))
            {
                return new RussianWord();
            }
            string text = File.ReadAllText(file);
            */

            var doc = new HtmlDocument();
            doc.LoadHtml(text);

            var el = doc.GetElementbyId("mw-page-base");
            //Console.WriteLine(doc);

            State state = new State();
            state.mainForm = word.word;
            RussianWord res = new RussianWord();

            if (word.native)
            {
                res.chunks = HTMLExtractor.ExtractBlock(text, "Русский", "<h1>", "ru");
            }
            else
            {
                res.chunks = HTMLExtractor.ExtractBlock(text, "Английский", "<h1>", "ru");
            }


            res.frequencyRank = word.frequencyRank;
            HtmlNodeCollection nodes = doc.DocumentNode.ChildNodes;
            foreach (HtmlNode node in nodes)
            {
                ExploreRussian(node, 0, state, res);
            }


            if (res.stressIndex == -1)
            {
                for (int i = 0; i < res.forms.Count; i++)
                {
                    if (res.forms[i].spelling.Equals(res.spelling) && res.forms[i].stress != -1)
                    {
                        if (res.stressIndex == -1)
                        {
                            res.stressIndex = res.forms[i].stress;
                            Console.WriteLine("New stress " + res.spelling + " " + res.spelling[res.stressIndex]);
                        }
                        else
                        {
                            if (res.forms[i].stress != res.stressIndex)
                            {
                                res.stressIndex = -1;
                                Console.WriteLine("Ambiguious " + res.spelling);
                                break;
                            }
                        }
                    }
                }
            }


            SingleVowel(res);

            return res;
        }


        public static void ExploreRussian(HtmlNode node, int level, State state, RussianWord result)
        {
            RussianWord lastWord = result;
            if (lastWord != null)
            {
                lastWord.spelling = state.mainForm;

                if (lastWord.yospelling == null)
                {
                    lastWord.yospelling = state.mainForm;
                }
            }

            if (state.isEnded)
            {
                return;
            }



            if (node.Id.Equals("Абазинский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Абхазский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Аварский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Агульский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Адыгейский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Азербайджанский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Алеутский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Алеутский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Алюторский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Андийский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Арчинский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Ахвахский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Башкирский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Бежтинский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Башкирский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Белорусский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Болгарский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Бурятский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Водский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Гагаузский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Горномарийский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Даргинский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Долганский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Древнерусский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Древнетюркский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Древнетюркский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Дунганский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Ингушский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Ительменский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Кабардино-черкесский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Казахский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Калмыцкий"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Караимский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Каракалпакский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Каратинский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Карачаево-балкарский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Кетский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Киргизский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Корякский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Крымскотатарский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Крымчакский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Кумыкский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Лакский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Лезгинский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Македонский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Мансийский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Марийский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Мокшанский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Молдавский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Монгольский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Нанайский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Нганасанский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Негидальский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Ненецкий"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Нивхский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Ногайский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Орокский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Орочский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Осетинский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Персидский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Русинский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Рутульский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Саамский (кильдинский)"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Северноюкагирский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Северо-западный марийский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Селькупский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Сербохорватский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Сербский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Сибирскотатарский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Сиреникский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Старославянский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Табасаранский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Таджикский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Татарский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Татский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Тофаларский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Тувинский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Туркменский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Тувинский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Убыхский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Удинский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Удмуртский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Удэгейский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Узбекский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Уйгурский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Украинский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Урумский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Хакасский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Хиналугский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Цахурский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Цезский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Церковнославянский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Цыганский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Чеченский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Чеченский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Чеченский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Чувашский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Чукотский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Шорский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Эвенкийский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Эрзянский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Южноюкагирский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Ягнобский"))
            {
                state.isEnded = true;
            }
            if (node.Id.Equals("Якутский"))
            {
                state.isEnded = true;
            }







            if (node.Id.Equals("Кириллица"))
            {
                state.isEnded = false;
            }


            if (node.Id.Equals("Татарский"))
            {
                state.isEnded = false;
            }

            if (node.Id.Equals("Русский"))
            {
                state.isStarted = true;
            }




            if (node.Id.Equals("Древнерусский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Старославянский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Мокшанский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Коми-зырянский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Коми-пермяцкий"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Ненецкий"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Удмуртский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Удмуртский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Белорусский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Болгарский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Украинский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Монгольский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Сербский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Ингушский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Коми-пермяцкий"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Македонский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Кумыкский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Башкирский"))
            {
                state.isEnded = true;
            }



            if (node.Id.Equals("Древнеанглийский"))
            {
                state.isEnded = true;
            }



            if (node.Id.Equals("Норвежский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Шведский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Эстонский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Вьетнамский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Датский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Польский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Сербский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Словенский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Хорватский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Чешский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Эсперанто"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Нидерландский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Латиница"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Айнский"))
            {
                state.isEnded = true;
            }




            if (node.Id.Equals("Интерлингва"))
            {
                state.isEnded = true;
            }



            if (node.Id.Equals("Испанский"))
            {
                state.isEnded = true;
            }



            if (node.Id.Equals("Итальянский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Курдский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Казахский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Казахский"))
            {
                state.isEnded = true;
            }

            if (node.Id.Equals("Южноминьский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Немецкий"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Румынский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Словацкий"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Польский"))
            {
                state.isEnded = true;
            }


            if (node.Id.Equals("Французский"))
            {
                state.isEnded = true;
            }

            if (state.isStarted)
            {
                if (node.Id.Equals("Синонимы"))
                {
                    state.parsingMeanings = false;
                    //state.parsingSynonyms = true;
                }

                if (node.Id.Equals("Антонимы"))
                {
                    state.parsingSynonyms = false;
                    state.parsingMeanings = false;
                }

                if (node.Id.Equals("Произношение") || node.Id.Equals("Произношение_1") || node.Id.Equals("Произношение_2") || node.Id.Equals("Произношение_3"))
                {
                    state.parsingGrammar = false;
                }

                if (state.parsingGrammar)
                {
                    string txt = node.InnerText.Replace("\n", "");


                    string[] decList = new string[] { "падеж", "форма", "&#160;—&#32;", "&#160;", "&#160;&#160;&#160;" };

                    if (level == 4)
                    {

                        if (txt.Length > 0)
                        {

                            bool indcl = false;

                            for (int q = 0; q < decList.Length; q++)
                            {
                                if (txt.Equals(decList[q]))
                                {
                                    indcl = true;
                                }
                            }

                            if (!(txt[0] >= 'А' && txt[0] <= 'Я'))
                            {
                                //Console.WriteLine(txt);


                                if (txt.IndexOf(".") == -1 && !indcl)
                                {
                                    string[] slp = txt.Split(' ');
                                    if (slp.Length == 2 && slp[0].Equals("в"))
                                    {
                                        txt = slp[1];
                                    }

                                    RussianDerivedWord rw = RussianWord.ParseStress(txt);

                                    if (rw != null)
                                    {


                                        if (rw.spelling.Length > 1 && !rw.spelling.Contains(" "))
                                        {
                                            if (txt.Equals("will"))
                                            {
                                                //Console.WriteLine(txt);
                                            }

                                            lastWord.forms.Add(rw);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (level == 0)
                    {

                        //Console.WriteLine(txt);

                        if (txt.Contains("падеж"))
                        {
                            if (!txt.Contains("ср."))
                            {
                            }
                            else
                            {
                            }
                        }

                        if (txt.Contains("наст.прош.повелит") || txt.Contains("будущ.прош.повелит"))
                        {
                        }

                        bool containGrammarWord = false;
                        for (int i = 0; i < grammarKeywords.Length; i++)
                        {
                            if (txt.Contains(grammarKeywords[i]))
                            {
                                containGrammarWord = true;
                            }
                        }

                        if (containGrammarWord)
                        {
                        }


                        string smallText = txt.Replace("?", "").Replace("-", "").Replace("·", "").Replace("&#160", "");
                        if (smallText.Contains("("))
                        {
                            if (smallText.IndexOf('(') != 0)
                            {
                                smallText = smallText.Substring(0, smallText.IndexOf('(') - 1);
                            }
                        }

                        if (smallText.Contains((char)(769)) || (smallText.Contains("ё")))
                        {
                            int sindex = -1;
                            char v = 'a';
                            if (smallText.Length == state.mainForm.Length + 1)
                            {
                                for (int i = 0; i < smallText.Length; i++)
                                {
                                    string newS = "";
                                    for (int k = 0; k < smallText.Length; k++)
                                    {
                                        if (k != i)
                                        {
                                            newS += smallText[k];
                                        }
                                    }
                                    if (newS.Equals(state.mainForm))
                                    {
                                        sindex = i;
                                    }
                                }
                            }


                            int ee = 0;
                            if (smallText.Length == state.mainForm.Length)
                            {
                                for (int i = 0; i < smallText.Length; i++)
                                {
                                    if (smallText[i] == 'ё' && state.mainForm[i] == 'е')
                                    {
                                        ee++;
                                    }
                                    else
                                    {
                                        if (smallText[i] != state.mainForm[i])
                                        {
                                            ee += 1000;
                                        }
                                    }
                                }
                            }

                            if (ee == 1)
                            {
                                lastWord.yospelling = smallText;
                            }

                            if (sindex != -1)
                            {
                                v = smallText[sindex];

                                if (v >= 'а' && v <= 'я')
                                {

                                }
                                else
                                {
                                    //if (lastWord.stressIndex == -2) {

                                    bool er = false;
                                    if (!vowels.Contains(lastWord.spelling[sindex - 1]))
                                    {
                                        //throw new Exception();
                                        //Console.WriteLine("ERROR");
                                        er = true;
                                    }
                                    else
                                    {
                                        //Console.WriteLine("\t\t\t" + lastWord.spelling + " " + lastWord.spelling[sindex - 1]);
                                    }

                                    if (!er)
                                    {
                                        bool over = false;
                                        if (lastWord.stressIndex != -1)
                                        {
                                            if (sindex - 1 != lastWord.stressIndex)
                                            {
                                                over = true;
                                            }
                                        }

                                        if (!over)
                                        {
                                            lastWord.stressIndex = sindex - 1;
                                        }
                                        else
                                        {
                                            lastWord.stressIndex = -1;
                                            //Console.WriteLine("OVERRIDE "+ lastWord.spelling);
                                        }
                                    }
                                    //}
                                }
                            }
                        }

                    }
                }



                if (node.Id.Equals("Значение") || node.Id.Equals("Значение_1") || node.Id.Equals("Значение_2") || node.Id.Equals("Значение_3") || node.Id.Equals("Значение_4"))
                {
                    if (result == null)
                    {
                        return;
                    }

                    state.parsingGrammar = false;
                    state.parsingMeanings = true;
                }


                if (node.Id.Equals("Морфологические_и_синтаксические_свойства") || node.Id.Equals("Морфологические_и_синтаксические_свойства_2") || node.Id.Equals("Морфологические_и_синтаксические_свойства_3") || node.Id.Equals("Морфологические_и_синтаксические_свойства_4"))
                {
                    if (state.observed)
                    {
                        //state.isEnded = true;
                        //state.observed = true;
                    }

                    state.parsingGrammar = true;
                    //state.observed = true;


                    //if (state.observed)

                    //result = new RussianWord();
                    //}
                }
            }

            bool levelUp = state.parsingGrammar;
            bool show = state.parsingMeanings;
            if (false)
            {
                for (int i = 0; i < level; i++)
                {
                    Console.Write("\t");
                }

                string text = node.Id + " -> " + node.InnerText.Replace("\n", "");
                string info = text;
                Console.WriteLine(info);
            }


            if (node.ChildNodes.Count > 0)
            {
                foreach (HtmlNode child in node.ChildNodes)
                {
                    ExploreRussian(child, levelUp ? level + 1 : level, state, result);
                }
            }
        }
    }
}


/*
— Значит, нам придётся всё разузнать самим, — заключил Рон, и они, расставшись с явно раздосадованным Хагридом, поспешили в библиотеку. С того самого дня, как Хагрид упомянул имя Фламеля, ребята действительно пересмотрели кучу книг в его поисках. А как ещё они могли узнать, что пытался украсть Снегг?Проблема заключалась в том, что они не представляли, с чего начать, и не знали, чем прославился Фламель, чтобы попасть в книгу. В «Великих волшебниках двадцатого века» он не упоминался, в «Выдающихся именах нашей эпохи» — тоже, равно как и в «Важных магических открытиях последнего времени» и «Новых направлениях магических наук». Ещё одной проблемой были сами размеры библиотеки — тысячи полок вытянулись в сотни рядов, а на них стояли десятки тысяч томов. Гермиона вытащила из кармана список книг, которые она запланировала просмотреть, Рон пошёл вдоль рядов, время от времени останавливаясь, наугад вытаскивая ту или иную книгу и начиная её листать. А Гарри побрёл по направлению к Особой секции, раздумывая о том, нет ли там чего-нибудь о Николасе Фламеле. К сожалению, для того чтобы попасть в эту секцию, надо было иметь разрешение, подписанное кем-либо из преподавателей. Гарри знал, что никто ему такого разрешения не даст. А придумать что-нибудь очень убедительное ему бы вряд ли удалось. К тому же книги, хранившиеся в этой секции, предназначались вовсе не для первокурсников. Гарри уже знал, что эти книги посвящены высшим разделам Тёмной магии, которые не изучали в школе. Так что доступ к ним был открыт только преподавателям и ещё старшекурсникам, выбравшим в качестве специализации защиту от Тёмных сил. <br>
 * */
