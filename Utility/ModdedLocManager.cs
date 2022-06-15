using I2.Loc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Morbs.Utility
{
    public static class ModdedLocManager
    {
        public static bool termsRegistered = false;
        private static List<String[]> terms = new List<String[]>();


        public static void AddTerm(string key, string localised)
        {
            var s = new String[] { key, localised };
            terms.Add(s);
        }

        public static void AddTermsFromCSV()
        {
            string[] csv = File.ReadAllLines("BepinEx/plugins/assets/loc.tsv");
           
            foreach(string line in csv)
            {
                string[] values = Regex.Split(line, "\t");
                AddTerm(values[0], values[1]);
            }

        }

        public static void RegisterTerms()
        {

            foreach (String[] term in terms)
            {
                String key = term[0];
                if (key == "Keys") continue;
                String[] values = new string[term.Length - 1];
                for (int i = 1; i < term.Length; i++)
                {
                    if (term[i] != "")
                        values[i - 1] = term[i];
                    else
                        values[i - 1] = null;
                }
                LocalizationManager.Sources[0].AddTerm(key).Languages = values;
            }

        }
    }
}
