using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lang
{
    public class HTMLExtractor
    {
        public enum TagType
        {
            Etymology,
            Pronunciation,
            SeeAlso,
            AlternativeForms,
            RelatedTerms,
            Noun,
            Preposition,
            Verb,
            Adjective,
            Adverb,
            Unknown,
            Interjection,
            Participle,
            Pronoun,
            Declension,
            Conjugation,
            Particle,
            Synonyms,
            UsageNotes,
            Conjunction,
            Article,
            Numeral,
            Determiner,
            Inflection,
            DerivedTerms,
            Contraction,
            ProperName,
            Predicative,
            Letter,
            Translations,
            Phrase,
            Definitions,
            Antonyms,

            МорфологическиеиСинтаксическиеСвойства,
            Значение,
            Синонимы,

            Lidwoord,
            Hoofdtelwoord,
            ZelfstandigNaamwoord,
            BijvoeglijkNaamwoord,
            AfgeleideBegrippen,
            Voorzetsel,
            Citaten,
            AanwijzendVoornaamwoord,
            PersoonlijkVoornaamwoord,
            BezittelijkVoornaamwoord,
            Voegwoord,
            Synoniemen,
            Werkwoord,
            Rangtelwoord,
            VerwanteBegrippen,
            Bijwoord
        }



        public static List<RussianDerivedWord> GetFormsRussian(List<Chunk> englishChunks)
        {
            List<RussianDerivedWord> res = new List<RussianDerivedWord>();

            for (int i = 0; i < englishChunks.Count; i++)
            {
                if (englishChunks[i].type == TagType.Inflection || englishChunks[i].type == TagType.Conjugation || englishChunks[i].type == TagType.Declension)
                {
                    string s = englishChunks[i].content;
                    int tableIndex = s.IndexOf("<table");
                    if (tableIndex != -1)
                    {

                        string before = s.Substring(0, tableIndex);
                        string after = s.Substring(tableIndex + 1);
                        int index2 = after.IndexOf("</table>");

                        string inner = after.Substring(0, index2);
                        string after_ = after.Substring(index2 + 8);
                        s = before + after_;

                        while (true)
                        {
                            //string q = "<span class=\"Latn\" lang=\"nl\">";
                            string q = "lang=\"ru\">";
                            int index = inner.IndexOf(q);


                            if (index != -1)
                            {
                                string _before = inner.Substring(0, index);
                                string _after = inner.Substring(index + 1);
                                int _index2 = _after.IndexOf("</span>");

                                string f = inner.Substring(index + q.Length, _index2 - q.Length + 1);

                                if (!f.Contains('<'))
                                {
                                    string[] exceptions = new string[] { "het", "dit", "wat", "dat", "iets", "niets", "alles" };
                                    if (!exceptions.Contains(f))
                                    {
                                        if (!f.Contains(" "))
                                        {
                                            RussianDerivedWord ps = RussianWord.ParseStress(f);
                                            if (ps != null)
                                            {
                                                res.Add(ps);
                                            }
                                        }
                                    }
                                }
                                inner = _after;
                            }
                            else
                            {
                                break;
                            }
                        }



                        //Console.WriteLine(inner);
                    }

                    //break;
                }

            }


            int cnt = 0;
            for (int i = 0; i < res.Count; i++)
            {
                if (res[i].spelling.Equals("ты"))
                {
                    cnt++;
                }

                if (res[i].spelling.Equals("я"))
                {
                    cnt++;
                }

                if (res[i].spelling.Equals("вы"))
                {
                    cnt++;
                }

                if (res[i].spelling.Equals("мы"))
                {
                    cnt++;
                }

                if (res[i].spelling.Equals("он"))
                {
                    cnt++;
                }
            }

            if (cnt > 2)
            {
                return new List<RussianDerivedWord>();
            }


            if (res.Count == 66 && res[0].spelling.Equals("я"))
            {
                return new List<RussianDerivedWord>();
            }

            return res;
        }

        public static List<string> GetForms(string s, string version)
        {
            s = s.Replace("cmn", "zh");

            if (s.Contains(","))
            {
                string[] spl = s.Split(new string[] { "," }, StringSplitOptions.None);

                List<string> res = new List<string>();
                for (int i = 0; i<spl.Length; i++)
                {
                    List<string> local = GetForms(spl[i], version);
                    res.AddRange(local);
                }
                return res;
            }
            else
            {

                List<string> res = new List<string>();

                while (true)
                {
                    //string q = "<span class=\"Latn\" lang=\"nl\">";

                    string q = "lang=\"" + version + "\">";
                    int index = s.IndexOf(q);


                    if (index != -1)
                    {
                        string _before = s.Substring(0, index);
                        string _after = s.Substring(index + 1);
                        int _index2 = _after.IndexOf("</span>");

                        if (_index2 != -1)
                        {
                            string f = s.Substring(index + q.Length, _index2 - q.Length + 1);

                            if (!f.Contains('<'))
                            {
                                f = f.Replace((char)(769) + "", "");
                                res.Add(f);
                            }
                            s = _after;
                        }
                        else
                        {
                            return new List<string>();
                        }
                    }
                    else
                    {
                        break;
                    }
                }


                while (true)
                {
                    //string q = "<span class=\"Latn\" lang=\"nl\">";
                    string q = "lang=\"" + version + "\" class=\"Latn\">";
                    int index = s.IndexOf(q);


                    if (index != -1)
                    {
                        string _before = s.Substring(0, index);
                        string _after = s.Substring(index + 1);
                        int _index2 = _after.IndexOf("</span>");

                        if (_index2 != -1)
                        {
                            string f = s.Substring(index + q.Length, _index2 - q.Length + 1);

                            if (!f.Contains('<'))
                            {
                                //f = f.Replace((char)(769) + "", "");
                                res.Add(f);
                            }
                            s = _after;
                        }
                        else
                        {
                            return new List<string>();
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                string r = String.Join(" ", res);
                List<string> resres = new List<string>();

                if (r.Length > 0) {
                    resres.Add(r);
                }

                return resres;
            }
        }

        public class Chunk
        {
            public string header;
            public string content;
            public TagType type;

            public Chunk(string header, string content)
            {
                this.header = header;
                this.content = content;

                type = TagType.Unknown;

                if (content.Contains("Etymology"))
                {
                    type = TagType.Etymology;
                }

                if (content.Contains("Translations"))
                {
                    type = TagType.Translations;
                }

                if (content.Contains("Pronunciation"))
                {
                    type = TagType.Pronunciation;
                }

                if (content.Contains("See also"))
                {
                    type = TagType.SeeAlso;
                }

                if (content.Contains("Alternative forms"))
                {
                    type = TagType.AlternativeForms;
                }

                if (content.Contains("Related terms") || content.Contains("Related words"))
                {
                    type = TagType.RelatedTerms;
                }

                if (content.Contains("Antonyms"))
                {
                    type = TagType.Antonyms;
                }

                if (content.Contains("Noun"))
                {
                    type = TagType.Noun;
                }

                if (content.Contains("Contraction"))
                {
                    type = TagType.Contraction;
                }

                if (content.Contains("Proper noun"))
                {
                    type = TagType.ProperName;
                }

                if (content.Contains("Preposition"))
                {
                    type = TagType.Preposition;
                }

                if (content.Contains("Verb"))
                {
                    type = TagType.Verb;
                }

                if (content.Contains("Derived terms"))
                {
                    type = TagType.DerivedTerms;
                }

                if (content.Contains("Adjective"))
                {
                    type = TagType.Adjective;
                }

                if (content.Contains("Adverb"))
                {
                    type = TagType.Adverb;
                }

                if (content.Contains("Interjection"))
                {
                    type = TagType.Interjection;
                }

                if (content.Contains("Participle"))
                {
                    type = TagType.Participle;
                }

                if (content.Contains("Article"))
                {
                    type = TagType.Article;
                }

                if (content.Contains("Pronoun"))
                {
                    type = TagType.Pronoun;
                }

                if (content.Contains("Numeral"))
                {
                    type = TagType.Numeral;
                }

                if (content.Contains("Determiner"))
                {
                    type = TagType.Determiner;
                }

                if (content.Contains("Predicative"))
                {
                    type = TagType.Predicative;
                }

                if (content.Contains("Letter"))
                {
                    type = TagType.Letter;
                }

                if (content.Contains("Particle"))
                {
                    type = TagType.Particle;
                }

                if (content.Contains("Declension"))
                {
                    type = TagType.Declension;
                }

                if (content.Contains("Conjunction"))
                {
                    type = TagType.Conjunction;
                }

                if (content.Contains("Bijwoord"))
                {
                    type = TagType.Bijwoord;
                }

                if (content.Contains("Synonyms"))
                {
                    type = TagType.Synonyms;
                }

                if (content.Contains("Declension"))
                {
                    type = TagType.Declension;
                }

                /*
                if (content.Contains("Phrase"))
                {
                    type = TagType.Phrase;
                }
                */

                if (content.Contains("Conjugation"))
                {
                    type = TagType.Conjugation;
                }

                if (content.Contains("Usage notes"))
                {
                    type = TagType.UsageNotes;
                }

                if (content.Contains("Inflection"))
                {
                    type = TagType.Inflection;
                }

                if (content.Contains("Definitions"))
                {
                    type = TagType.Definitions;
                }


                if (content.Contains("Морфологические и синтаксические свойства"))
                {
                    type = TagType.МорфологическиеиСинтаксическиеСвойства;
                }

                if (content.Contains("Значение"))
                {
                    type = TagType.Значение;
                }

                if (content.Contains("Синонимы"))
                {
                    type = TagType.Синонимы;
                }




                if (content.Contains("Lidwoord"))
                {
                    type = TagType.Lidwoord;
                }

                if (content.Contains("Hoofdtelwoord"))
                {
                    type = TagType.Hoofdtelwoord;
                }

                if (content.Contains("Zelfstandig naamwoord"))
                {
                    type = TagType.ZelfstandigNaamwoord;
                }

                if (content.Contains("Bijvoeglijk naamwoord"))
                {
                    type = TagType.BijvoeglijkNaamwoord;
                }


                if (content.Contains("Afgeleide begrippen"))
                {
                    type = TagType.AfgeleideBegrippen;
                }

                if (content.Contains("Voorzetsel"))
                {
                    type = TagType.Voorzetsel;
                }

                if (content.Contains("Citaten"))
                {
                    type = TagType.Citaten;
                }

                if (content.Contains("Aanwijzend voornaamwoord"))
                {
                    type = TagType.AanwijzendVoornaamwoord;
                }


                if (content.Contains("Persoonlijk voornaamwoord"))
                {
                    type = TagType.PersoonlijkVoornaamwoord;
                }

                if (content.Contains("Voegwoord"))
                {
                    type = TagType.Voegwoord;
                }

                if (content.Contains("Synoniemen"))
                {
                    type = TagType.Synoniemen;
                }

                if (content.Contains("Werkwoord"))
                {
                    type = TagType.Werkwoord;
                }

                if (content.Contains("Rangtelwoord"))
                {
                    type = TagType.Rangtelwoord;
                }

                if (content.Contains("Verwante begrippen"))
                {
                    type = TagType.VerwanteBegrippen;
                }
            }
        }


        public static List<Chunk> ExtractBlock(string text, string language, string mainHeader, string version)
        {
            string[] parts = text.Split(new string[] { mainHeader }, StringSplitOptions.RemoveEmptyEntries);

            int[] russianPoses = new int[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                russianPoses[i] = parts[i].IndexOf(language);
                if (russianPoses[i] == -1)
                {
                    russianPoses[i] = int.MaxValue;
                }
            }



            int bestIndex = 0;
            int bb = 0;
            for (int i = 0; i < parts.Length; i++)
            {
                if (russianPoses[i] < russianPoses[bestIndex])
                {
                    bestIndex = i;
                    bb = russianPoses[i];
                }
            }

            if (bb > 1000)
            {
                return new List<Chunk>();
            }

            string res = parts[bestIndex];

            if (res.Contains("<!--"))
            {
                res = res.Substring(0, res.IndexOf("<!--"));
            }

            res = RemoveLinks(res, version);

            List<Chunk> chunks = SeparateByHeaders(res);
            return chunks;
        }



        public static string RemoveLinks(string s, string version)
        {
            //string newS = "";

            while (true)
            {
                int index = s.IndexOf("<a href=\"/wiki/");

                if (index == -1)
                {
                    index = s.IndexOf("<a href=\"/w/");
                }

                if (index != -1)
                {
                    string before = s.Substring(0, index);
                    string after = s.Substring(index + 1);
                    int index2 = after.IndexOf("</a>");

                    if (index2 != -1)
                    {
                        string inner = after.Substring(0, index2);
                        inner = inner.Replace("w", "$w$");
                        inner = inner.Replace("/$w$iki/", "https://"+version+".wiktionary.org/$w$iki/");

                        string after_ = after.Substring(index2 + 4);
                        string text = inner.Substring(inner.IndexOf(">") + 1);

                        if (text.Contains(" ") || text.Contains("-"))
                        {
                            s = before+ "<a target=\"_blank\"" + inner.Substring(1).Trim(' ') + "</a>" + after_;
                            //Console.WriteLine(s);
                        }
                        else
                        {
                            s = before + text + after_;
                        }
                    }
                    else
                    {
                        index2 = after.IndexOf(">");
                        string inner = after.Substring(0, index2);
                        string after_ = after.Substring(index2 + 1);
                        s = before + after_;
                    }


                }
                else
                {
                    break;
                }
            }

            s = s.Replace("<li class=\"mw-empty-elt\"></li>", "");
            s = s.Replace("$w$", "w");

            return s;
        }

        public static List<Chunk> SeparateByHeaders(string s)
        {
            List<Chunk> res = new List<Chunk>();
            string[] headers = new string[] { "<h2>", "<h3>", "<h4>", "<h5>" };
            string[] endings = new string[] { "</h2>", "</h3>", "</h4>", "</h5>" };

            while (true)
            {
                bool found = false;
                for (int i = 0; i < headers.Length; i++)
                {
                    int index = s.IndexOf(headers[i]);
                    if (index == 0)
                    {
                        int afterIndex = s.IndexOf(endings[i]) + endings[i].Length;
                        string content = s.Substring(0, afterIndex);
                        s = s.Substring(afterIndex);
                        Chunk chunk = new Chunk(headers[i], content);
                        res.Add(chunk);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    int minIndex = int.MaxValue;
                    for (int i = 0; i < headers.Length; i++)
                    {
                        int index = s.IndexOf(headers[i]);
                        if (index != -1)
                        {
                            if (index < minIndex)
                            {
                                minIndex = index;
                            }
                        }
                    }

                    if (minIndex != int.MaxValue)
                    {
                        if (res.Count > 0)
                        {
                            res[res.Count - 1].content += s.Substring(0, minIndex);
                        }
                        s = s.Substring(minIndex);
                        //Console.WriteLine();
                        found = true;
                    }
                }

                if (!found)
                {
                    break;
                }
            }

            if (res.Count > 0)
            {
                res[res.Count - 1].content += s;
            }
            //var stuff = SplitAndKeep(s, headers);

            return res;
        }

    }
}
