using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lang
{
    public class RussianWord : Word
    {
        public int stressIndex = -1;
        //public RussianGrammar grammar;

        public string extract;
        public string yospelling;


        public List<RussianDerivedWord> engForms = new List<RussianDerivedWord>();

        public List<RussianDerivedWord> forms = new List<RussianDerivedWord>();


        public static RussianDerivedWord ParseStress(string stuff)
        {
            string resStr = "";
            int stress = -1;
            for (int i = 0; i < stuff.Length; i++)
            {
                if ((stuff[i] >= 'а' && stuff[i] <= 'я') || (stuff[i] == 'ё' ) || (stuff[i] == '-') || (stuff[i] == ' ') || (stuff[i] == '(') || (stuff[i] == ')'))
                {
                    resStr += stuff[i];
                }
                else
                {
                    if (stress == -1) {
                        stress = i - 1;
                    }
                    else
                    {
                        RussianDerivedWord res = new RussianDerivedWord();
                        res.spelling = stuff;
                        return res;
                    }
                }
            }

            if (resStr.Length == 0)
            {
                return null;
            }

            RussianDerivedWord rdw = new RussianDerivedWord();
            rdw.spelling = resStr;
            rdw.stress = stress;

            //rdw.spelling = rdw.spelling.Replace(" ", "");

            return rdw;
        }
    }
}
