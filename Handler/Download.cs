using ARXIVDownloader.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ARXIVDownloader.Handler
{
    class Download : HTMLTagParser
    {
        public Log log;

        public string LoadPage(string URL)
        {
            log.Show("Please wait...");

            string html = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                log.Show(ex.Message);
                html = "NaN";
            }

            return html;
        }
    }
}
