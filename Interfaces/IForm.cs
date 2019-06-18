using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARXIVDownloader.Interfaces
{
    interface IForm
    {
        string LogPanel { get; set; }
        void UpdateDownloadButton(String label, bool Enable);
    }
}
