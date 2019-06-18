using ARXIVDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARXIVDownloader.Helper
{
    class Log : Console
    {

        public Log(Form1 form1)
        {
            this._form = form1;
        }

        internal void ShowResult(string message)
        {
            WriteLine("Number of result: " + message);
        }

        internal void Show(string message)
        {
            WriteLine(message);
        }

        internal void ShowIntro()
        {
            WriteLine("ARXIV Downloader v1.0\r\n");
        }

        internal void ShowExit()
        {
            WriteLine("\nPress any key to exit.");
        }

        internal void Clear(string phrase)
        {
            _form.LogPanel = _form.LogPanel.Replace(phrase, "");
        }
    }
}
