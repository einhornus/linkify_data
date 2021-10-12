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
    public abstract class LanguageProcessor
    {
        public string version;
        public string language;
        public int count;


        public static string Simp2Trad(string str)
        {
            return Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
        }

        public static string Trad2Simp(string str)
        {
            return Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0);
        }

        public Dictionary<string, Dictionary<string, int>> badLemmas = new Dictionary<string, Dictionary<string, int>>();

        /*
        public void ParseBadLemmas()
        {
            string file = "E:\\tests//data//useless_" + this.version + ".txt";
            string[] lines = File.ReadAllLines(file);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split('|');
                string w = parts[0];
                string pos = parts[1];

                if (!badLemmas.ContainsKey(w))
                {
                    badLemmas.Add(w, new Dictionary<string, int>());
                }

                if (!badLemmas[w].ContainsKey(pos))
                {
                    badLemmas[w].Add(pos, 0);
                }

                badLemmas[w][pos]++;
            }

            Console.WriteLine();
        }
        */


        public LanguageProcessor(int count)
        {
            this.count = count;
        }

        public abstract void ExportDictionary();

        public abstract void MakeJson();


        public static List<string> ProcessTranslationFile(List<LanguageProcessor> list, string text, int index, string word)
        {
            List<string> items = new List<string>();

            var v = HTMLExtractor.ExtractBlock(text, "English", "<h2>", "en");

            var sel = new List<Tuple<HTMLExtractor.Chunk, HTMLExtractor.TagType>>();

            HTMLExtractor.TagType last = HTMLExtractor.TagType.Unknown;

            foreach (var q in v)
            {
                if (q.type == HTMLExtractor.TagType.Verb ||
                    q.type == HTMLExtractor.TagType.Noun ||
                    q.type == HTMLExtractor.TagType.Adjective ||
                    q.type == HTMLExtractor.TagType.Article ||
                    q.type == HTMLExtractor.TagType.Contraction ||
                    q.type == HTMLExtractor.TagType.Determiner ||
                    q.type == HTMLExtractor.TagType.Interjection ||
                    q.type == HTMLExtractor.TagType.Letter ||
                    q.type == HTMLExtractor.TagType.Numeral ||
                    q.type == HTMLExtractor.TagType.Participle ||
                    q.type == HTMLExtractor.TagType.Particle ||
                    q.type == HTMLExtractor.TagType.Predicative ||
                    q.type == HTMLExtractor.TagType.Preposition ||
                     q.type == HTMLExtractor.TagType.Pronoun ||
                     q.type == HTMLExtractor.TagType.ProperName ||
                     q.type == HTMLExtractor.TagType.Adverb ||
                     q.type == HTMLExtractor.TagType.Conjunction
                    )
                {
                    last = q.type;
                }

                if (q.type == HTMLExtractor.TagType.Translations)
                {
                    sel.Add(new Tuple<HTMLExtractor.Chunk, HTMLExtractor.TagType>(q, last));
                    last = HTMLExtractor.TagType.Unknown;
                }
            }


            for (int qq = 0; qq < sel.Count; qq++)
            {
                HTMLExtractor.Chunk q = sel[qq].Item1;
                File.WriteAllText("inner" + qq + ".html", q.content);

                string news = q.content;

                string s1 = "<div class=\"NavHead\" style=\"text-align:left;\">";
                string s2 = "<div class=\"NavHead\" style=\"text-align: left;\">";


                string[] points = q.content.Split(new string[] { s1, s2 }, StringSplitOptions.None);



                for (int ee = 1; ee < points.Length; ee++)
                {
                    string point = points[ee];
                    string englishPoint = point.Substring(0, point.IndexOf("</div>"));

                    if (!englishPoint.Equals("Translations to be checked"))
                    {
                        if (point.Contains("<ul>"))
                        {
                            string[] members = point.Split(new string[] { "<li>", "</li>" }, StringSplitOptions.None);

                            string p1 = null;
                            string p2 = null;

                            List<string> lst = new List<string>();
                            for (int i = 0; i < list.Count; i++)
                            {
                                lst.Add("");
                            }

                            lst[0] = word;

                            for (int e = 0; e < members.Length; e++)
                            {
                                string _m1 = members[e];
                                string _m2 = members[e];

                                if (ee == 9)
                                {
                                    //Console.WriteLine();
                                }

                                int curlang = -1;
                                for (int qqq = 0; qqq < list.Count; qqq++)
                                {
                                    if (_m1.IndexOf(list[qqq].language) == 0)
                                    {
                                        curlang = qqq;
                                        break;
                                    }
                                }

                                if (curlang != -1)
                                {
                                    List<string> fms = HTMLExtractor.GetForms(_m1, list[curlang].version);
                                    p1 = String.Join(", ", fms.ToArray());
                                    string mmm = _m1;
                                    mmm = mmm.Replace("(" + list[curlang].version + ")", "");
                                    mmm = mmm.Replace(list[curlang].language + ":", "");
                                    if (mmm.Contains("\n"))
                                    {
                                        mmm = mmm.Substring(0, mmm.IndexOf("\n"));
                                    }

                                    /*
                                    int bracketLevel = 0;
                                    string newmmmm = "";
                                    for (int i = 0; i<mmm.Length; i++)
                                    {
                                        if (mmm[i] == '(')
                                        {
                                            bracketLevel++;
                                        }

                                        if (mmm[i] == ')')
                                        {
                                            bracketLevel--;
                                        }

                                        if (bracketLevel == 0)
                                        {
                                            newmmmm += mmm[i];
                                        }
                                    }
                                    */

                                    mmm = mmm.Replace("  ", " ");

                                    lst[curlang] = mmm + "^^^" + p1;
                                }
                            }


                            string item = word + "|" + englishPoint + "|" + sel[qq].Item2.ToString() + "|" + index + "|";
                            for (int i = 0; i < lst.Count; i++)
                            {
                                item += lst[i].ToString();
                                if (i != lst.Count - 1)
                                {
                                    item += "|";
                                }
                            }
                            Console.WriteLine(item);
                            items.Add(item);
                        }
                    }
                }

            }
            return items;
        }

        public static string CapitalizeFirst(string wrd)
        {
            return (wrd[0] + "").ToUpper() + wrd.Substring(1);
        }

        public static void MakeDictionary(List<LanguageProcessor> languages)
        {
            WordList nat = WordlistConstructor.CreateFromTxt("English" + ".txt", ' ', 0, 0, 200000, true);
            string fileName = "E:\\tests\\data\\ultimate_dictionary.txt";
            List<string> allItems = new List<string>();

            string[] specialPages = new string[]
            {
                "water",
                "I",
                "language",
                "a",
                "air",
                "an",
                "and",
                "animal",
                "arm",
                "bar",
                "be",
                "bear",
                "bed",
                "black",
                "blood",
                "bone",
                "book",
                "brother",
                "bus",
                "cat",
                "China",
                "cloud",
                "coffee",
                "cow",
                "day",
                "die",
                "do",
                "dog",
                "drink",
                "ear",
                "egg",
                "eye",
                "face",
                "far",
                "finger",
                "fire",
                "fish",
                "friend",
                "god",
                "gold",
                "green",
                "hand",
                "he",
                "head",
                "heart",
                "here",
                "house",
                "ice",
                "in",
                "iron",
                "king",
                "lake",
                "lead",
                "leg",
                "life",
                "ligth",
                "long",
                "love",
                "man",
                "me",
                "mi",
                "milk",
                "moon",
                "mother",
                "new",
                "no",
                "nose",
                "o",
                "on",
                "one",
                "plant",
                "rain",
                "red",
                "river",
                "salt",
                "sand",
                "se",
                "sex",
                "si",
                "silver",
                "sin",
                "sky",
                "smoke",
                "son",
                "star",
                "sugar",
                "sun",
                "sweet",
                "tea",
                "ten",
                "time",
                "voice",
                "white",
                "wind",
                "woman",
                "you"
            };

            int[] indexes = new int[specialPages.Length];
            for (int i = 0; i < nat.dictionary.Count; i++)
            {
                for (int j = 0; j < specialPages.Length; j++)
                {
                    if (nat.dictionary[i].word.Equals(specialPages[j]))
                    {
                        indexes[j] = i;
                    }
                }
            }

            for (int i = 0; i < specialPages.Length; i++)
            {
                string link = "https://en.wiktionary.org/wiki/" + specialPages[i] + "/translations";
                string stuff = WiktionaryCollector.Collect(link);
                if (stuff != null)
                {
                    List<string> items = ProcessTranslationFile(languages, stuff, indexes[i], specialPages[i]);
                    allItems.AddRange(items);
                }
            }

            File.WriteAllLines(fileName, allItems.ToArray());



            for (int i = 0; i < Math.Min(10000000000, nat.dictionary.Count); i++)
            {
                //string file = "data/" + "en" + "_wiktionary/" + nat.dictionary[i].word + ".html";

                string link = "https://en.wiktionary.org/wiki/" + nat.dictionary[i].word;
                string stuff = WiktionaryCollector.Collect(link);


                if (stuff != null)
                {
                    string text = stuff;

                    List<string> items = ProcessTranslationFile(languages, text, i, nat.dictionary[i].word);


                    allItems.AddRange(items);
                }
            }
            File.WriteAllLines(fileName, allItems.ToArray());

        }

        public void ClearFUCK(string assets)
        {
            {
                string dataDir = assets;
                string[] files = Directory.GetFiles(dataDir);
                for (int i = 0; i < files.Length; i++)
                {
                    if (!files[i].Contains("jokes.txt"))
                    {
                        File.Delete(files[i]);
                    }
                }
            }
        }

        public void Clear(string assets)
        {
            {
                string dataDir = assets;
                string[] files = Directory.GetFiles(dataDir);
                for (int i = 0; i < files.Length; i++)
                {
                    if (!files[i].Contains("linkify.html") && !files[i].Contains(".txt"))
                    {
                        File.Delete(files[i]);
                    }
                }
            }
        }

        public void SplitDictionary(string folder)
        {
            string dict = folder + "dictionary.txt";
            string[] lines = File.ReadAllLines(dict);
            int count = 100;

            List<string>[] lists = new List<string>[count];
            for (int i = 0; i < count; i++)
            {
                lists[i] = new List<string>();
            }

            int size = lines.Length / count;
            for (int i = 0; i < lines.Length; i++)
            {
                int index = i / size;
                if (index >= count)
                {
                    index = count - 1;
                }
                lists[index].Add(lines[i]);
            }

            for (int i = 0; i < lists.Length; i++)
            {
                File.WriteAllLines(folder + "dictionary" + i + ".txt", lists[i]);
            }
        }

        public abstract Word Merge(Word englishWord, Word nationalWord);
        public abstract Word ParseNational(BasicWord bw);

        public string GetDir()
        {
            return "data//lang//" + language + "//";
        }

        public string GetDir2()
        {
            return "E://unilang_server//data//" + version + "//";
        }




        public void ParseWord(BasicWord bw)
        {
            Word merged = null;

            if (!this.version.Equals("en"))
            {
                Word englishWord = ParseEnglish(bw, "en");
                Word nationalWord = ParseNational(bw);

                if (englishWord != null)
                {
                    englishWord.nativeWord = bw.native;
                }
                if (nationalWord != null)
                {
                    nationalWord.nativeWord = bw.native;
                }


                merged = Merge(englishWord, nationalWord);
            }
            else
            {
                merged = ParseSimpleEnglish(bw);
                if (merged == null)
                {
                    merged = ParseEnglish(bw, "en");
                }
            }

            if (bw.word != null && merged != null)
            {
                merged.spelling = bw.word;
                merged.nativeWord = bw.native;

                string content = JsonConvert.SerializeObject(merged, Formatting.Indented);
                string dir = GetDir();
                //if (bw.word) {
                string path = dir + "//json//" + bw.word + ".json";
                //}
                File.WriteAllText(path, content);
            }
        }

        public void Clear()
        {
            {
                string dataDir = "data\\lang\\" + language + "\\json";
                string[] files = Directory.GetFiles(dataDir);
                for (int i = 0; i < files.Length; i++)
                {
                    if (!files[i].Contains("linkify.html") && !files[i].Contains(".txt") && !files[i].Contains("vocabulary_test.html"))
                    {
                        File.Delete(files[i]);
                    }
                }
            }
        }

        public static bool WordStartsWithUpper(string w)
        {
            return w[0] == w.ToUpper()[0];
        }

        public void ClearHTML()
        {
            {
                string dataDir = GetDir2() + "html//articles";
                string[] files = Directory.GetFiles(dataDir);
                for (int i = 0; i < files.Length; i++)
                {
                    if (!files[i].Contains("linkify.html") && !files[i].Contains(".txt") && !files[i].Contains("vocabulary_test.html"))
                    {
                        File.Delete(files[i]);
                    }
                }
            }
        }

        public static bool WordInUC(string w)
        {
            return (w[0] + "").ToUpper().Equals(w[0] + "");
        }

        public void ParseData()
        {
            MakeJson();
            //Clear();
            string json = File.ReadAllText("data/lang/" + language + ".json");
            WordList wList = JsonConvert.DeserializeObject<WordList>(json);

            for (int i = 0; i < Math.Min(this.count, wList.dictionary.Count); i++)
            {
                string dir = GetDir();

                string path = dir + "//json//" + wList.dictionary[i].word + ".json";
                //if (WordStartsWithUpper(wList.dictionary[i].word))
                {
                    //path = dir + "//json//!" + wList.dictionary[i].word + ".json";
                }

                if (!File.Exists(path))
                {
                    if (!wList.dictionary[i].word.Equals("con") && !wList.dictionary[i].word.Equals("aux") && !wList.dictionary[i].word.Contains("Con.") && !wList.dictionary[i].word.Contains("Con"))
                    {
                        Console.WriteLine(i + "/" + Math.Min(this.count, wList.dictionary.Count) + ": " + "Parsing " + wList.dictionary[i].word);
                        ParseWord(wList.dictionary[i]);
                    }
                }
                else
                {
                    Console.WriteLine(i + "/" + Math.Min(this.count, wList.dictionary.Count) + ": " + "exists already ");
                }
            }

            //AddExtra();
            //ExportDictionary();
            //ExportHTML(this.count);
        }

        public List<Tuple<string, string, string>> dt = null;

        public Tuple<string, int> Filter(string form)
        {
            if (version.Equals("nl"))
            {
                if (form.Equals("past participle"))
                {
                    return new Tuple<string, int>("past participle", 4);
                }

                if (form.Equals("second- and third-person singular present indicative"))
                {
                    return new Tuple<string, int>("jij, hij, zij, het", 2);
                }

                if (form.Equals("singular past indicative"))
                {
                    return new Tuple<string, int>("singular past", 3);
                }

                if (form.Equals("first-person singular present indicative"))
                {
                    return new Tuple<string, int>("ik", 0);
                }

                if (form.Equals("third-person singular present indicative"))
                {
                    return new Tuple<string, int>("hij, zij, het", 2);
                }

                if (form.Equals("second-person singular present indicative"))
                {
                    return new Tuple<string, int>("jij", 1);
                }

                if (form.Equals("plural past indicative and subjunctive"))
                {
                    return new Tuple<string, int>("plural past", 5);
                }

                if (form.Equals("first-, second- and third-person singular present indicative"))
                {
                    return new Tuple<string, int>("ik, jij, hij, zij, het", -1);
                }

                /*
                if (form.Equals("inflected form"))
                {
                    return new Tuple<string, int>("inflected", 0);
                }

                if (form.Equals("superlative form"))
                {
                    return new Tuple<string, int>("superlative", 1);
                }

                if (form.Equals("comparative form"))
                {
                    return new Tuple<string, int>("comparative", 2);
                }
                */

                /*
                if (form.Equals("diminutive"))
                {
                    return new Tuple<string, int>("diminutive", 1);
                }

                if (form.Equals("plural form"))
                {
                    return new Tuple<string, int>("plural", 0);
                }
                */
            }


            /*
            if (version.Equals("de"))
            {
                if (form.Equals("past participle"))
                {
                    return new Tuple<string, int>("past participle", 7);
                }

                if (form.Equals("first-person singular present"))
                {
                    return new Tuple<string, int>("ich", 0);
                }

                if (form.Equals("singular imperative"))
                {
                    return new Tuple<string, int>("singular imperative", 11);
                }

                if (form.Equals("plural imperative"))
                {
                    return new Tuple<string, int>("plural imperative", 12);
                }


                if (form.Equals("second-person singular present"))
                {
                    return new Tuple<string, int>("du", 1);
                }

                if (form.Equals("second-person singular preterite"))
                {
                    return new Tuple<string, int>("du (past)", 5);
                }

                if (form.Equals("second-person plural preterite"))
                {
                    return new Tuple<string, int>("ihr (past)", 6);
                }

                if (form.Equals("second-person plural present"))
                {
                    return new Tuple<string, int>("ihr", 2);
                }

                if (form.Equals("first/third-person singular preterite"))
                {
                    return new Tuple<string, int>("ich, er, sie, es (past)", 8);
                }

                if (form.Equals("first/third-person plural preterite"))
                {
                    return new Tuple<string, int>("wir, sie (past)", 9);
                }

                if (form.Equals("first/third-person plural present"))
                {
                    return new Tuple<string, int>("wir, sie", 4);
                }

                if (form.Equals("third-person singular present"))
                {
                    return new Tuple<string, int>("er, sie, es", 3);
                }









                if (form.Equals("dative plural"))
                {
                    return new Tuple<string, int>("dative plural", 8);
                }

                if (form.Equals("nominative plural"))
                {
                    return new Tuple<string, int>("nominative plural", 5);
                }

                if (form.Equals("accusative plural"))
                {
                    return new Tuple<string, int>("accusative plural", 6);
                }

                if (form.Equals("genitive plural"))
                {
                    return new Tuple<string, int>("genitive plural", 7);
                }

                if (form.Equals("dative singular"))
                {
                    return new Tuple<string, int>("dative singular", 4);
                }

                if (form.Equals("accusative singular"))
                {
                    return new Tuple<string, int>("accusative singular", 1);
                }

                if (form.Equals("genitive singular"))
                {
                    return new Tuple<string, int>("genitive singular", 2);
                }


                if (form.Equals("diminutive"))
                {
                    return new Tuple<string, int>("diminutive", 10);
                }

                if (form.Equals("plural"))
                {
                    return new Tuple<string, int>("plural", 3);
                }

                if (form.Equals("genitive"))
                {
                    return new Tuple<string, int>("genitive", 3);
                }
                

                //if (form.Contains("mixed") || form.Contains("strong") || form.Contains("weak"))
                {
                    //return new Tuple<string, int>(form, 3);
                }
            }
            */

            return null;
            //return null;
        }

        public string GenerateInflection(string word)
        {
            if (version.Equals("ru"))
            {
                return "";
            }

            if (dt == null)
            {
                string[] s = File.ReadAllLines("E:\\unilang_server\\data\\dictionaries\\" + this.version + ".txt");
                dt = new List<Tuple<string, string, string>>();
                for (int i = 0; i < s.Length; i++)
                {
                    string[] data = s[i].Split(';');
                    dt.Add(new Tuple<string, string, string>(data[1], data[2], data[3]));
                }
            }

            List<Tuple<string, string, int>> accepted = new List<Tuple<string, string, int>>();
            for (int i = 0; i < dt.Count; i++)
            {
                if (dt[i].Item3.Equals(word))
                {
                    Tuple<string, int> filtered = Filter(dt[i].Item2);
                    if (filtered != null)
                    {
                        accepted.Add(new Tuple<string, string, int>(filtered.Item1, dt[i].Item1, filtered.Item2));
                    }
                }
            }
            accepted.Sort((x, y) => x.Item3.CompareTo(y.Item3));

            string res = "<table class='center'>";
            for (int i = 0; i < accepted.Count; i++)
            {
                res += "<tr>";
                res += "<td>";
                res += accepted[i].Item1;
                res += "</td>";

                res += "<td>";
                res += accepted[i].Item2;
                res += "</td>";
                res += "</tr>";
            }
            res += "<table>";
            return res;
        }



        public void ExportOne(string word)
        {
            RussianWord rrr = new RussianWord();

            string[] files = Directory.GetFiles("data\\lang\\" + this.language + "\\json");
            string cont = File.ReadAllText("data\\lang\\" + this.language + "\\json\\" + word + ".json");
            JsonSerializerSettings settings = new JsonSerializerSettings();
            rrr = JsonConvert.DeserializeObject<RussianWord>(cont, settings);

            HTML html = new HTML(rrr, this);
            html.back += GenerateInflection(word);

            string file = GetDir2() + "html//articles//" + word + ".html";
            File.WriteAllText(file, html.back);


            Console.WriteLine(html.back);
        }

        public static Dictionary<string, List<string>> chineseHanzi = null;

        public static List<string> ToSimp(string trad)
        {
            if (chineseHanzi == null) {
                chineseHanzi = new Dictionary<string, List<string>>();
                string[] dictionary = File.ReadAllLines("cedict_ts.u8");

                for (int i = 0; i<dictionary.Length; i++)
                {
                    string[] spl = dictionary[i].Split(' ');
                    if (!chineseHanzi.ContainsKey(spl[0]))
                    {
                        chineseHanzi[spl[0]] = new List<string>();
                    }

                    bool exists = false;
                    for (int j = 0; j< chineseHanzi[spl[0]].Count; j++)
                    {
                        if (spl[1].Equals(chineseHanzi[spl[0]][j]))
                        {
                            exists = true;
                        }
                    }

                    if (!exists) {
                        chineseHanzi[spl[0]].Add(spl[1]);
                    }
                }
            }

            if (!chineseHanzi.ContainsKey(trad))
            {
                List<string> r = new List<string>();
                r.Add(trad);
                return r;
            }
            return chineseHanzi[trad];
        }

        public void ExportChineseSounds(string filter)
        {
            string[] dictionary = File.ReadAllLines("cedict_ts.u8");

            Language rl = new Language(this.language, filter);

            List<HTML> clist = new List<HTML>();
            LanguageProcessor proc = new GenericLanguageProcessor(200000, "Chinese", "zh");

            for (int i = 0; i < Math.Min(rl.words.Count, this.count); i++)
            {
                string old = rl.words[i].spelling;

                List<string> news = ToSimp(old);

                string new_ = Trad2Simp(old);
                if (news.Count == 1)
                {
                    new_ = news[0];
                }
                rl.words[i].spelling = new_;
                string audio = "";
                string wiktionaryContent = "";
                string origin = "";

                String path = "E:\\unilang_server\\data\\zh\\html\\articles\\" + rl.words[i].spelling + ".html";
                if (!File.Exists(path))
                {
                }
                else
                {
                    String content = File.ReadAllText(path);
                    Console.WriteLine(i);

                    if (!rl.words[i].spelling.Equals("linkify"))
                    {
                        for (int j = 0; j < rl.words[i].chunks.Count; j++)
                        {

                            if (rl.words[i].chunks[j].type == HTMLExtractor.TagType.Pronunciation)
                            {
                                string cont = rl.words[i].chunks[j].content;
                                string head = rl.words[i].chunks[j].header;

                                string closed = head[0] + "/" + head.Substring(1);
                                string preHeader = cont.Substring(0, cont.IndexOf(closed) + closed.Length);
                                string postHeader = cont.Substring(cont.IndexOf(closed) + closed.Length + 1);

                                string transcriptions = "";
                                bool open = false;
                                for (int q = 0; q < postHeader.Length; q++)
                                {
                                    if (postHeader[q] == '[')
                                    {
                                        open = true;
                                    }

                                    if (open)
                                    {
                                        transcriptions += postHeader[q];
                                    }

                                    if (postHeader[q] == ']')
                                    {
                                        open = false;
                                        transcriptions += " ";
                                    }
                                }

                                while (true)
                                {
                                    if (postHeader.Contains("<audio "))
                                    {
                                        string before = postHeader.Substring(0, postHeader.IndexOf("<audio "));
                                        string after = postHeader.Substring(postHeader.IndexOf("<audio "));
                                        string inner = after.Substring(0, after.IndexOf("</audio>"));
                                        string outer = after.Substring(after.IndexOf("</audio>") + "</audio>".Length);

                                        audio += inner;

                                        postHeader = outer;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                            }

                            bool good = false;
                            if (rl.words[i].chunks[j].content.Contains("Glyph origin"))
                            {
                                good = true;
                            }

                            List<HTMLExtractor.TagType> mainTypes = new HTMLExtractor.TagType[]
                    {
                        //HTMLExtractor.TagType.Etymology,
                        //HTMLExtractor.TagType.Pronunciation,

                        //HTMLExtractor.TagType.DerivedTerms,
                        //HTMLExtractor.TagType.RelatedTerms,
                        //HTMLExtractor.TagType.SeeAlso,
                        HTMLExtractor.TagType.UsageNotes,
                        HTMLExtractor.TagType.Noun,
                        HTMLExtractor.TagType.Adjective,
                        HTMLExtractor.TagType.Adverb,
                        HTMLExtractor.TagType.Interjection,
                        HTMLExtractor.TagType.Participle,
                        HTMLExtractor.TagType.Preposition,
                        HTMLExtractor.TagType.Pronoun,
                        HTMLExtractor.TagType.Verb,
                        HTMLExtractor.TagType.Participle,
                        HTMLExtractor.TagType.Particle,
                        HTMLExtractor.TagType.Conjunction,
                        HTMLExtractor.TagType.Determiner,
                        HTMLExtractor.TagType.Numeral,
                        HTMLExtractor.TagType.Article,
                        HTMLExtractor.TagType.Contraction,
                        HTMLExtractor.TagType.ProperName,
                        HTMLExtractor.TagType.Predicative,
                        HTMLExtractor.TagType.Letter,
                        HTMLExtractor.TagType.Phrase,
                        HTMLExtractor.TagType.Definitions,


                        HTMLExtractor.TagType.Lidwoord,
                        HTMLExtractor.TagType.Hoofdtelwoord,
                        HTMLExtractor.TagType.ZelfstandigNaamwoord,
                        HTMLExtractor.TagType.BijvoeglijkNaamwoord,
                        HTMLExtractor.TagType.Voorzetsel,
                        HTMLExtractor.TagType.AanwijzendVoornaamwoord,
                        HTMLExtractor.TagType.PersoonlijkVoornaamwoord,
                        HTMLExtractor.TagType.BezittelijkVoornaamwoord,
                        HTMLExtractor.TagType.Voegwoord,
                        HTMLExtractor.TagType.Werkwoord,
                        HTMLExtractor.TagType.Bijwoord,
                        HTMLExtractor.TagType.Rangtelwoord,
                    }.ToList();

                            if (mainTypes.Contains(rl.words[i].chunks[j].type))
                            {
                                good = true;
                            }

                            if (good)
                            {
                                //"</table></div></div>"
                                string boring = "</table>\n</div></div>";

                                string c = rl.words[i].chunks[j].content;
                                if (c.Contains("Glyph origin"))
                                {
                                    bool cont = c.Contains(boring);
                                    if (cont)
                                    {
                                        
                                        c = c.Substring(c.IndexOf(boring) + boring.Length);
                                        c = "<a></a>$START_SMALL$<a></a>" + c+ "<a></a>$END_SMALL$<a></a>";
                                    }

                                    origin = c;
                                }
                                else
                                {
                                    try
                                    {
                                        if (!HTML.JustAForm(c, proc, 0.25))
                                        {
                                            wiktionaryContent += ClearChinese(c);
                                            wiktionaryContent += "<hr>";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine();
                                    }
                                }

                                //wiktionaryContent += c;
                                //wiktionaryContent += "<hr>";
                            }
                        }
                    }

                    if (audio.Length > 0 || wiktionaryContent.Length > 0)
                    {
                        content = content.Replace("#SNDSND1#SNDSND2", "#SNDSND1" + audio + "#SNDSND2");
                        content = content.Replace("$$$ARTICLE_BODY$$$", wiktionaryContent+ "$$$ARTICLE_BODY$$$");

                        File.WriteAllText(path, content);
                    }
                }

            }

        }

        public static bool bannedExample(string x)
        {
            string[] ban = new string[] { "Cantonese", "Classical Chinese", "From:", "Jyutping", "Sino-Korean", "Shanghainese", "Hakka" };
            for (int i = 0; i<ban.Length; i++)
            {
                if (x.Contains(ban[i]))
                {
                    Console.WriteLine("Deleted "+x);
                    return true;
                }
            }
            return false;
        }


        public bool cstuff(string s)
        {
            bool res = false;

            int lev = 0;
            for (int i = 0; i<s.Length; i++)
            {
                if (s[i] == '<')
                {
                    lev++; 
                }

                if (s[i] == '>')
                {
                    lev--;
                }

                if (lev == 0 && s[i] != '<' && s[i] != '>' && s[i] != '\n' && s[i] != ' ')
                {
                    return true;
                }
            }
            return res;
        }

        public string ClearChinese(string html)
        {
            html = HTML.RemoveHTMLTag(html, "<li", "</li>", x => x.Contains("†") || x.Contains("‡") || x.Contains("A surname") || x.Contains("(historical)"));

            html = HTML.RemoveHTMLTag(html, "<dl class=\"zhusex\">", "</dl>", x => bannedExample(x));

            html = html.Replace("<span class=\"mw-editsection-bracket\">[</span>edit<span class=\"mw-editsection-bracket\">]</span>", "");
            html = HTML.RemoveHTMLTag(html, "<span lang=\"zh-Hant\" class=\"Hant\">", "</span>", x => !Trad2Simp(x).Equals(x));
            //html = HTML.RemoveHTMLTag(html, "<span class=\"Hani\"", "</span>", x => !Trad2Simp(x).Equals(x));
            html = html.Replace("style=\"color:#404D52\"", "");
            html = html.Replace(" / ", "");

            html = HTML.RemoveHTMLTag(html, "<span style=\"color:darkgreen;", "</span>", x => true);
            html = HTML.RemoveHTMLTag2(html, "<strong class=\"Hani headword\"", "</strong>", x => Trad2Simp(x));
            html = html.Replace("<ul><li></ul>", "");
            html = html.Replace("<ul><li>\n<li></ul>", "");
            html = html.Replace("<ul><li><li></ul>", "");
            //html = html.Replace("<li></ul></li>", "");

            html = html.Replace("&#", "$$1");

            html = html.Replace("#0", "");
            html = html.Replace("#1", "");
            html = html.Replace("#2", "");
            html = html.Replace("#3", "");
            html = html.Replace("#4", "");
            html = html.Replace("#5", "");
            html = html.Replace("#6", "");
            html = html.Replace("#7", "");
            html = html.Replace("#8", "");
            html = html.Replace("#9", "");
            html = html.Replace("#A", "");
            html = html.Replace("#B", "");
            html = html.Replace("#C", "");
            html = html.Replace("#D", "");
            html = html.Replace("#E", "");
            html = html.Replace("#F", "");
            html = html.Replace("#a", "");
            html = html.Replace("#b", "");
            html = html.Replace("#c", "");
            html = html.Replace("#d", "");
            html = html.Replace("#e", "");
            html = html.Replace("#f", "");

            html = html.Replace("$$1", "&#");


            html = html.Replace("<dl class=\"zhusex\"> <br />", "<dl class=\"zhusex\">");


            //html = HTML.RemoveHTMLTag(html, "<ul", "</ul>", x => !cstuff(x));

            /*
            for (int i = 0; i<100; i++)
            {
                if (html.Contains("color:#"))
                {
                    string bef = html.Substring(0, html.IndexOf("color:#"));
                    string after = html.Substring(html.IndexOf("color:#")+7+6);
                    html = bef + "color:#FFFFFF" + after;
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < 100; i++)
            {
                if (html.Contains("color: #"))
                {
                    string bef = html.Substring(0, html.IndexOf("color: #"));
                    string after = html.Substring(html.IndexOf("color: #") + 8 + 6);
                    html = bef + "color: #FFFFFF" + after;
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < 100; i++)
            {
                if (html.Contains("background:#"))
                {
                    string bef = html.Substring(0, html.IndexOf("background:#"));
                    string after = html.Substring(html.IndexOf("background:#") + 7+5 + 6);
                    html = bef + after;
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < 100; i++)
            {
                if (html.Contains("background: #"))
                {
                    string bef = html.Substring(0, html.IndexOf("background: #"));
                    string after = html.Substring(html.IndexOf("background: #") + 8 + 5 + 6);
                    html = bef + after;
                }
                else
                {
                    break;
                }
            }
            */

            html = html.Replace("<b>", "");
            html = html.Replace("</b>", "");

            return html; 
        }

        public void ExportHTML(int max = 10000000)
        {
            ClearHTML();

            Language rl = new Language(this.language);

            List<HTML> clist = new List<HTML>();

            for (int i = 0; i < Math.Min(rl.words.Count, this.count); i++)
            {
                if (!rl.words[i].spelling.Equals("linkify"))
                {
                    HTML html = new HTML(rl.words[i], this);
                    html.back += GenerateInflection(rl.words[i].spelling);

                    string file = GetDir2() + "html//articles//" + rl.words[i].spelling + ".html";



                    /*
                    bool UC = WordInUC(rl.words[i].spelling);
                    if (UC && rl.words[i].spelling[0] < 200)
                    {
                        file = GetDir2() + "html//articles//!" + rl.words[i].spelling + ".html";
                    }
                    else
                    {
                        if (WordStartsWithUpper(rl.words[i].spelling))
                        {
                            file = GetDir2() + "html//articles//!" + rl.words[i].spelling + ".html";
                        }
                    }
                    */

                    //File.WriteAllText(file2, html.back);
                    Console.WriteLine("Saved " + file);
                    double compl = 0;
                    //string newShit = tt.Convert(html.back, false);
                    File.WriteAllText(file, html.back);
                }
            }

        }

        public void ExportFileLocations(string assets)
        {
            List<String> l = new List<string>();

            string[] lst = File.ReadAllLines(assets + "filelist.txt");
            for (int i = 0; i < lst.Length; i++)
            {
                string[] allL = File.ReadAllLines(assets + "" + lst[i]);

                for (int j = 0; j < allL.Length; j++)
                {
                    if (allL[j].Contains("***"))
                    {
                        string file = allL[j].Substring(3);
                        l.Add(file + " " + lst[i]);
                    }
                }
            }

            File.WriteAllLines(assets + "locations.txt", l.ToArray());
        }

        public Word ParseSimpleEnglish(BasicWord word)
        {
            string version = "en";


            string link = "https://simple.wiktionary.org/wiki/" + word.word;
            string text = WiktionaryCollector.Collect(link);//File.ReadAllText(file);

            if (text == null)
            {
                return null;
            }

            //text = text.Replace(">change<", "><");
            text = text.Replace("[</span>", "</span>");
            text = text.Replace("]</span>", "</span>");

            text = HTML.RemoveTables(text);


            if (text.Contains("<div class=\"printfooter\">Retrieved from "))
            {
                text = text.Substring(0, text.IndexOf("<div class=\"printfooter\">Retrieved from "));
            }

            if (text.Contains("NewPP limit report"))
            {
                //text = text.Substring(0, text.IndexOf("NewPP limit report"));
            }

            if (text.Contains("<dl><dd><div class=\"notice stub plainlinks\">"))
            {
                text = text.Substring(0, text.IndexOf("<dl><dd><div class=\"notice stub plainlinks\">"));
            }

            File.WriteAllText("se.html", text);

            /*
            var doc = new HtmlDocument();
            doc.LoadHtml(text);

            //File.WriteAllText("dump.txt", text);

            var el = doc.GetElementbyId("mw-page-base");
            //Console.WriteLine(doc);

            Parser.EnglishState state = new Parser.EnglishState();
            Word res = new Word();

            res.chunks = HTMLExtractor.ExtractBlock(text, language, "<h2>", "en");
            res.englishForms = FormsGrabber.GetForms(res.chunks, this.version);


            HtmlNodeCollection nodes = doc.DocumentNode.ChildNodes;
            foreach (HtmlNode node in nodes)
            {
                Parser.ExploreEnglish(node, 0, state, res, this.version);
            }

            res.frequencyRank = word.frequencyRank;
            return res;
            */



            Word res = new Word();
            //res.chunks = HTMLExtractor.ExtractBlock(text, language, "<h2>", "en");
            res.chunks = HTMLExtractor.SeparateByHeaders(text);
            return res;
        }

        public Word ParseEnglish(BasicWord word, string target)
        {
            string version = "en";

            string text = "";
            string link = "https://en.wiktionary.org/wiki/" + word.word;
            text = WiktionaryCollector.Collect(link);//File.ReadAllText(file);
                                                     //text = text.Replace("[edit]", "");


            //File.WriteAllText("se.html", text);

            if (text == null)
            {
                return null;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(text);

            //File.WriteAllText("dump.txt", text);

            var el = doc.GetElementbyId("mw-page-base");
            //Console.WriteLine(doc);

            Parser.EnglishState state = new Parser.EnglishState();
            Word res = new Word();

            res.chunks = HTMLExtractor.ExtractBlock(text, language, "<h2>", "en");
            res.englishForms = FormsGrabber.GetForms(res.chunks, this.version);


            HtmlNodeCollection nodes = doc.DocumentNode.ChildNodes;
            foreach (HtmlNode node in nodes)
            {
                Parser.ExploreEnglish(node, 0, state, res, this.version);
            }

            res.frequencyRank = word.frequencyRank;
            return res;
        }
    }
}

