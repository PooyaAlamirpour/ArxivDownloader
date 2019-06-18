using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARXIVDownloader.Handler
{
    class FormHandler
    {
        private readonly Form1 form;
        
        public FormHandler(Form1 form)
        {
            this.form = form;
        }

        private void SomeMethodDoingStuffWithText()
        {
            string firstName = form.LogPanel;
            form.LogPanel = "some name";
        }
    }
}
