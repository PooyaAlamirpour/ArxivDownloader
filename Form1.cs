using ARXIVDownloader.Core;
using ARXIVDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
                updateTotalArticlesProgressbar(0);
                Task.Factory.StartNew(() => Arxiv.Process(txtLink.Text));
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            updateTotalArticlesProgressbar(0);
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

        public void setMaximumTotalProgressbar(int max)
        {
            if (pbTotal.InvokeRequired)
            {
                pbTotal.BeginInvoke(new MethodInvoker(delegate()
                    {
                        pbTotal.Maximum = max;
                    }));
            }
        }

        internal void updateTotalArticlesProgressbar(int value)
        {
            if (pbTotal.InvokeRequired)
            {
                pbTotal.BeginInvoke(new MethodInvoker(delegate()
                {
                    pbTotal.Value = value;
                }));
            }
        }

        internal void updateDownloadProgressbar(int value)
        {
            if (pbDownload.InvokeRequired)
            {
                pbDownload.BeginInvoke(new MethodInvoker(delegate()
                {
                    pbDownload.Value = value;
                }));
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            int results = Convert.ToInt16(btnDownload.Text.Replace("Download ", ""));
            Task.Factory.StartNew(() =>
            {
                return Arxiv.CollectAllAvailableLinks();
            });
        }

        private void txtLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                Arxiv.MissionAborted();
            }
        }

        public String getFolderPath
        {
            get
            {
                string path = "NaN";
                if (!txtFolderPath.Text.Equals(""))
                {
                    path = txtFolderPath.Text;
                }
                return path;
            }
            set
            {
                
            }
            
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath))
            {
                txtFolderPath.Text = folderBrowserDialog1.SelectedPath;
            }
           
        }
    }
}
