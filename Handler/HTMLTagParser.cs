using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARXIVDownloader.Handler
{
    class HTMLTagParser
    {
        public static string getSpecialPhrase(string Content, string From, string To)
        {
            int pFrom = Content.IndexOf(From) + From.Length;
            int pTo = Content.IndexOf(To);

            String result = Content.Substring(pFrom, pTo - pFrom);
            String[] splitedResult = result.Split(' ');
            return splitedResult[splitedResult.Length - 2];
        }
    }
}
