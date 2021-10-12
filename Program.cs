using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;



namespace Lang
{
    class Program
    {
        /*
        static void Main(string[] args)
        {
            //LanguageProcessor rlp = new RussianLanguageProcessor(1000000);
            //rlp.CollectData();
           .//rlp.ParseData();
            //rlp.ImportAndroid("C:\\Users\\Einhorn\\AndroidStudioProjects\\MyApplication3\\app\\src\\main\\assets\\");

            //LanguageProcessor rlp = new RussianLanguageProcessor(100000);
            //rlp.ParseData();
            //rlp.ImportAndroid("C:\\Users\\Einhorn\\AndroidStudioProjects\\MyApplication3\\app\\src\\main\\assets\\");
        }
        */

        static void Main(string[] args)
        {
            LanguageProcessor glp = new GenericLanguageProcessor(200000, "English", "en");
            //glp.ParseSimpleEnglish(new BasicWord("change"));
            //glp.ParseData();
            //glp.ExportOne("change");
            //glp.ExportOne("second");
            glp.ExportHTML();

            //glp.ExportOne("похоже");

            //RussianLanguageProcessor rlp = new RussianLanguageProcessor(1000000);
            //rlp.ExportDictionary();

            //LanguageProcessor glp2 = new GenericLanguageProcessor(200000, "German", "de");
            //glp2.ExportHTML();

            /*
            LanguageProcessor glp3 = new GenericLanguageProcessor(200000, "French", "fr");
            glp3.ExportHTML();

            LanguageProcessor glp4 = new GenericLanguageProcessor(200000, "Dutch", "nl");
            glp4.ExportHTML();

            LanguageProcessor glp5 = new GenericLanguageProcessor(200000, "English", "en");
            glp5.ExportHTML();
            */


            //LanguageProcessor glp2 = new GenericLanguageProcessor(100000, "English", "en");




            //LanguageProcessor glp2 = new GermanLanguageProcessor(100000);
            //LanguageProcessor glp2 = new GenericLanguageProcessor(1000, "Chinese", "zh");
            //LanguageProcessor glp = new RussianLanguageProcessor(10000000);
            //Console.WriteLine(glp.Test("<a href=\"smth\" title=\"делать большие глаза\">де́лать больши́е глаза́</a> делать большие глаза"));
            //glp.ExportDictionary();麵包


            //glp.ParseData();
            //glp.ExportChineseSounds("那兒");
            //glp.ExportChineseSounds("麵包");//我兄出來 以為
            //glp.ExportChineseSounds("我");
            //glp.ExportChineseSounds("出來");

            //glp2.ExportOne("weiß");
            //glp2.ParseData();

            //glp2.ParseData();
            //glp2.ExportOne("maar");
            //glp.ExportHTML();

            //glp.ExportOne("она");
            //glp.ParseData();
            //glp.ExportHTML();
            //glp.ExportDictionary();
            //glp2.ParseData();
            //glp2.ExportHTML();

            //LemmasGrabber(glp);
            //glp.ParseData();
            //glp.ExportHTML();
            //glp.grab();

            //glp3.ParseData();
            //glp2.ParseData();
            //glp2.ExportHTML();

            //glp2.ExportHTML();

            //glp2.ExportOne("hebben");
            //glp2.ExportOne("zien");
            //glp2.ExportOne("mooi");
            //glp2.ExportOne("vrouw");

            //glp2.ParseData();
            //glp2.ExportHTML();
            //glp2.ExportOne("有");
            //glp.ExportDictionary();

            //glp.ExportDictionary();
            //glp3.ParseData();


            //glp3.ExportHTML();
            //glp2.ExportHTML();

            /*
            List<LanguageProcessor> mainLangs = new List<LanguageProcessor>();
            mainLangs.Add(new GenericLanguageProcessor(100000, "English", "en"));
            mainLangs.Add(new GenericLanguageProcessor(100000, "Spanish", "es"));
            mainLangs.Add(new GenericLanguageProcessor(100000, "German", "de"));
            mainLangs.Add(new GenericLanguageProcessor(100000, "French", "fr"));
            mainLangs.Add(new GenericLanguageProcessor(100000, "Russian", "ru"));
            mainLangs.Add(new GenericLanguageProcessor(100000, "Portuguese", "pt"));
            mainLangs.Add(new GenericLanguageProcessor(100000, "Italian", "it"));
            mainLangs.Add(new GenericLanguageProcessor(100000, "Dutch", "nl"));
            mainLangs.Add(new GenericLanguageProcessor(100000, "Chinese", "zh"));
            */

            //List<LanguageProcessor> secondaryLangs = new List<LanguageProcessor>();
            //secondaryLangs.Add(new GenericLanguageProcessor(100000, "Chinese", "zh"));
            //secondaryLangs.Add(new GenericLanguageProcessor(100000, "Japanese", "ja"));
            //secondaryLangs.Add(new GenericLanguageProcessor(100000, "Korean", "ko"));
            //secondaryLangs.Add(new GenericLanguageProcessor(100000, "Arabic", "ar"));
            //secondaryLangs.Add(new GenericLanguageProcessor(100000, "Hindi", "hi"));

            //LanguageProcessor.MakeDictionary(mainLangs);

            /*
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Polish", "pl"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Bulgarian", "bg"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Serbo-Croatian", "sh"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Slovene", "sl"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Afrikaans", "af"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Czech", "cs"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Danish", "da"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Greek", "el"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Persian", "fa"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Finnish", "fi"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Hebrew", "he"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Hindi", "hi"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Hungaian", "hu"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Indonesian", "id"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Lithuanian", "lt"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Latvian", "lv"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Estonian", "es"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Malay", "ms"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Norvegian", "no"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Romanian", "ro"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Slovak", "sk"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Romanian", "ro"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Turkish", "tk"));
            secondaryLangs.Add(new GenericLanguageProcessor(100000, "Macedonian", "mk"));
            */

            //for (int i = 0; i<langs.Count; i++) {
            //LanguageProcessor.MakeDictionary(langs[2], langs[1]);
            //}

            //LanguageProcessor.MakeDictionary(mainLangs[0], secondaryLangs[0], true);
            //LanguageProcessor.MakeDictionary(mainLangs);

            /*
            int cnt = 9;

            for (int i = 0; i < cnt; i++)
            {
                for (int j = i+1; j < cnt; j++)
                {
                    LanguageProcessor.MakeDictionary(mainLangs[i], mainLangs[j], false);
                }

                for (int j = 0; j < secondaryLangs.Count; j++)
                {
                    //LanguageProcessor.MakeDictionary(mainLangs[i], secondaryLangs[j]);
                }
            }
            */


            //rlp.ParseData();

            //GenericLanguageProcessor glp1 = new GermanLanguageProcessor(100000);
            //Console.WriteLine(glp1.Test("Liebe liebe"));
            //glp1.ParseData();

            //GenericLanguageProcessor glp2 = new GenericLanguageProcessor(100000, "Dutch", "nl");
            //glp2.CollectData();


            // GenericLanguageProcessor glp2 = new GenericLanguageProcessor(100000, "Spanish", "es");
            //glp2.CollectData();

            //GenericLanguageProcessor glp3 = new GenericLanguageProcessor(100000, "Portuguese", "pt");
            //glp3.CollectData();

            //GenericLanguageProcessor glp4 = new GenericLanguageProcessor(100000, "Italian", "it");
            //glp4.CollectData();


            //RussianLanguageProcessor glp = new RussianLanguageProcessor(100000000);
            //glp.ExportFileLocations("data\\lang\\RussianAndroid\\");

            //glp.ParseData();
            //glp.ExportDefinitionWordlist(null);

            //GenericLanguageProcessor glp = new GenericLanguageProcessor(1000000, "French", "fr");
            //glp.ParseData();

            //GenericLanguageProcessor glp = new GenericLanguageProcessor(100000, "Swedish", "sv");
            //glp.ExportDefinitionWordlist(null);

            //DutchLanguageProcessor dlp = new DutchLanguageProcessor(1000);
            //dlp.ParseData();
            //dlp.ExportHTML();

            /*
            string line = "<span id=\".D0.9C.D0.BE.D1.80.D1.84.D0.BE.D0.BB.D0.BE.D0.B3.D0.B8.D1.87.D0.B5.D1.81.D0.BA.D0.B8.D0.B5_.D0.B8_.D1.81.D0.B8.D0.BD.D1.82.D0.B0.D0.BA.D1.81.D0.B8.D1.87.D0.B5.D1.81.D0.BA.D0.B8.D0.B5_.D1.81.D0.B2.D0.BE.D0.B9.D1.81.D1.82.D0.B2.D0.B0\"></span><span class=\"mw-headline\" id=\"Морфологические_и_синтаксические_свойства\"><hr></span>";
            LanguageProcessor rlp = new RussianLanguageProcessor(1000000);
            string dic = File.ReadAllText(rlp.GetDir() + "html//dictionary.txt");
            TextTranformer tt = new TextTranformer(dic);

            string conv = tt.Convert(line, false);

            Console.WriteLine(line);
            Console.WriteLine(conv);
            */

            //WiktionaryCollector.Collect2("buiten", "nl", "ru");

            //dlp.ImportAndroid("data\\lang\\RussianAndroid\\");
            //dlp.ExportDefinitionWordlist("data\\lang\\RussianAndroid\\");
            //dlp.ExportFileLocations("data\\lang\\RussianAndroid\\");

            //RussianLanguageProcessor dlp = new RussianLanguageProcessor(10000000);
            //dlp.MakeParticiples();
            //dlp.CollectData();
            //dlp.MakeParticiples();
            //dlp.ImportAndroid("data\\lang\\RussianAndroid\\");
            //dlp.ImportAndroid("data\\lang\\RussianAndroid\\");
            //dlp.ExportDictionary();
            //dlp.ExportHTML();
            //dlp.ExportDefinitionWordlist();

            //LanguageProcessor dlp = new DutchLanguageProcessor(1000000);
            //dlp.ExportDefinitionWordlist(null);

            //anguageProcessor rlp = new RussianLanguageProcessor(10000000);
            //rlp.ParseData();

            //rlp.ExportDictionary();
            //rlp.ParseData();
            //rlp.ExportDefinitionWordlist();

            //rlp.ExportDictionary();
            //rlp.ExportHTML();
            //rlp.ExportDefinitionWordlist();

            //rlp.ImportAndroid("data\\lang\\RussianAndroid\\");

            //LanguageProcessor rlp2 = new RussianLanguageProcessor(100000);
            //rlp.CollectData();
            //rlp.MakeJson();
            //rlp2.ParseData();
            //rlp.ImportAndroid("data\\lang\\RussianAndroid\\");

            //LanguageProcessor.MakeDB("E:\\projects\\Lang\\Lang\\bin\\Debug\\data\\lang\\Russian\\html", "E:\\projects\\Lang\\Lang\\bin\\Debug\\data\\lang\\Russian\\db");
        }
    }
}



//чтобы
//усовершенствовать
//кабан
//правильно
//звонок
//шалун



/*
\\trenintest.fintech.ru
 * */

