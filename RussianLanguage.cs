using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Lang
{
    public class Language
    {
        public List<RussianWord> words = new List<RussianWord>();

        public Language(string name, string filter=null)
        {
            string[] _files = Directory.GetFiles("data\\lang\\"+name+"\\json");

            List<string> files = new List<string>();

            for (int i = 0; i<_files.Length; i++) {
                if (filter != null)
                {
                    if (_files[i].Contains("\\"+filter+".json"))
                    {
                        files.Add(_files[i]);
                    }
                }
                else
                {
                    files.Add(_files[i]);
                }
            }



            foreach (string file in files)
            {
                string cont = File.ReadAllText(file);
                JsonSerializerSettings settings = new JsonSerializerSettings();
                try
                {
                    RussianWord rw = JsonConvert.DeserializeObject<RussianWord>(cont, settings);
                    words.Add(rw);
                    Console.WriteLine(rw.spelling);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Can't do"+file);
                }
            }

            words.Sort(delegate (RussianWord x, RussianWord y)
            {
                int xf = x.frequencyRank;
                int yf = y.frequencyRank;

                if (xf <= 1)
                {
                    xf = 1000000;
                }

                if (yf <= 1)
                {
                    yf = 1000000;
                }

                if (x.spelling.Equals("определенный"))
                {
                    xf = 1;
                }

                if (y.spelling.Equals("определенный"))
                {
                    yf = 1;
                }

                return xf.CompareTo(yf);
            });

            Console.WriteLine(words);
        }


       
    }
}



/*
I realize that the Chinese version lacks details
but it's all because the wiktionary for Chinese is just a mess
it contains 
1) traditional and simplified hanzi versions of phrases
2) phrases and definitions of words in different dialects
3) examples in **Classical Chinese** (!). So yeah, fragments from texts written like 1500-2000 years ago
4) archaic and dead definitions of words
5) different transcriptions 
so previously I decided to just upload the data from a less fancy dictionary

but now I cleaned it up a little bit and uploaded the wiktionary content eventually cause it's just better for understanding the words to have a lot of information
take a look at this version. It's very overwhelming but honestly I think it's better to learn with this than with simple list of definitions. You kinda feel safer because you know that the information you need will more likely be there. Also, it's always been the spirit of this project that there is just a lot of information on each word so you learn deeply rather than widely. With small list of definitions it doesn't stand out the well. If you don't like it I can revert to the previous version without any problem
 * */


/*
also
I realize that the Chinese version lacks details
but it's all because the wiktionary for Chinese is just a mess
it contains 
1) traditional and simplified hanzi versions of phrases
2) phrases and definitions of words in different dialects
3) examples in **Classical Chinese** (!). So yeah, fragments from texts written like 1500-2000 years ago
4) archaic and dead definitions of words
5) different transcriptions 
so previously I decided to just upload the data from a less fancy dictionary

but now I cleaned it up a little bit and uploaded the wiktionary content eventually cause it's just better for understanding the words to have a lot of information
take a look at this version. It's very overwhelming but honestly I think it's better to learn with this than with simple list of definitions. If you don't like it I can revert to the previous version without any problem

 * 
honestly, I've spent two whole days of making the Chinese wiktionary content look nice and it's driving me crazy. There are so many little things that screw up the overall look of the layout. Also it takes so much take to update all the articles (because there are >140000 of them so the amount of text is just insane). Can we please wait just a bit until I make it look nice enough? 
 * * */
