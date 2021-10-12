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
    public partial class Parser
    {
        public class EnglishState
        {
            public bool active = false;
            public bool parseMeaning = false;
            public int reached0s = 0;
            public string pos = "";
            public bool examplesWritten = false;
            public bool wasSynonims = false;
            public bool parseEtymology = false;
            public string currentEtymology = "";
            public PartOfSpeech currentPs = PartOfSpeech.UNKNOWN;
        }

        public static Dictionary<string, string> languageCodes = null;

        public static void Fill()
        {
            string[] s = File.ReadAllLines("langscodes.txt");

            List<string> lst = new List<string>();
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = s[i].Replace("\t", "");
                if (!s[i].Contains(":") && s[i].Length>0 && !s[i].Contains("Index of"))
                {
                    lst.Add(s[i]);
                }
            }

            languageCodes = new Dictionary<string, string>();
            for (int i = 0; i<lst.Count/2; i++)
            {
                if (!languageCodes.ContainsKey(lst[2 * i + 1])) {
                    languageCodes.Add(lst[2 * i + 1], lst[2 * i]);
                }
            }

            languageCodes.Add("sh65", "Serbo-Croatian");



            languageCodes.Add("sh1", "Abinomn");
            languageCodes.Add("sh2", "Albanian");
            languageCodes.Add("sh3", "Arigidi");
            languageCodes.Add("sh4", "Bagirmi");
            languageCodes.Add("sh5", "Bikol_Central");
            languageCodes.Add("sh6", "Bonggo");
            languageCodes.Add("sh7", "Caló");
            languageCodes.Add("sh8", "Chinese");
            languageCodes.Add("sh9", "Chinook_Jargon");
            languageCodes.Add("sh10", "Chuukese");
            languageCodes.Add("sh11", "Cimbrian");
            languageCodes.Add("sh12", "Danish");
            languageCodes.Add("sh13", "Faroese");
            languageCodes.Add("sh14", "Friulian");
            languageCodes.Add("sh15", "Gaikundi");
            languageCodes.Add("sh16", "Galician");
            languageCodes.Add("sh17", "German_Low_German");
            languageCodes.Add("sh18", "Gothic");
            languageCodes.Add("sh19", "Icelandic");
            languageCodes.Add("sh20", "Istriot");
            languageCodes.Add("sh21", "Kurdish");
            languageCodes.Add("sh22", "Ladin");
            languageCodes.Add("sh23", "Latvian");
            languageCodes.Add("sh24", "Ligurian");
            languageCodes.Add("sh25", "Lithuanian");
            languageCodes.Add("sh26", "Luxembourgish");
            languageCodes.Add("sh27", "Miskito");
            languageCodes.Add("sh28", "Norman");
            languageCodes.Add("sh29", "North_Frisian");
            languageCodes.Add("sh30", "Northern_Sami");
            languageCodes.Add("sh31", "Norwegian_Bokmål");
            languageCodes.Add("sh32", "Norwegian_Nynorsk");
            languageCodes.Add("sh33", "Occitan");
            languageCodes.Add("sh34", "Old_Dutch");
            languageCodes.Add("sh35", "Old_English");
            languageCodes.Add("sh36", "Old_High_German");
            languageCodes.Add("sh37", "Old_Norse");
            languageCodes.Add("sh38", "Old_Occitan");
            languageCodes.Add("sh39", "Old_Saxon");
            languageCodes.Add("sh40", "Spanish");
            languageCodes.Add("sh41", "Sranan_Tongo");
            languageCodes.Add("sh43", "Tagalog");
            languageCodes.Add("sh44", "Tarpia");
            languageCodes.Add("sh45", "Tok_Pisin");
            languageCodes.Add("sh46", "Torres_Strait_Creole");
            languageCodes.Add("sh47", "Venetian");
            languageCodes.Add("sh48", "Vietnamese");
            languageCodes.Add("sh49", "Volapük");
            languageCodes.Add("sh50", "Welsh");
            languageCodes.Add("sh51", "Westrobothnian");
            languageCodes.Add("sh52", "Wik-Mungkan");
            languageCodes.Add("sh53", "Wolof");
            languageCodes.Add("sh54", "Yola");
            languageCodes.Add("sh55", "Zealandic");
            languageCodes.Add("sh56", "Finnish");
            languageCodes.Add("sh57", "Esperanto");
            languageCodes.Add("sh58", "Fala");
            languageCodes.Add("sh59", "Bulgarian");
            languageCodes.Add("sh60", "Ukrainian");
            languageCodes.Add("sh61", "Belarusian");
            languageCodes.Add("sh62", "Polish");
            languageCodes.Add("sh63", "Serbian");
            languageCodes.Add("sh64", "Mongol");
            languageCodes.Add("sh66", "Translingual");
            languageCodes.Add("sh67", "Rusyn");
            languageCodes.Add("sh68", "Tuvan");
            languageCodes.Add("sh69", "Dutch_Low_Saxon");
            languageCodes.Add("sh70", "Estonian");
            languageCodes.Add("sh71", "East_Central_German");
            languageCodes.Add("sh72", "Ewe");
            languageCodes.Add("sh73", "Limburgish");
            languageCodes.Add("sh74", "Hungarian");
            languageCodes.Add("sh75", "Tocharian");
            languageCodes.Add("sh76", "Low_German");
            languageCodes.Add("sh77", "Gobasi");
            languageCodes.Add("sh78", "Hawaiian");
            languageCodes.Add("sh79", "Hlai");
            languageCodes.Add("sh80", "Maori");
            languageCodes.Add("sh81", "Samo");
            languageCodes.Add("sh82", "Middle_High_German");
            languageCodes.Add("sh83", "Saterland_Frisian");
            languageCodes.Add("sh84", "Eastern_Huasteca_Nahuatl");
            languageCodes.Add("sh85", "Mauritian_Creole");
            languageCodes.Add("sh86", "Tundra_Nenets");

            languageCodes.Add("sh87", "Fwâi");
            languageCodes.Add("sh88", "Talur");
            languageCodes.Add("sh89", "Ido");
            languageCodes.Add("sh90", "Emilian");
            languageCodes.Add("sh91", "East_Damar");
            languageCodes.Add("sh92", "Slovene");
            languageCodes.Add("sh93", "Fijian");

            languageCodes.Add("sh94", "Taparita");
            languageCodes.Add("sh95", "Xhosa");
            languageCodes.Add("sh96", "Zulu");
            languageCodes.Add("sh97", "Indonesian");
            languageCodes.Add("sh98", "Abkhaz");
            languageCodes.Add("sh99", "Adyghe");
            languageCodes.Add("sh101", "Afrikaans");
            languageCodes.Add("sh102", "Akan");
            languageCodes.Add("sh103", "Albanian");
            languageCodes.Add("sh104", "American Sign Language");
            languageCodes.Add("sh105", "Amharic");
            languageCodes.Add("sh106", "Arabic");
            languageCodes.Add("sh107", "Aragonese");
            languageCodes.Add("sh108", "Aramaic");
            languageCodes.Add("sh109", "Armenian");
            languageCodes.Add("sh110", "Assamese");


            languageCodes.Add("sh111", "Votic");
            languageCodes.Add("sh112", "Tapino");

        }

        public static void ExploreEnglish(HtmlNode node, int level, EnglishState state, Word result, string targetLanguage)
        {
            Word lastWord = result;

            string[] wordGroups = new string[] { "Verb", "Adjective", "Noun", "Adjective", "Pronoun", "Preposition" };

            if (languageCodes == null)
            {
                Fill();
            }
            /*
            Dictionary<string, string> languageCodes = new Dictionary<string, string>();
            languageCodes.Add("de", "German");
            languageCodes.Add("ru", "Russian");
            languageCodes.Add("nl", "Dutch");
            languageCodes.Add("en", "English");
            languageCodes.Add("oe", "Old_English");
            languageCodes.Add("af", "Afrikaans");
            languageCodes.Add("wf", "West_Frisian");
            languageCodes.Add("lu", "Luxembourgish");
            languageCodes.Add("me", "Middle_English");
            languageCodes.Add("med", "Middle_Dutch");
            languageCodes.Add("sg", "Scottish_Gaelic");
            languageCodes.Add("wfl", "West_Flemish");
            languageCodes.Add("jp", "Japanese");
            languageCodes.Add("lat", "Latin");
            languageCodes.Add("ch", "Mandarin");
            languageCodes.Add("fr", "French");
            languageCodes.Add("sv", "Swedish");


            languageCodes.Add("sh1", "Abinomn");
            languageCodes.Add("sh2", "Albanian");
            languageCodes.Add("sh3", "Arigidi");
            languageCodes.Add("sh4", "Bagirmi");
            languageCodes.Add("sh5", "Bikol_Central");
            languageCodes.Add("sh6", "Bonggo");
            languageCodes.Add("sh7", "Caló");
            languageCodes.Add("sh8", "Chinese");
            languageCodes.Add("sh9", "Chinook_Jargon");
            languageCodes.Add("sh10", "Chuukese");
            languageCodes.Add("sh11", "Cimbrian");
            languageCodes.Add("sh12", "Danish");
            languageCodes.Add("sh13", "Faroese");
            languageCodes.Add("sh14", "Friulian");
            languageCodes.Add("sh15", "Gaikundi");
            languageCodes.Add("sh16", "Galician");
            languageCodes.Add("sh17", "German_Low_German");
            languageCodes.Add("sh18", "Gothic");
            languageCodes.Add("sh19", "Icelandic");
            languageCodes.Add("sh20", "Istriot");
            languageCodes.Add("sh21", "Kurdish");
            languageCodes.Add("sh22", "Ladin");
            languageCodes.Add("sh23", "Latvian");
            languageCodes.Add("sh24", "Ligurian");
            languageCodes.Add("sh25", "Lithuanian");
            languageCodes.Add("sh26", "Luxembourgish");
            languageCodes.Add("sh27", "Miskito");
            languageCodes.Add("sh28", "Norman");
            languageCodes.Add("sh29", "North_Frisian");
            languageCodes.Add("sh30", "Northern_Sami");
            languageCodes.Add("sh31", "Norwegian_Bokmål");
            languageCodes.Add("sh32", "Norwegian_Nynorsk");
            languageCodes.Add("sh33", "Occitan");
            languageCodes.Add("sh34", "Old_Dutch");
            languageCodes.Add("sh35", "Old_English");
            languageCodes.Add("sh36", "Old_High_German");
            languageCodes.Add("sh37", "Old_Norse");
            languageCodes.Add("sh38", "Old_Occitan");
            languageCodes.Add("sh39", "Old_Saxon");
            languageCodes.Add("sh40", "Spanish");
            languageCodes.Add("sh41", "Sranan_Tongo");
            languageCodes.Add("sh43", "Tagalog");
            languageCodes.Add("sh44", "Tarpia");
            languageCodes.Add("sh45", "Tok_Pisin");
            languageCodes.Add("sh46", "Torres_Strait_Creole");
            languageCodes.Add("sh47", "Venetian");
            languageCodes.Add("sh48", "Vietnamese");
            languageCodes.Add("sh49", "Volapük");
            languageCodes.Add("sh50", "Welsh");
            languageCodes.Add("sh51", "Westrobothnian");
            languageCodes.Add("sh52", "Wik-Mungkan");
            languageCodes.Add("sh53", "Wolof");
            languageCodes.Add("sh54", "Yola");
            languageCodes.Add("sh55", "Zealandic");
            languageCodes.Add("sh56", "Finnish");
            languageCodes.Add("sh57", "Esperanto");
            languageCodes.Add("sh58", "Fala");
            languageCodes.Add("sh59", "Bulgarian");
            languageCodes.Add("sh60", "Ukrainian");
            languageCodes.Add("sh61", "Belarusian");
            languageCodes.Add("sh62", "Polish");
            languageCodes.Add("sh63", "Serbian");
            languageCodes.Add("sh64", "Mongol");
            languageCodes.Add("sh65", "Serbo-Croatian");
            languageCodes.Add("sh66", "Translingual");
            languageCodes.Add("sh67", "Rusyn");
            languageCodes.Add("sh68", "Tuvan");
            languageCodes.Add("sh69", "Dutch_Low_Saxon");
            languageCodes.Add("sh70", "Estonian");
            languageCodes.Add("sh71", "East_Central_German");
            languageCodes.Add("sh72", "Ewe");
            languageCodes.Add("sh73", "Limburgish");
            languageCodes.Add("sh74", "Hungarian");
            languageCodes.Add("sh75", "Tocharian");
            languageCodes.Add("sh76", "Low_German");
            languageCodes.Add("sh77", "Gobasi");
            languageCodes.Add("sh78", "Hawaiian");
            languageCodes.Add("sh79", "Hlai");
            languageCodes.Add("sh80", "Maori");
            languageCodes.Add("sh81", "Samo");
            languageCodes.Add("sh82", "Middle_High_German");
            languageCodes.Add("sh83", "Saterland_Frisian");
            languageCodes.Add("sh84", "Eastern_Huasteca_Nahuatl");
            languageCodes.Add("sh85", "Mauritian_Creole");
            languageCodes.Add("sh86", "Tundra_Nenets");

            languageCodes.Add("sh87", "Fwâi");
            languageCodes.Add("sh88", "Talur");
            languageCodes.Add("sh89", "Ido");
            languageCodes.Add("sh90", "Emilian");
            languageCodes.Add("sh91", "East_Damar");
            languageCodes.Add("sh92", "Slovene");
            languageCodes.Add("sh93", "Fijian");

            languageCodes.Add("sh94", "Taparita");
            languageCodes.Add("sh95", "Xhosa");
            languageCodes.Add("sh96", "Zulu");
            languageCodes.Add("sh97", "Indonesian");
            languageCodes.Add("sh98", "Abkhaz");
            languageCodes.Add("sh99", "Adyghe");
            languageCodes.Add("sh101", "Afrikaans");
            languageCodes.Add("sh102", "Akan");
            languageCodes.Add("sh103", "Albanian");
            languageCodes.Add("sh104", "American Sign Language");
            languageCodes.Add("sh105", "Amharic");
            languageCodes.Add("sh106", "Arabic");
            languageCodes.Add("sh107", "Aragonese");
            languageCodes.Add("sh108", "Aramaic");
            languageCodes.Add("sh109", "Armenian");
            languageCodes.Add("sh110", "Assamese");
            */

            string[] markers = new string[] { "Derived terms", "Anagrams", "Inflection", "Antonyms", "Synonyms", "Descendants", "Usage notes", "Conjugation", "Alternative forms", "See also", "Further reading", "Declension", "Navigation menu", "Categories", "Related terms", "Idioms", "References", "Coordinate terms" };

            string langName = languageCodes[targetLanguage];
            string nodeId = node.Id;

            if (nodeId.Equals("Swedish"))
            {
                Console.WriteLine();
            }

            foreach (var c in languageCodes.Keys)
            {

                if (nodeId.Equals(languageCodes[c]))
                {
                    if (c.Equals(targetLanguage))
                    {
                        state.active = true;
                    }
                    else
                    {
                        state.active = false;
                        state.parseMeaning = false;
                        state.parseEtymology = false;
                    }
                }
            }


            bool nextUp = state.active;

            string showText = node.InnerText.Replace("\n", "");
            showText = showText.Replace("[edit]", "");
            showText = showText.Replace("[change]", "");

            if (state.active)
            {
                if (true)
                {
                    bool pt = false;
                    if (showText.Equals("Verb"))
                    {
                        pt = true;
                        state.currentPs = PartOfSpeech.VERB;
                    }
                    if (showText.Equals("Noun") || showText.Equals("Numeral"))
                    {
                        pt = true;
                        state.currentPs = PartOfSpeech.NOUN;
                    }
                    if (showText.Equals("Adjective"))
                    {
                        pt = true;
                        state.currentPs = PartOfSpeech.ADJECTIVE;
                    }

                    if (showText.Equals("Pronoun"))
                    {
                        pt = true;
                        state.currentPs = PartOfSpeech.PRONOUN;
                    }

                    if (showText.Equals("Preposition"))
                    {
                        pt = true;
                        state.currentPs = PartOfSpeech.PRESPOSITION;
                    }

                    if (showText.Equals("Determiner") || showText.Equals("Article"))
                    {
                        pt = true;
                        state.currentPs = PartOfSpeech.DETERMINER;
                    }

                    if (showText.Equals("Conjunction"))
                    {
                        pt = true;
                        state.currentPs = PartOfSpeech.CONJUNCTION;
                    }

                    if (showText.Equals("Adverb"))
                    {
                        pt = true;
                        state.currentPs = PartOfSpeech.ADVERB;
                    }

                    if (showText.Equals("Particle"))
                    {
                        pt = true;
                        state.currentPs = PartOfSpeech.PARTICLE;
                    }

                    if (showText.Equals("Participle"))
                    {
                        pt = true;
                        state.currentPs = PartOfSpeech.PARTICIPLE;
                    }

                    if (showText.Equals("Interjection"))
                    {
                        pt = true;
                        state.currentPs = PartOfSpeech.INTERJECTION;
                    }

                    if (pt) {
                        state.pos = showText;
                        state.parseMeaning = true;
                        state.parseEtymology = false;
                        state.reached0s = 0;
                    }
                }
            }


            if (state.parseMeaning && state.reached0s > 2)
            {
                if (level == 1) {
                    if (showText.Equals(state.pos))
                    {

                    }
                    else
                    {
                        if (showText.Length > 0)
                        {
                            //if (result.meanDescription == null)
                            //{
                            //    result.meanDescription = showText;
                            //}
                            //else
                            {
                            }
                        }

                        if (showText.Length > 0) {
                            //Console.WriteLine("Show text: " + level + " " + showText);
                        }
                    }
                }

                /*
                if (level == 4 && !state.examplesWritten)
                {
                    if (state.wasSynonims)
                    {
                        state.wasSynonims = false;
                    }
                }

                if (level == 5 && !state.examplesWritten)
                {
                    if (showText.Contains("Synonyms"))
                    {
                        state.wasSynonims = true;
                    }
                    if (showText.Length>0 && ! state.wasSynonims) {
                        Meaning meaning = result.meanings[result.meanings.Count - 1];
                        meaning.AddExample(showText);
                    }
                }
                string sep = "&#8212;";
                if (level == 3 && showText.Contains(sep))
                {
                    if (showText.Length > 0)
                    {
                        string[] parts = showText.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
                        Meaning meaning = result.meanings[result.meanings.Count - 1];
                        meaning.exampleSentences.Add(new Tuple<string, string>(parts[0], parts[1]));
                        state.examplesWritten = true;
                        meaning.explanation = meaning.explanation.Replace(showText, "");
                    }
                }

                if (level == 3 && !showText.Contains(sep))
                {
                    state.examplesWritten = false;
                }
                */
            }


            if (markers.Contains(showText))
            {
                state.parseMeaning = false;
                state.parseEtymology = false;
                state.reached0s = 0;
            }

            if (showText.Contains("Retrieved from"))
            {
                state.parseMeaning = false;
                state.parseEtymology = false;
                state.reached0s = 0;
            }

            if (state.parseEtymology && level == 0 && state.reached0s==1)
            {
                //Console.WriteLine(state.reached0s+" Show text: " + level + " " + node.InnerText);
                if (!showText.Contains("Pronunciation")) {
                    state.currentEtymology = showText;
                }
            }

            if (state.parseEtymology && level == 0)
            {
                state.reached0s++;
            }

            if (state.active && showText.Contains("Etymology"))
            {
                state.parseEtymology = true;
                state.parseMeaning = false;
                state.reached0s = 0;
            }

            if (showText.Contains("Pronunciation"))
            {
                state.parseEtymology = false;
                state.reached0s = 0;
            }

            if ((state.parseMeaning) && !showText.Equals(state.pos))
            {
                if (level == 0)
                {
                    state.reached0s++;
                }
            }

            bool show = state.parseEtymology && state.reached0s > 0;
            if (false)
            {
                for (int i = 0; i < level; i++)
                {
                    Console.Write("\t");
                }

                string text = level+","+state.reached0s+" // "+node.Id + " -> " + node.InnerText.Replace("\n", "");
                string info = text;
                Console.WriteLine(info);
            }




            if (node.ChildNodes.Count > 0)
            {
                foreach (HtmlNode child in node.ChildNodes)
                {
                    ExploreEnglish(child, nextUp?level+1:level , state, result, targetLanguage);
                }
            }

        }
    }
}
