using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Lang
{
    public class HTML
    {
        public string front;
        public string back;

        public static string Accentuate(string word, int stress)
        {
            return word;

            /*
            if (stress < 0)
            {
                return word;
            }
            else
            {
                string before = word.Substring(0, stress+1);
                string letter = (char)(769) + "";
                string after = word.Substring(stress+1);
                string res = before + letter + after;
                return res;
            }
            */
        }





        public HTML(Word rw, LanguageProcessor language)
        {
            bool add = true;

            this.front = rw.spelling;
            Tuple<List<HTMLExtractor.Chunk>, List<HTMLExtractor.Chunk>> reorganized = ReorganizeChunks(rw.chunks, rw, language);
            //List<HTMLExtractor.Chunk> reorganized = rw.chunks;
            //this.back = "<head> <meta charset='utf-8'> </head> <body>";
            this.back = "";


            var hs = ChunkCleaner.HandleBadChunks(reorganized.Item1, language, rw.spelling);


            string pronoParts = "";
            string etyParts = "";
            string delimiter = "|||||";

            /*
            if (rw is RussianWord && ((RussianWord)rw).yospelling != null)
            {
                if (rw.frequencyRank == 1000000)
                {
                    pronoParts += "<h1> " + Accentuate(((RussianWord)rw).yospelling, ((RussianWord)rw).stressIndex) + "</h1>";
                }
                else
                {
                    pronoParts += "<h1> " + Accentuate(((RussianWord)rw).yospelling, ((RussianWord)rw).stressIndex) + " (" + (rw.frequencyRank + 2) + ")</h1>";
                }
            }
            else
            {
                if (rw.frequencyRank == 1000000)
                {
                    pronoParts += "<h1> " + rw.spelling + "</h1>";
                }
                else
                {
                    pronoParts += "<h1> " + rw.spelling + " (" + (rw.frequencyRank + 2) + ")</h1>";
                }
            }
            */


            for (int i = 0; i < reorganized.Item1.Count; i++)
            {
                /*
                if (reorganized.Item1[i].type == HTMLExtractor.TagType.Etymology)
                {
                    string cont = reorganized.Item1[i].content;
                    string head = reorganized.Item1[i].header;

                    string closed = head[0] + "/" + head.Substring(1);
                    string preHeader = cont.Substring(0, cont.IndexOf(closed) + closed.Length);
                    string postHeader = cont.Substring(cont.IndexOf(closed) + closed.Length + 1);


                    etyParts += postHeader + "<br>";
                }
                */

                if (reorganized.Item1[i].type == HTMLExtractor.TagType.Pronunciation)
                {
                    string cont = reorganized.Item1[i].content;
                    string head = reorganized.Item1[i].header;

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

                            pronoParts += inner;

                            postHeader = outer;
                        }
                        else
                        {
                            break;
                        }
                    }

                }
            }


            this.back = "#SNDSND1" + pronoParts + "#SNDSND2";


            if (add)
            {
                //this.back += "<head> <meta charset='utf-8'> </head> <body>";
            }

            //if (rw.spelling.Equals("to"))
            {
                //Console.WriteLine();
            }

            List<HTMLExtractor.Chunk> reorganized1 = reorganized.Item1;
            List<HTMLExtractor.Chunk> reorganized2 = reorganized.Item2;


            for (int i = 0; i < reorganized1.Count; i++)
            {
                ChunkCleaner.ClearChunk(reorganized1[i], rw.spelling);
            }

            for (int i = 0; i < reorganized2.Count; i++)
            {
                ChunkCleaner.ClearChunk(reorganized2[i], rw.spelling);
            }


            List<HTMLExtractor.TagType> mainTypes = new HTMLExtractor.TagType[]
                    {
                        //HTMLExtractor.TagType.Etymology,
                        //HTMLExtractor.TagType.Pronunciation,

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

            if (reorganized2.Count > 0)
            {
                //this.back += delimiter;
                //this.back += "\n<table class=\"center\"><tr><td>\n";
            }


            /*
            bool qq = false;
            for (int i = 0; i < reorganized1.Count; i++)
            {
                bool meaningful = ChunkCleaner.IsChunkMeaningful(reorganized1[i], hs);

                if (meaningful) {
                    if ((reorganized1[i].type == HTMLExtractor.TagType.Pronunciation))
                    {
                    }
                    else
                    {
                        if (qq)
                        {
                            if (i > 0) {
                                if (mainTypes.Contains(reorganized1[i].type))
                                {
                                    this.back += "<hr><hr>";
                                }
                                else
                                {
                                    this.back += "<br>";
                                }
                            }

                        }

                        if (reorganized1[i].type == HTMLExtractor.TagType.Conjugation
                            || reorganized1[i].type == HTMLExtractor.TagType.Declension
                            || reorganized1[i].type == HTMLExtractor.TagType.Inflection
                            || reorganized1[i].type == HTMLExtractor.TagType.Etymology
                            || reorganized1[i].type == HTMLExtractor.TagType.RelatedTerms
                            || reorganized1[i].type == HTMLExtractor.TagType.DerivedTerms
                            || reorganized1[i].type == HTMLExtractor.TagType.Synonyms
                            || reorganized1[i].type == HTMLExtractor.TagType.UsageNotes
                            )
                        {
                            string type = reorganized1[i].type.ToString();
                            if (reorganized1[i].type == HTMLExtractor.TagType.Conjugation || reorganized1[i].type == HTMLExtractor.TagType.Declension || reorganized1[i].type == HTMLExtractor.TagType.Inflection)
                            {
                                type = "Inflection";
                            }
                            this.back += "<$!$!" + type.ToLower()[0] + ">";
                        }

                        if (!mainTypes.Contains(reorganized1[i].type))
                        {

                            string cont = reorganized1[i].content;
                            string head = reorganized1[i].header;

                            if (cont.IndexOf(head) != -1)
                            {
                                string closed = head[0] + "/" + head.Substring(1);
                                string preHeader = cont.Substring(0, cont.IndexOf(closed) + closed.Length);
                                string postHeader = cont.Substring(cont.IndexOf(closed) + closed.Length + 1);


                                this.back += "<details class = \"" + reorganized1[i].type.ToString() + "\">";

                                this.back += "<summary>";
                                this.back += preHeader;
                                this.back += "</summary>";


                                this.back += postHeader;
                                this.back += "</details>";
                            }
                            else
                            {
                                this.back += reorganized1[i].content;
                            }
                        }
                        else
                        {
                            this.back += reorganized1[i].content;
                        }


                        if (reorganized1[i].type == HTMLExtractor.TagType.Conjugation
                                                || reorganized1[i].type == HTMLExtractor.TagType.Declension
                                                || reorganized1[i].type == HTMLExtractor.TagType.Inflection
                                                || reorganized1[i].type == HTMLExtractor.TagType.Etymology
                                                || reorganized1[i].type == HTMLExtractor.TagType.RelatedTerms
                                                || reorganized1[i].type == HTMLExtractor.TagType.DerivedTerms
                                                || reorganized1[i].type == HTMLExtractor.TagType.Synonyms
                                                || reorganized1[i].type == HTMLExtractor.TagType.UsageNotes
                                                )
                        {
                            this.back += "$!$$";
                        }

                        qq = true;

                    }
                }
            }
            */

            List<Tuple<HTMLExtractor.Chunk, List<HTMLExtractor.Chunk>>> pairs = new List<Tuple<HTMLExtractor.Chunk, List<HTMLExtractor.Chunk>>>();


            List<HTMLExtractor.Chunk> list2 = new List<HTMLExtractor.Chunk>();

            HTMLExtractor.Chunk lastEtymology = null;
            bool lasted = false;
            for (int i = 0; i < reorganized1.Count; i++)
            {
                bool meaningful = ChunkCleaner.IsChunkMeaningful(reorganized1[i], hs);

                if (true)//if (meaningful)
                {
                    if (mainTypes.Contains(reorganized.Item1[i].type))
                    {
                        pairs.Add(new Tuple<HTMLExtractor.Chunk, List<HTMLExtractor.Chunk>>(reorganized1[i], new List<HTMLExtractor.Chunk>()));
                        if (list2.Count != 0)
                        {
                            pairs[pairs.Count - 1].Item2.AddRange(list2);
                            list2 = new List<HTMLExtractor.Chunk>();
                            lasted = false;
                        }
                    }
                    else
                    {
                        if (reorganized1[i].type != HTMLExtractor.TagType.Etymology)
                        {
                            if (pairs.Count > 0)
                            {
                                pairs[pairs.Count - 1].Item2.Add(reorganized1[i]);
                            }
                            //list2.Add(reorganized1[i]);
                        }
                        else
                        {
                            if (!lasted)
                            {
                                if (reorganized1[i].content.Length > 110)
                                {
                                    list2.Add(reorganized1[i]);
                                    lasted = true;
                                }
                            }
                        }
                    }
                }
            }

            if (list2.Count > 0)
            {
                if (pairs.Count != 0)
                {
                    pairs[pairs.Count - 1].Item2.AddRange(list2);
                }
            }


            string table = null;
            if (reorganized2[reorganized2.Count - 1] != null)
            {
                table = reorganized2[reorganized2.Count - 1].content;
            }

            if (table != null)
            {
                table = table.Replace("Пр. действ.", "active participle, ");
                table = table.Replace("Пр. страд.", "passive participle, ");
                table = table.Replace("Пр. действ. наст.", "present active participle");
                table = table.Replace("Пр. действ. прош.", "past active participle");
                table = table.Replace("Деепр. наст.", "transgressive, present");
                table = table.Replace("Деепр. прош.", "transgressive, past");
                table = table.Replace("Пр. страд. наст.", "present passive participle");
                table = table.Replace("Пр. страд. прош.", "past passive participle");
                table = table.Replace("Будущее", "future");
                table = table.Replace("падеж", "case");
                table = table.Replace("ед. ч.", "singular");

                table = table.Replace("уст.", "dated");
                table = table.Replace("шутл.", "humorous");
                table = table.Replace("причисл.", "counting form");

                table = table.Replace("мн. ч.", "plural");
                table = table.Replace("сч.", "counting form");
                table = table.Replace("муж. р.", "masculine");
                table = table.Replace("жен. р.", "feminine");
                table = table.Replace("ср. р.", "neuter");
                table = table.Replace("Им.", "nominative");
                table = table.Replace("Р.", "genitive");
                table = table.Replace("Рд.", "genitive");
                table = table.Replace("Дт.", "dative");
                table = table.Replace("Д.", "dative");
                table = table.Replace("Вн.", "accusative");
                table = table.Replace("В.", "accusative");
                table = table.Replace("Тв.", "instrumental");
                table = table.Replace("Пр.", "prepositional");
                table = table.Replace("М.", "locative");
                table = table.Replace("Разд.", "partitive");
                table = table.Replace("Кр. форма", "short form");
                table = table.Replace("Кратк. форма", "short form");
                table = table.Replace("одуш.", "animate");
                table = table.Replace("неодуш.", "inanimate");
                table = table.Replace("неод.", "inanimate");
                table = table.Replace("наст.", "present");
                table = table.Replace("прош.", "past");
                table = table.Replace("повелит.", "imperative");
                table = table.Replace("повелит.", "imperative");
                table = table.Replace("будущ.", "future");
            }

            bool puttedTable = false;
            bool makeTable = false;
            for (int i = 0; i < pairs.Count; i++)
            {
                if (pairs[i].Item2.Count > 0)
                {
                    makeTable = true;
                }
            }
            makeTable = false;



            if (makeTable)
            {
                this.back += "<table class=\"center2\">";
            }
            else
            {

            }

            //this.back += "###Universal###";
            //this.back += "<hr>";

            bool first = true;

            for (int i = 0; i < pairs.Count; i++)
            {
                if (makeTable)
                {
                    this.back += "<tr>";
                    this.back += "<td>";
                }

                if (!language.version.Equals("ru"))
                {
                    this.back += RemoveTables(pairs[i].Item1.content);

                    if (pairs[i].Item1.type != HTMLExtractor.TagType.Unknown)
                    {
                        if (!JustAForm(pairs[i].Item1.content, language) || first)
                        {
                            this.back += "###" + pairs[i].Item1.type + "###";
                            first = false;
                        }
                        else
                        {
                            Console.WriteLine("Denied " + rw.spelling + " from " + pairs[i].Item1.type);
                        }
                    }
                }
                else
                {
                    this.back += pairs[i].Item1.content;
                    if (pairs[i].Item1.type != HTMLExtractor.TagType.Unknown)
                    {
                        if (!JustAForm(pairs[i].Item1.content, language) || first)
                        {
                            this.back += "###" + pairs[i].Item1.type + "###";
                            first = false;
                        }
                        else
                        {
                            Console.WriteLine("Denied " + rw.spelling + " from " + pairs[i].Item1.type);
                        }
                    }
                }

                if (makeTable)
                {
                    this.back += "</td>";
                    this.back += "<td>";
                }

                HTMLExtractor.TagType tp = pairs[i].Item1.type;
                if (tp == HTMLExtractor.TagType.Noun || tp == HTMLExtractor.TagType.Verb || tp == HTMLExtractor.TagType.Adjective || tp == HTMLExtractor.TagType.Pronoun || tp == HTMLExtractor.TagType.Participle || tp == HTMLExtractor.TagType.Numeral || tp == HTMLExtractor.TagType.Determiner)
                {
                    if (!puttedTable && table != null)
                    {
                        //this.back += "<h4>Inflection</h4>";
                        this.back += table;
                        puttedTable = true;
                    }
                }

                for (int j = 0; j < pairs[i].Item2.Count; j++)
                {
                    if (pairs[i].Item2[j].type != HTMLExtractor.TagType.Pronunciation)
                    {
                        if (!language.version.Equals("ru"))
                        {
                            this.back += RemoveTables(pairs[i].Item2[j].content);
                        }
                        else
                        {
                            this.back += pairs[i].Item2[j].content;
                        }
                    }
                }

                if (makeTable)
                {

                    this.back += "</td>";
                    this.back += "</tr>";
                }
                else
                {
                    this.back += "<hr>";
                }
            }

            if (makeTable)
            {
                this.back += "</table>";
            }

            if (reorganized2.Count > 0)
            {
                //this.back += "</td><td>";
            }

            if (reorganized2.Count > 0)
            {
                this.back += delimiter;
            }

            for (int i = 0; i < reorganized2.Count - 1; i++)
            {
                if (i >= 1)
                {
                    if (mainTypes.Contains(reorganized2[i].type))
                    {
                        //this.back += "<hr><hr>";
                    }

                    if (reorganized2[i].type == HTMLExtractor.TagType.МорфологическиеиСинтаксическиеСвойства)
                    {
                        //this.back += "<hr><hr>";
                    }
                }

                this.back += reorganized2[i].content;
            }

            if (reorganized2.Count > 0)
            {
                //this.back += "</td></tr></table>";
            }

            if (add)
            {
                //this.back += "</body>";
            }
            this.back += delimiter;

            /*
            if (reorganized2[reorganized2.Count - 1] != null)
            {
                this.back += reorganized2[reorganized2.Count - 1].content;
            }
            */

            this.back = this.back.Replace("&#8206;", "");

        }


        public static bool JustAForm(string content, LanguageProcessor lang, double th=0.2)
        {
            if (content.IndexOf("<ol>") == -1)
            {
                return false;
            }

            string inside = content;
            if (content.IndexOf("</ol>") - (content.IndexOf("<ol>") + 4) > 0)
            {
                inside = content.Substring(content.IndexOf("<ol>") + 4, content.IndexOf("</ol>") - (content.IndexOf("<ol>") + 4));
            }

            string[] keywords = new string[]
            {
                "alternative",
                "spelling",
                "imperative",
                "plural",
                "present",
                "past",
                "future",
                "nominative",
                "genitive",
                "dative",
                "accusative",
                "intrumental",
                "prepositional",
                "feminine",
                "masculine",
                "neuter",
                "singular",
                "indicative",
                "conditional",
                "diminutive",
                "first person",
                "second person",
                "third person",
                "participle",
                "short",
                "only used in"
            };

            /*
            inside = HTMLExtractor.RemoveLinks(inside, lang.version);
            inside = inside.Replace("<span>", "");
            inside = inside.Replace("</span>", "");
            inside = inside.Replace("<li>", "");
            inside = inside.Replace("</li>", "");
            inside = inside.Replace("<span class=\"use-with-mention\">", "");
            inside = inside.Replace("class=\"Latn mention\"", "");
            inside = inside.Replace("</i>", "");
            inside = inside.Replace("</a>", "");
            inside = inside.Replace("<i", "");
            inside = inside.Replace("<a", "");
            inside = inside.Replace("<span", "");
            inside = inside.Replace("<span", "");
            inside = inside.Replace("<span", "");
            inside = inside.Replace("<span", "");
            inside = inside.Replace("target=\"_blank\"", "");
            inside = inside.Replace("href=\"", "");
            inside = inside.Replace("https://en.wiktionary.org/wiki/", "");
            inside = inside.Replace("lang=\"", "");
            inside = inside.Replace("\"form-of-definition use-with-mention\"", "");
            inside = inside.Replace(">", "");
            inside = inside.Replace(",", "");
            */
            inside = HtmlToPlainText(inside);

            double v = 0;
            string l = inside.ToLower();
            for (int i = 0; i < keywords.Length; i++)
            {
                int count = l.Split(new string[] { keywords[i] }, StringSplitOptions.None).Length - 1;

                if (count > 0)
                {
                    v += count * keywords[i].Length;
                }
            }

            v /= l.Length;

            return v >= th;
        }


        private static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

        /*
         target="_blank"href="https://en.wiktionary.org/wiki/first_person" title="first person">first-person singular present indicative of   lang="nl">weren
  imperative of   lang="nl">weren
        
         * */

        public string RemoveAmps(string s)
        {
            while (true)
            {
                int tableIndex = s.IndexOf("&#");
                if (tableIndex != -1)
                {

                    string before = s.Substring(0, tableIndex);
                    string after = s.Substring(tableIndex + 1);
                    int index2 = after.IndexOf(";");

                    if (index2 != -1)
                    {
                        string inner = after.Substring(0, index2);
                        string after_ = after.Substring(index2 + 1);
                        s = before + " | " + after_;
                    }
                    else
                    {
                        s = before + after.Substring(2);
                    }
                }
                else
                {
                    break;
                }
            }
            return s;
        }




        public static string RemoveTables(string s)
        {
            while (true)
            {
                int tableIndex = s.IndexOf("<table");
                if (tableIndex != -1)
                {

                    string before = s.Substring(0, tableIndex);
                    string after = s.Substring(tableIndex + 1);
                    int index2 = after.IndexOf("</table>");

                    try
                    {
                        string inner = after.Substring(0, index2);
                        string after_ = after.Substring(index2 + 8);
                        s = before + after_;
                    }
                    catch (Exception ex)
                    {
                        return s;
                    }
                }
                else
                {
                    break;
                }
            }
            return s;
        }


        public static List<int> AllIndexesOf(string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }




        public static string RemoveHTMLTag(string s, string begin, string end, Func<string, bool> condo, int step = 0)
        {
            if (step > 20)
            {
                return s;
            }

            int tableIndex = s.IndexOf(begin);
            if (tableIndex != -1)
            {

                string before = s.Substring(0, tableIndex);
                string after = s.Substring(tableIndex);
                int index2 = after.IndexOf(end);

                try
                {
                    string inner = after.Substring(0, index2);
                    string after_ = after.Substring(index2 + end.Length);

                    bool ccc = condo(inner);

                    if (ccc)
                    {
                        return before + RemoveHTMLTag(after_, begin, end, condo, step++);
                    }
                    else
                    {
                        return before + inner + RemoveHTMLTag(after_, begin, end, condo, step++);
                    }
                }
                catch (Exception ex)
                {
                    return s;
                }
            }
            else
            {
                return s;
            }
        }


        public static string RemoveHTMLTag2(string s, string begin, string end, Func<string, string> condo, int step = 0)
        {
            if (step > 20)
            {
                return s;
            }

            int tableIndex = s.IndexOf(begin);
            if (tableIndex != -1)
            {

                string before = s.Substring(0, tableIndex);
                string after = s.Substring(tableIndex);
                int index2 = after.IndexOf(end);

                try
                {
                    string inner = after.Substring(0, index2);
                    string after_ = after.Substring(index2 + end.Length);

                    string news = condo(inner);

                    return before + news +  end + RemoveHTMLTag2(after_, begin, end, condo, step++);
                }
                catch (Exception ex)
                {
                    return s;
                }
            }
            else
            {
                return s;
            }
        }


        /*
        public static string RemoveHTMLTag(string s, string begin, string end, Func<string, bool> condo)
        {
            List<int> inds = AllIndexesOf(s, begin);


            int skips = 0;
            while (true)
            {
                int tableIndex = -1;
                if (skips < inds.Count)
                {
                    tableIndex = inds[skips];
                }

                //inds[skips]; //s.IndexOf(begin);
                if (tableIndex != -1)
                {

                    string before = s.Substring(0, tableIndex);
                    string after = s.Substring(tableIndex + 1);
                    int index2 = after.IndexOf(end);

                    try
                    {
                        string inner = after.Substring(0, index2);
                        string after_ = after.Substring(index2 + end.Length);

                        if (condo(inner)) {
                            s = before + after_;
                        }
                        else
                        {
                            s = before + inner + after_;
                        }
                    }
                    catch (Exception ex)
                    {
                        return s;
                    }
                }
                else
                {
                    break;
                }
            }
            return s;
        }
        */


        public Tuple<List<HTMLExtractor.Chunk>, List<HTMLExtractor.Chunk>> ReorganizeChunks(List<HTMLExtractor.Chunk> chunks, Word word, LanguageProcessor proc)
        {
            /*
            for (int i = 0; i < chunks.Count; i++)
            {
                if (chunks[i].type == HTMLExtractor.TagType.Unknown && chunks[i].content.Contains("Definitions"))
                {
                    chunks[i].type = HTMLExtractor.TagType.Definitions;
                }
            }
            */

            List<HTMLExtractor.Chunk> res1 = new List<HTMLExtractor.Chunk>();
            List<HTMLExtractor.Chunk> res2 = new List<HTMLExtractor.Chunk>();

            List<HTMLExtractor.TagType> acceptableTypes1 = new HTMLExtractor.TagType[]
            {
                HTMLExtractor.TagType.Etymology,
                HTMLExtractor.TagType.Noun,
                HTMLExtractor.TagType.Adjective,
                HTMLExtractor.TagType.Adverb,
                HTMLExtractor.TagType.Interjection,
                HTMLExtractor.TagType.Participle,
                HTMLExtractor.TagType.Preposition,
                HTMLExtractor.TagType.Pronoun,
                HTMLExtractor.TagType.RelatedTerms,
                HTMLExtractor.TagType.Verb,
                HTMLExtractor.TagType.UsageNotes,
                HTMLExtractor.TagType.Participle,
                HTMLExtractor.TagType.Particle,
                HTMLExtractor.TagType.Conjunction,
                HTMLExtractor.TagType.Determiner,
                HTMLExtractor.TagType.Numeral,
                HTMLExtractor.TagType.Article,
                HTMLExtractor.TagType.DerivedTerms,
                HTMLExtractor.TagType.Contraction,
                HTMLExtractor.TagType.ProperName,
                HTMLExtractor.TagType.Predicative,
                HTMLExtractor.TagType.Letter,
                HTMLExtractor.TagType.Pronunciation,
                //HTMLExtractor.TagType.Phrase,
                HTMLExtractor.TagType.Definitions,
                HTMLExtractor.TagType.SeeAlso
            }.ToList();

            List<HTMLExtractor.TagType> acceptableTypes2 = new HTMLExtractor.TagType[]
            {
                HTMLExtractor.TagType.МорфологическиеиСинтаксическиеСвойства,
                HTMLExtractor.TagType.Значение,
                HTMLExtractor.TagType.Синонимы,

                HTMLExtractor.TagType.Lidwoord,
                HTMLExtractor.TagType.Hoofdtelwoord,
                HTMLExtractor.TagType.ZelfstandigNaamwoord,
                HTMLExtractor.TagType.BijvoeglijkNaamwoord,
                HTMLExtractor.TagType.Voorzetsel,
                HTMLExtractor.TagType.Citaten,
                HTMLExtractor.TagType.AanwijzendVoornaamwoord,
                HTMLExtractor.TagType.PersoonlijkVoornaamwoord,
                HTMLExtractor.TagType.BezittelijkVoornaamwoord,
                HTMLExtractor.TagType.Voegwoord,
                HTMLExtractor.TagType.Synoniemen,
                HTMLExtractor.TagType.Werkwoord,
                HTMLExtractor.TagType.Rangtelwoord,
                HTMLExtractor.TagType.VerwanteBegrippen,
                HTMLExtractor.TagType.Bijwoord
            }.ToList();



            if (proc.version.Equals("nl") && false)
            {
                acceptableTypes1.Add(HTMLExtractor.TagType.Inflection);
                acceptableTypes1.Add(HTMLExtractor.TagType.Declension);
                acceptableTypes1.Add(HTMLExtractor.TagType.Conjugation);
            }

            if (!proc.version.Equals("zh"))
            {
                acceptableTypes1.Add(HTMLExtractor.TagType.Synonyms);
            }



            for (int i = 0; i < chunks.Count; i++)
            {
                HTMLExtractor.Chunk chunk = chunks[i];

                if (chunk.type == HTMLExtractor.TagType.ZelfstandigNaamwoord)
                {
                    //chunk.content = RemoveTables(chunk.content);
                }

                if (chunk.type == HTMLExtractor.TagType.Werkwoord)
                {
                    // chunk.content = RemoveTables(chunk.content);
                }

                if (acceptableTypes1.Contains(chunk.type))
                {
                    res1.Add(chunk);
                }

                if (acceptableTypes2.Contains(chunk.type))
                {
                    res2.Add(chunk);
                }
            }



            HTMLExtractor.Chunk lastChunk = null;

            if (res2.Count > 0)
            {
                int[] puntos = new int[res2.Count];
                for (int i = 0; i < res2.Count; i++)
                {
                    string cont = res2[i].content;

                    if (cont.Contains("<table ") && cont.Contains("</table>"))
                    {
                        //if (cont.Contains("Существительное") || cont.Contains("Прилагательное") || cont.Contains("Глагол") || cont.Contains("Местоимение") || cont.Contains("местоимение") || cont.Contains("Причастие") || cont.Contains("Числительное"))
                        {
                            if (cont.Contains("Существительное") || cont.Contains("Глагол") || cont.Contains("существительное"))
                            {
                                puntos[i] = 2;
                            }

                            if (cont.Contains("Прилагательное") || cont.Contains("прилагательное"))
                            {
                                puntos[i] = 3;
                            }

                            if (cont.Contains("Местоимение") || cont.Contains("местоимение") || cont.Contains("Числительное") || cont.Contains("Причастие"))
                            {
                                puntos[i] = 4;
                            }
                        }

                    }

                }



                int bi = 0;
                for (int i = 0; i < puntos.Length; i++)
                {
                    if (puntos[i] > 0 && puntos[i] > puntos[bi])
                    {
                        bi = i;
                    }
                }


                if (puntos[bi] != 0)
                {
                    string cont = res2[bi].content;
                    string body = cont.Substring(cont.IndexOf("<table ") - 1, cont.IndexOf("</table>") + "</table>".Length - cont.IndexOf("<table ") + 1);

                    HTMLExtractor.Chunk ch = new HTMLExtractor.Chunk("<h3>", body);
                    ch.type = HTMLExtractor.TagType.Inflection;

                    lastChunk = ch;
                    res2.Add(lastChunk);
                }
                else
                {
                    res2.Add(null);
                }


                for (int i = 0; i < res2.Count - 1; i++)
                {
                    if (puntos[i] > 0)
                    {
                        string cont = res2[bi].content;

                        if (cont.IndexOf("<table ") != -1)
                        {
                            string body = cont.Substring(0, cont.IndexOf("<table ")) + cont.Substring(cont.IndexOf("</table>") + "</table>".Length);
                            res2[i].content = body;
                        }
                    }
                }
            }
            else
            {
                res2.Add(null);
            }


            return new Tuple<List<HTMLExtractor.Chunk>, List<HTMLExtractor.Chunk>>(res1, res2);
        }
    }
}
