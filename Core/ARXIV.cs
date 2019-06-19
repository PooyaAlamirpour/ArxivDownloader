using ARXIVDownloader.Handler;
using ARXIVDownloader.Helper;
using ARXIVDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ARXIVDownloader.Core
{
    class ARXIV : Download, IARXIV
    {
        private Form1 form1;
        private string URL { get; set; }
        
        private bool _f_process = true;
        private string path { get; set; }
        public string content { get; set; }
        public int results { get; set; }
        public List<string> PDFList { get; set; }

        public ARXIV(Form1 form1)
        {
            // TODO: Complete member initialization
            this.form1 = form1;
            log = new Log(this.form1);
        }

        internal void Process(String input)
        {
            log.ShowIntro();

            if (Validation.isValid(URL: input))
            {
                int result = getResult(URL: input);
                log.ShowResult(message: result.ToString());
                if(result != 0)
                {
                    this.form1.UpdateDownloadButton("Download " + result.ToString(), true);
                }
                else
                {
                    this.form1.UpdateDownloadButton("Download 0", false);
                }
            }
            else
            {
                log.Show("The entered URL is not valid. Press any key to exit.");
            }

        }

        internal int getResult(string URL)
        {
            this.URL = URL;
            if (URL.Contains("&size="))
            {
                String tmpURL = URL.Replace("&size=", "@");
                String[] splitedURL = tmpURL.Split('&');
                URL = "";
                foreach (String item in splitedURL)
                {
                    String tmpItem = "";
                    if(item.Contains("@"))
                    {
                        String[] splitedItem = item.Split('@');
                        tmpItem = splitedItem[0] + "&size=200";
                    }
                    else
                    {
                        tmpItem = item;
                    }
                    URL += tmpItem + "&";
                }
                
            }
            else
            {
                URL += "&size=200";
            }
            this.content = LoadPage(URL);
            String strResult = "0";
            if (!content.Equals("NaN"))
            {
                strResult = getSpecialPhrase(Content: content, From: "Showing", To: "results");
            }
            this.results = Convert.ToInt16(strResult.Replace(",", "").Replace(" ", ""));
            return results;
        }

        internal List<string> CollectAllAvailableLinks()
        {
            path = this.form1.getFolderPath;
            if(!path.Equals("NaN"))
            {
                if(Directory.Exists(path))
                {
                    this.form1.setMaximumTotalProgressbar(results);
                    _f_process = true;
                    PDFList = new List<String>();
                    this.form1.UpdateDownloadButton("Downloading...", false);
                    this.log.Show("Downloading start...");
                    if (results > 200)
                    {
                        CollectLinksInSinglePage(this.content);
                        int numPages = (results / 200) + 1;
                        for (int i = 1; i < numPages; i++)
                        {
                            if (!_f_process) break;
                            this.content = LoadPage(URL: nextPage(URL, i));
                            CollectLinksInSinglePage(this.content);
                        }
                    }
                    else
                    {
                        CollectLinksInSinglePage(this.content);
                    }
                    int length = PDFList.Count;
                    if (results > 0)
                    {
                        this.form1.UpdateDownloadButton("Download " + results.ToString(), true);
                    }
                    if (!_f_process)
                    {
                        this.log.Show("Downloading aborted.");
                    }
                    this.form1.updateTotalArticlesProgressbar(0);
                }
                else
                {
                    log.Show("Please choose valid path for saving articles.");
                }
            }
            else
            {
                log.Show("Please choose valid path for saving articles.");
            }
            return PDFList;
        }

        private string nextPage(string URL, int page)
        {
            if(URL.Contains("&start="))
            {
                string[] tmpSplitedURL = URL.Replace("&start=", "@").Split('@');
                URL = tmpSplitedURL[0] + "&start=" + (page * 200).ToString();
            }
            else
            {
                URL += "&start=" + (page * 200).ToString();
            }
            return URL;
        }

        private void CollectLinksInSinglePage(String input)
        {
            Regex regex = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase);
            Match match;
            for (match = regex.Match(input); match.Success; match = match.NextMatch())
            {
                if (!_f_process) break;
                foreach (Group group in match.Groups)
                {
                    if (!_f_process) break;
                    if (group.Value.ToLower().Contains("pdf") && !group.Value.ToLower().Contains("href="))
                    {
                        log.Show(">> " + group);
                        PDFList.Add(group.Value);
                        DownloadSignleFile(group.Value + ".pdf");
                        this.form1.updateTotalArticlesProgressbar(PDFList.Count);
                        Thread.Sleep(100);
                    }
                }
            }
        }

        public void MissionAborted()
        {
            _f_process = false;
        }

        Stopwatch sw = new Stopwatch();
        private async void DownloadSignleFile(String address)
        {
            string[] splitedAddress = address.Split('/');
            string articleId = splitedAddress[splitedAddress.Length - 1].Replace(".pdf", "");
            string tmpContent = LoadPage("https://arxiv.org/abs/" + articleId);
            string filename = getArticleName(tmpContent);
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add("User-Agent: Other");
                    client.DownloadProgressChanged += wc_DownloadProgressChanged;
                    client.DownloadFileCompleted += wc_DownloadFileCompleted;
                    await client.DownloadFileTaskAsync(new Uri(address), path + "\\" + filename.Replace(":", ",") + ".pdf");
                }
                catch (Exception ex)
                {
                    string tmp = ex.Message;
                }
            }
        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.form1.updateDownloadProgressbar(e.ProgressPercentage);
        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ((WebClient)sender).Dispose();
        }

        private string getArticleName(string input)
        {
            string articleName = "NaN";
            Regex regex = new Regex("<h1\\s*(.+?)\\s*>\\s*(.+?)\\s*</h1>", RegexOptions.IgnoreCase);
            Match match;
            for (match = regex.Match(input); match.Success; match = match.NextMatch())
            {
                if (!_f_process) break;
                foreach (Group group in match.Groups)
                {
                    if (!_f_process) break;
                    if(!group.Value.Contains("<h1"))
                    {
                        if(group.Value.Contains("<span class=\"descriptor\">Title:</span>"))
                        {
                            articleName = group.Value.Replace("<span class=\"descriptor\">Title:</span>", "");
                        }
                    }
                }
            }
            return articleName;
        }
        
    }
}
