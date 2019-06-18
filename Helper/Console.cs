using ARXIVDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARXIVDownloader.Helper
{
    class Console
    {
        public Form1 _form;

        internal void WriteLine(string message)
        {
            _form.LogPanel = message;
        }
    }
}
