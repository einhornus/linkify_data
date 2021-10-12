using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lang
{
    public class ChunkCleaner
    {

        public static string RemoveTag(string s, string begin, string tagName)
        {
            string newS = s;
            while (true)
            {
                if (newS.Contains(begin))
                {
                    int index = newS.IndexOf(begin);

                    string before = newS.Substring(0, index);
                    string after = newS.Substring(index + begin.Length);

                    int pop = 0;

                    while (true)
                    {
                        int index1 = after.IndexOf("<" + tagName);
                        int index2 = after.IndexOf("</" + tagName + ">");


                        //if (index1 == -1 && index2 != -1)
                        {
                            if (pop == -1)
                            {
                                after = after.Substring(index2 + ("</" + tagName + ">").Length);

                                newS = before + after;
                                break;
                            }
                        }


                        if (index1 == -1 && index2 == -1)
                        {
                            return s;
                        }

                        if (index1 != -1 && (index2 != -1 && index1 < index2))
                        {
                            after = after.Substring(index1 + ("<" + tagName + ">").Length);
                            pop++;
                            continue;
                        }

                        if (index1 != -1 && (index2 == -1))
                        {
                            after = after.Substring(index1 + ("<" + tagName + ">").Length);
                            pop++;
                            continue;
                        }

                        if (index2 != -1 && (index1 != -1 && index2 < index1))
                        {
                            after = after.Substring(index2 + ("</" + tagName + ">").Length);
                            pop--;
                            continue;
                        }

                        if (index2 != -1 && (index1 == -1))
                        {
                            after = after.Substring(index2 + ("</" + tagName + ">").Length);
                            pop--;
                            continue;
                        }

                    }
                }
                else
                {
                    break;
                }
            }

            return newS;
        }

        public static string repl(string x, string w)
        {
            if (x.Contains("title="+w+"andaction"))
            {
                return "";
            }
            else
            {
                return x.Substring(x.IndexOf("title=") + "title=".Length, x.IndexOf("andaction") - (x.IndexOf("title=") + "title=".Length));
            }
        }

        public static void ClearChunk(HTMLExtractor.Chunk chunk, string w)
        {
            if (chunk == null)
            {
                return;
            }

            chunk.content = chunk.content.Replace("bgcolor=\"#eef9ff\"", "");
            chunk.content = chunk.content.Replace("bgcolor=\"#ffffff\"", "");
            chunk.content = chunk.content.Replace("bgcolor=\"#EEF9FF\"", "");
            chunk.content = chunk.content.Replace("bgcolor=\"#eeeeee\"", "");
            chunk.content = chunk.content.Replace("bgcolor=\"#EEEEEE\"", "");


            chunk.content = chunk.content.Replace("&#", "$$1");
            chunk.content = chunk.content.Replace("#0", "");
            chunk.content = chunk.content.Replace("#1", "");
            chunk.content = chunk.content.Replace("#2", "");
            chunk.content = chunk.content.Replace("#3", "");
            chunk.content = chunk.content.Replace("#4", "");
            chunk.content = chunk.content.Replace("#5", "");
            chunk.content = chunk.content.Replace("#6", "");
            chunk.content = chunk.content.Replace("#7", "");
            chunk.content = chunk.content.Replace("#8", "");
            chunk.content = chunk.content.Replace("#9", "");
            chunk.content = chunk.content.Replace("#A", "");
            chunk.content = chunk.content.Replace("#B", "");
            chunk.content = chunk.content.Replace("#C", "");
            chunk.content = chunk.content.Replace("#D", "");
            chunk.content = chunk.content.Replace("#E", "");
            chunk.content = chunk.content.Replace("#F", "");
            chunk.content = chunk.content.Replace("#a", "");
            chunk.content = chunk.content.Replace("#b", "");
            chunk.content = chunk.content.Replace("#c", "");
            chunk.content = chunk.content.Replace("#d", "");
            chunk.content = chunk.content.Replace("#e", "");
            chunk.content = chunk.content.Replace("#f", "");
            chunk.content = chunk.content.Replace("&amp;", "and");

            chunk.content = chunk.content.Replace("$$1", "&#");

            chunk.content = chunk.content.Replace("color:green", "color:yellow");

            chunk.content = chunk.content.Replace("&#32;", " ");

            chunk.content = HTML.RemoveHTMLTag2(chunk.content, "<a href=\"/w/index.php?title=", "</a>", x=>repl(x, w));
            chunk.content = HTML.RemoveHTMLTag(chunk.content, "<span class=\"mw-editsection\">", "</span>", x => true);

            chunk.content = chunk.content.Replace("<li></li>", "");

            if (chunk.type == HTMLExtractor.TagType.МорфологическиеиСинтаксическиеСвойства)
            {
                int tableEndIndex = chunk.content.IndexOf("</table>");
                if (tableEndIndex != -1)
                {
                    string before = chunk.content.Substring(0, tableEndIndex + "</table>".Length);
                    string after = chunk.content.Substring(tableEndIndex + "</table>".Length);

                    chunk.content = before + "<br>";

                    if (after.Contains("Глагол"))
                    {
                        chunk.content += "Глагол";
                    }

                    if (after.Contains("Существительное"))
                    {
                        chunk.content += "Существительное";
                    }

                    if (after.Contains("Прилагательное"))
                    {
                        chunk.content += "Прилагательное";
                    }

                    if (after.Contains("Местоимение"))
                    {
                        chunk.content += "Местоимение";
                    }

                    if (after.Contains("Числительное"))
                    {
                        chunk.content += "Числительное";
                    }

                    if (after.Contains("Причастие"))
                    {
                        chunk.content += "Причастие";
                    }
                }
                else
                {
                    bool nullify = true;

                    if (chunk.content.Contains("Предлог"))
                    {
                        chunk.content = "Предлог";
                        nullify = false;
                    }

                    if (chunk.content.Contains("Союз"))
                    {
                        chunk.content = "Союз";
                        nullify = false;
                    }

                    if (chunk.content.Contains("Частица"))
                    {
                        chunk.content = "Частица";
                        nullify = false;
                    }

                    if (chunk.content.Contains("Наречие"))
                    {
                        chunk.content = "Наречие";
                        nullify = false;
                    }

                    if (chunk.content.Contains("Деепричастие"))
                    {
                        chunk.content = "Деепричастие";
                        nullify = false;
                    }

                    if (nullify)
                    {
                        chunk.content = "";
                    }
                }
            }

            if (chunk.type == HTMLExtractor.TagType.Значение)
            {

                string part = chunk.content;

                string[] parts = chunk.content.Split(new string[] { "<li>", "</li>" }, StringSplitOptions.RemoveEmptyEntries);
                string[] newParts = new string[parts.Length];


                for (int k = 1; k < parts.Length - 1; k++)
                {
                    string[] sss = parts[k].Split(new string[] { "&#9670;&#160;" }, StringSplitOptions.RemoveEmptyEntries);
                    if (sss.Length > 1)
                    {
                        string newCont = "<li>" + sss[0];
                        newCont += "<ul>\n";
                        for (int i = 1; i < sss.Length; i++)
                        {
                            if (!sss[i].Contains("Отсутствует пример употребления"))
                            {
                                newCont += "<li>" + sss[i] + "</li>\n";
                            }
                        }
                        newCont += "</ul></li>\n\n";
                        newParts[k] = newCont;
                    }
                    else
                    {
                        newParts[k] = parts[k];
                    }
                }
                newParts[0] = parts[0];
                newParts[newParts.Length - 1] = parts[parts.Length - 1];

                string nk = "";
                for (int k = 0; k < parts.Length; k++)
                {
                    nk += newParts[k];
                }
                chunk.content = nk;

                //chunk.content = chunk.content.Replace("&#9670;&#160;", "<br>");

                /*
                string[] parts = chunk.content.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i<parts.Length; i++)
                {
                    parts[i] = Meaning.Namefication(parts[i]);
                }

                chunk.content = String.Join("<br>", parts);
                */

                chunk.content = chunk.content.Replace("Отсутствует пример употребления см. ", "");
                chunk.content = chunk.content.Replace("Отсутствует пример употребления", "");
                chunk.content = chunk.content.Replace(">рекомендации<", "><");
                chunk.content = chunk.content.Replace("> (см. <", "><");
                chunk.content = chunk.content.Replace("</span>).</span>", "</span></span>");


                string q = "(цитата из <a href=\"https://ru.wikipedia.org/wiki/%D0%9D%D0%B0%D1%86%D0%B8%D0%BE%D0%BD%D0%B0%D0%BB%D1%8C%D0%BD%D1%8B%D0%B9_%D0%BA%D0%BE%D1%80%D0%BF%D1%83%D1%81_%D1%80%D1%83%D1%81%D1%81%D0%BA%D0%BE%D0%B3%D0%BE_%D1%8F%D0%B7%D1%8B%D0%BA%D0%B0\" class=\"extiw\" title=\"w:Национальный корпус русского языка\">Национального корпуса русского языка</a>, см. Список литературы)";

                if (chunk.content.Contains(q))
                {
                    chunk.content = chunk.content.Replace(q, "");
                }

                chunk.content = RemoveTag(chunk.content, "<sup", "sup");
            }

            chunk.content = chunk.content.Replace("Морфологические и синтаксические свойства", "");
            chunk.content = chunk.content.Replace("</span>edit<span", "</span><span");
            //chunk.content = chunk.content.Replace("</span>change<span", "</span><span");

            chunk.content = chunk.content.Replace("<span class=\"mw-editsection-bracket\">]</span>", "");
            chunk.content = chunk.content.Replace("<span class=\"mw-editsection-bracket\">[</span>", "");

            chunk.content = chunk.content.Replace("<span class=\"mw-editsection\">править</span>", "");

            chunk.content = chunk.content.Replace("&#160;", " ");
            chunk.content = chunk.content.Replace("&#32;", " ");

            chunk.content = chunk.content.Replace("color:darkgreen", "color:black");

            chunk.content = chunk.content.Replace("<span style=\"font-style: italic;background-color:#CCFFFF;\"", "<span class=\"coolthing\"");
            chunk.content = chunk.content.Replace("<span style=\"font-style: italic;background-color:#CCFFFF;cursor:help;\"", "<span class=\"coolthing\"");
            //chunk.content = chunk.content.Replace("&#91;", " ");

            chunk.content = RemoveTag(chunk.content, "<span class=\"example-details\"", "span");
            chunk.content = RemoveTag(chunk.content, "<span class=\"example-details\"", "span");

            //chunk.content = HTMLExtractor.RemoveLinks(chunk.content, "en");


            //chunk.content = RemoveTag(chunk.content, "<i lang=\"ru-Latn\" class=\"e-transliteration tr Latn\"", "i");
            //chunk.content = RemoveTag(chunk.content, "<span lang=\"ru-Latn\"", "span");
            //chunk.content = chunk.content.Replace("― ―", " ― ");
            //chunk.content = chunk.content.Replace("()", "");

            chunk.content = chunk.content.Replace("<hr />", "");
            chunk.content = chunk.content.Replace("data-column-count=\"4\"", "");
            chunk.content = chunk.content.Replace("data-column-count=\"3\"", "");
            chunk.content = chunk.content.Replace("data-column-count=\"5\"", "");
            chunk.content = chunk.content.Replace("data-column-count=\"6\"", "");
            chunk.content = chunk.content.Replace("data-column-count=\"7\"", "");
            chunk.content = chunk.content.Replace("background-color: #F8F8FF;", "");

            /*
            while (true) {
                string sx = "&#91;";
                if (chunk.content.Contains(sx))
                {
                    string bef = chunk.content.Substring(0, chunk.content.IndexOf(sx));
                    string _af = chunk.content.Substring(chunk.content.IndexOf(sx) + sx.Length);
                    string after = _af.Substring(_af.IndexOf(sx)+sx.Length);
                    chunk.content = bef + after;
                }
                else
                {
                    break;
                }
            }
            */


            if (chunk.type == HTMLExtractor.TagType.Синонимы)
            {
                if (chunk.content.Length < 150)
                {
                    chunk.content = "";
                }
            }
        }

        public static HashSet<HTMLExtractor.Chunk> HandleBadChunks(List<HTMLExtractor.Chunk> chunks, LanguageProcessor processor, string word)
        {

            if (!processor.badLemmas.ContainsKey(word))
            {
                return new HashSet<HTMLExtractor.Chunk>();
            }


            Dictionary<string, HTMLExtractor.TagType> typeDics = new Dictionary<string, HTMLExtractor.TagType>();

            typeDics.Add("noun", HTMLExtractor.TagType.Noun);
            typeDics.Add("adj", HTMLExtractor.TagType.Adjective);
            typeDics.Add("verb", HTMLExtractor.TagType.Verb);
            typeDics.Add("pron", HTMLExtractor.TagType.Pronoun);
            typeDics.Add("particle", HTMLExtractor.TagType.Participle);
            typeDics.Add("article", HTMLExtractor.TagType.Article);
            typeDics.Add("prep", HTMLExtractor.TagType.Preposition);
            typeDics.Add("det", HTMLExtractor.TagType.Determiner);

            Dictionary<string, int> poses = processor.badLemmas[word];
            Dictionary<string, List<HTMLExtractor.Chunk>> relevantChunks = new Dictionary<string, List<HTMLExtractor.Chunk>>();


            for (int i = 0; i<chunks.Count; i++)
            {
                chunks[i].content = chunks[i].content.Replace("&#32;", " ");

                if (chunks[i].content.Contains(" of "))
                {
                    foreach (var v in typeDics.Keys)
                    {
                        if (typeDics[v] == chunks[i].type)
                        {
                            if (!relevantChunks.ContainsKey(v))
                            {
                                relevantChunks.Add(v, new List<HTMLExtractor.Chunk>());
                            }

                            relevantChunks[v].Add(chunks[i]);
                        }
                    }
                }
            }

            HashSet<HTMLExtractor.Chunk> res = new HashSet<HTMLExtractor.Chunk>();

            foreach (var v in typeDics.Keys)
            {
                if (poses.ContainsKey(v) && relevantChunks.ContainsKey(v))
                {
                    int cnt = poses[v];
                    List<HTMLExtractor.Chunk> relevant = relevantChunks[v];

                    List<Tuple<HTMLExtractor.Chunk, int>> sortlist = new List<Tuple<HTMLExtractor.Chunk, int>>();
                    for (int i = 0; i<relevant.Count; i++)
                    {
                        sortlist.Add(new Tuple<HTMLExtractor.Chunk, int>(relevant[i], relevant[i].content.Length));
                    }


                    if (cnt != sortlist.Count)
                    {
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Good "+word);
                    }

                    sortlist.Sort((x, y)=>x.Item2.CompareTo(y.Item2));
                    for (int i = 0; i<Math.Min(cnt, sortlist.Count); i++)
                    {
                        res.Add(sortlist[i].Item1);
                    }
                }
            }

            return res;
        }

        public static bool IsChunkMeaningful(HTMLExtractor.Chunk chunk, HashSet<HTMLExtractor.Chunk> dic)
        {
            return !dic.Contains(chunk);
        }
    }
}