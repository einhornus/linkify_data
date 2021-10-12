using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
namespace LanguageLib
{
    public class FormsGrabber
    {
        public static List<string> GetForms(List<HTMLExtractor.Chunk> englishChunks, string version)
        {
            for (int i = 0; i < englishChunks.Count; i++)
            {
                if (englishChunks[i].type == HTMLExtractor.TagType.Inflection || (englishChunks[i].type == HTMLExtractor.TagType.Declension) || englishChunks[i].type == HTMLExtractor.TagType.Conjugation)
                {
                    string v = englishChunks[i].content;

                    Console.WriteLine(v);
                }
            }

            return new List<string>();
        }
    }
}
*/


namespace Lang
{
    public class FormsGrabber
    {
        public static List<string> GetForms(List<HTMLExtractor.Chunk> englishChunks, string version)
        {


            List<string> res = new List<string>();

            for (int i = 0; i < englishChunks.Count; i++)
            {
                if (englishChunks[i].type == HTMLExtractor.TagType.Inflection || (englishChunks[i].type == HTMLExtractor.TagType.Declension) || englishChunks[i].type == HTMLExtractor.TagType.Conjugation)
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
                            string q = "lang=\"" + version + "\">";
                            int index = inner.IndexOf(q);


                            if (index != -1)
                            {
                                string _before = inner.Substring(0, index);
                                string _after = inner.Substring(index + 1);
                                int _index2 = _after.IndexOf("</span>");

                                if (_index2 != -1)
                                {
                                    string f = inner.Substring(index + q.Length, _index2 - q.Length + 1);

                                    if (!f.Contains('<'))
                                    {
                                        string[] exceptions = new string[] { "het", "dit", "wat", "dat", "iets", "niets", "alles" };
                                        if (!exceptions.Contains(f))
                                        {
                                            if (!res.Contains(f))
                                            {
                                                if (!f.Contains(" "))
                                                {
                                                    res.Add(f);
                                                }
                                            }
                                        }
                                    }
                                    inner = _after;
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



                        //Console.WriteLine(inner);
                    }

                    //break;
                }

                if (englishChunks[i].type == HTMLExtractor.TagType.Noun)
                {
                    string pluralIndicator = "Latn form-of lang-nl p-form-of\" lang=\"nl\">";
                    if (englishChunks[i].content.Contains(pluralIndicator))
                    {
                        string after = englishChunks[i].content.Substring(englishChunks[i].content.IndexOf(pluralIndicator) + pluralIndicator.Length);
                        string plural = after.Substring(0, after.IndexOf("<"));
                        res.Add(plural);
                    }


                    string dimIndicator = "Latn form-of lang-nl diminutive-form-of\" lang=\"nl\">";
                    if (englishChunks[i].content.Contains(dimIndicator))
                    {
                        string after = englishChunks[i].content.Substring(englishChunks[i].content.IndexOf(dimIndicator) + dimIndicator.Length);
                        string dim = after.Substring(0, after.IndexOf("<"));
                        res.Add(dim);
                    }
                }
            }


            if (res.Count > 0)
            {
                if (res[0].Equals("'k") || res[0].Equals("ik"))
                {
                    res = new List<string>();
                    return res;
                }

                if (res[0].Equals("jag"))
                {
                    res = new List<string>();
                    return res;
                }
            }

            return res;
        }
    }
}
