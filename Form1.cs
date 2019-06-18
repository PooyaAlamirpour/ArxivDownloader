using ARXIVDownloader.Core;
using ARXIVDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARXIVDownloader
{
    public partial class Form1 : Form, IForm
    {
        private ARXIV Arxiv;
        public Form1()
        {
            InitializeComponent();
            Arxiv = new ARXIV(this);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                txtLog.Clear();
                Task.Factory.StartNew(() => Arxiv.Process(txtLink.Text));
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            Task.Factory.StartNew(() => Arxiv.Process(txtLink.Text));
        }

        public string LogPanel
        {
            get
            {
                return txtLog.Text;
            }
            set
            {
                if(txtLog.InvokeRequired)
                {
                    txtLog.BeginInvoke(new MethodInvoker(delegate()
                        {
                            txtLog.Text += value + "\r\n";
                            txtLog.SelectionStart = txtLog.TextLength;
                            txtLog.ScrollToCaret();
                        }));
                }
            }
        }

        public void UpdateDownloadButton(string label, bool Enable)
        {
            if(btnDownload.InvokeRequired)
            {
                btnDownload.BeginInvoke(new MethodInvoker(delegate()
                    {
                        btnDownload.Text = label;
                        btnDownload.Enabled = Enable;
                    }));
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            int results = Convert.ToInt16(btnDownload.Text.Replace("Download ", ""));
            Task.Factory.StartNew(() => Arxiv.Download());
        }
        
    }
}
