using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using RaptorDB;
using Dapper;
using System.Text;

namespace SampleApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Hoot _hoot;
        DateTime _indextime;

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Words = " + _hoot.WordCount.ToString("#,#") + "\r\nDocuments = " + _hoot.DocumentCount.ToString("#,#"));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _hoot.Save();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (_hoot == null)
            {
                MessageBox.Show("hOOt not loaded");
                return;
            }

            listBox1.Items.Clear();
            DateTime dt = DateTime.Now;
            listBox1.BeginUpdate();

            foreach (var d in _hoot.FindDocumentFileNames(txtSearch.Text))
            {
                listBox1.Items.Add(d);
            }

            // Hier für DB-Einträge
            foreach (var d in _hoot.FindRows(txtSearch.Text))
            {
                listBox1.Items.Add(d);
            }
            listBox1.EndUpdate();
            lblStatus.Text = "Search = " + listBox1.Items.Count + " items, " + DateTime.Now.Subtract(dt).TotalSeconds + " s";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = txtWhere.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtWhere.Text = fbd.SelectedPath;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtIndexFolder.Text == "" || txtWhere.Text == "")
            {
                MessageBox.Show("Please supply the index storage folder and the where to start indexing from.");
                return;
            }

            btnStart.Enabled = false;
            btnStop.Enabled = true;
            if (_hoot == null)
                _hoot = new Hoot(Path.GetFullPath(txtIndexFolder.Text), "index", true);

            string[] files = Directory.GetFiles(txtWhere.Text, "*", SearchOption.AllDirectories);
            _indextime = DateTime.Now;
            backgroundWorker1.RunWorkerAsync(files);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Directory.GetCurrentDirectory();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtIndexFolder.Text = fbd.SelectedPath;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] files = e.Argument as string[];
            BackgroundWorker wrk = sender as BackgroundWorker;
            int i = 0;
            foreach (string fn in files)
            {
                if (wrk.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                backgroundWorker1.ReportProgress(1, fn);
                try
                {
                    if (_hoot.IsIndexed(fn) == false)
                    {
                        TextReader tf = new EPocalipse.IFilter.FilterReader(fn);
                        string s = "";
                        if (tf != null)
                            s = tf.ReadToEnd();

                        _hoot.Index(new myDoc(new FileInfo(fn), s), true);
                    }
                }
                catch { }
                i++;
                if (i > 1000)
                {
                    i = 0;
                    _hoot.Save();
                }
            }
            _hoot.Save();
            _hoot.OptimizeIndex();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblIndexer.Text = "" + e.UserState;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IndexReady();
        }

        private void IndexReady()
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            lblIndexer.Text = "Indexing done: " + DateTime.Now.Subtract(_indextime).TotalSeconds + " sec";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter) btnSearch_Click(null, null);
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Process.Start("" + listBox1.SelectedItem);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (txtIndexFolder.Text == "")
            {
                MessageBox.Show("Please supply the index storage folder.");
                return;
            }

            _hoot = new Hoot(Path.GetFullPath(txtIndexFolder.Text), "index", true);
            button1.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_hoot != null)
                _hoot.Shutdown();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _indextime = DateTime.Now;
            FillDataFromDb();
            _hoot.Index(10, "Anna");
            _hoot.Index(20, "Berta");
            _hoot.Index(30, "Gamma");
            _hoot.Index(40, "Delta");
            _hoot.Index(50, "Willi Weise");
            _hoot.Save();
            _hoot.OptimizeIndex();
            IndexReady();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }

        void FillDataFromDb()
        {
            using (var conn = new System.Data.SqlClient.SqlConnection("Server=DevDb4.sse.local;User=service;Password=service;Initial Catalog=ELS3_GARD;"))
            {
                StringBuilder sb = new StringBuilder();
                int count = 0;
                var results = conn.Query("select ort.Name OrtName, strasse.Name StrasseName, Strasse.StrasseId Id From Ort left join strasse on ort.ortId=strasse.ortid");
                foreach (var r in results)
                {
                    count++;
                    string value = r.OrtName + " " + r.StrasseName;
                    if (count % 1000 == 0) Console.WriteLine(count + " - " +r.Id + ":" + value);
                    sb.AppendLine($"{r.OrtName}:{r.StrasseName}");
                    // _hoot.Index(r.Id, value);
                }
                System.IO.File.WriteAllText(@"c:\temp\OrtStrasse.txt",sb.ToString());
            }
            Debug.Assert(false);
        }


    }
}
