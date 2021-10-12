using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Threading;

namespace Lang
{
    public class WiktionaryCollector
    {
        public static string Collect(string link)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string res = "";
            var myClient = new WebClient();

            try
            {
                Stream response = myClient.OpenRead(link);
                using (StreamReader readStream = new StreamReader(response, Encoding.UTF8))
                {
                    res = readStream.ReadToEnd();
                }

                response.Close();

                return res;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool Collect(string word, string version)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string res = "";
            var myClient = new WebClient();

            try
            {
                string link = "https://" + version + ".wiktionary.org/wiki/" + word;
                Stream response = myClient.OpenRead(link);
                using (StreamReader readStream = new StreamReader(response, Encoding.UTF8))
                {
                    res = readStream.ReadToEnd();
                }

                response.Close();

                if ((word[0]+"").ToUpper().Equals(word[0]+"")) {
                    File.WriteAllText("data/" + version + "_wiktionary/_" + word + ".html", res);
                }
                else
                {
                    File.WriteAllText("data/" + version + "_wiktionary/" + word + ".html", res);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

       
    }
}
