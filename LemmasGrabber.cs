using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Lang
{
    public class LemmasGrabber
    {
        public static Dictionary<string, string> GrabLemmas(LanguageProcessor lang)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();

            HashSet<string> existing = new HashSet<string>();

            int ti = 0;
            string link = "https://en.wiktionary.org/wiki/Category:"+lang.language+"_lemmas";

            //string link = "https://en.wiktionary.org/w/index.php?title=Category:English_lemmas&pagefrom=-C-%0A-c-#mw-pages";

            while (true)
            {
                if (ti == 10)
                {
                    //break;
                }

                Console.WriteLine("Page "+link);
                existing.Add(link);

                string content = WiktionaryCollector.Collect(link);
                string stop = "<h2>Pages in category ";
                content = content.Substring(content.IndexOf(stop));
                int f = 0;

                if (ti == 107)
                {
                    Console.WriteLine();
                }

                int c = 0;
                int cnt = 2000;
                string last = null;

                string next = "";

                for(int i = 0; i<cnt; i++)
                {
                    if (content.IndexOf("<a href=\"") != -1)
                    {
                        content = content.Substring(content.IndexOf("<a href=\""));
                        string linkBody = content.Substring(0, content.IndexOf(">"));

                        string afterFirst = linkBody.Substring(linkBody.IndexOf("<a href=\"")+ ("<a href=\"").Length);
                        string afterSecond = linkBody.Substring(linkBody.IndexOf("title=\"") + ("title=\"").Length);

                        string linkItself = afterFirst.Substring(0, afterFirst.IndexOf("\""));
                        string label = afterSecond.Substring(0, afterSecond.IndexOf("\""));

                        if (label.Equals("Category:" + lang.language + " lemmas"))
                        {
                            if (i > 10)
                            {
                                if ((f > 0 && ti > 0) || (ti == 0 && f == 0)) {
                                    next = linkItself;
                                    break;
                                }
                                f++;
                            }
                        }
                        else
                        {
                            if (!res.ContainsKey(label)) {
                                res.Add(label, linkItself);

                                if (WordlistConstructor.Check(label) && !(label.Contains("ё"))) {
                                    last = label;
                                }
                            }

                            Console.WriteLine("\t\t\tAdded " + label);
                        }

                        content = content.Substring(content.IndexOf(">"));
                    }
                    else
                    {
                        break;
                    }
                }

                if (last == null)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                    //link = "https://en.wiktionary.org/"+next;


                    link = "https://en.wiktionary.org/w/index.php?title=Category:"+lang.language+"_lemmas&amp%3Bpagefrom="+last.ToUpper()+"%0A"+last+"&pagefrom="+last.ToUpper()+"%0A"+last+"#mw-pages";
                    //link = "https://en.wiktionary.org/wiki/Category:" + lang.language + "_lemmas&page_from="+next;
                }

                ti++;
            }

            string file = "E:\\projects\\Lang\\Lang\\bin\\Debug" + lang.language + "_lemmas.txt";

            List<string> list = new List<string>();
            foreach (var q in res)
            {
                list.Add(q.Key);
            }

            File.WriteAllLines(file, list.ToArray());

            return res;
        }
    }
}
