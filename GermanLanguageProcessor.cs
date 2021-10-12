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
    public class GermanLanguageProcessor : GenericLanguageProcessor
    {
        public GermanLanguageProcessor(int count) : base(count, "German", "de")
        {

        }

        public override void MakeJson()
        {
            WordList nat = WordlistConstructor.CreateFromTxt(this.language + ".txt", ' ', 0, 0, 1000000000, true);
            WordList lemmas = WordlistConstructor.CreateFromTxt(this.language + "_lemmas.txt", '|', 0, 0, 10000000, true);


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

                if (nn.ContainsKey(x.word.ToLower()))
                {
                    f1 = nn[x.word.ToLower()];
                }

                if (nn.ContainsKey(y.word))
                {
                    f2 = nn[y.word];
                }

                if (nn.ContainsKey(y.word.ToLower()))
                {
                    f2 = nn[y.word.ToLower()];
                }

                return f1.CompareTo(f2);
            });


            string rusLang = JsonConvert.SerializeObject(lemmas, Formatting.Indented);
            File.WriteAllText("data/lang/" + language + ".json", rusLang, Encoding.UTF8);

        }

    }
}
