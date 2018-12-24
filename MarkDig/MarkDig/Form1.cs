using Markdig;
using Markdig.Extensions.Emoji;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarkDig
{

    // https://rawgit.com/fletcher/MultiMarkdown-6-Syntax-Guide/master/index.html
    // https://github.com/lunet-io/markdig/blob/master/readme.md
    // https://fletcherpenney.net/multimarkdown/

    public partial class Form1 : Form
    {
        private Timer _tim;

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _tim.Stop();
            _tim.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _tim = new Timer();
            _tim.Interval = 300;
            _tim.Tick += _tim_Tick;
            //this.Width = this.Width * 2;
            textBox1.Text = System.IO.File.ReadAllText("TextFile1.txt");
        }

        private void _tim_Tick(object sender, EventArgs e)
        {
            _tim.Stop();
            var pipe = new MarkdownPipelineBuilder().UseAdvancedExtensions();
            pipe = pipe.UseGridTables().UsePipeTables();
            pipe.Extensions.Add(new EmojiExtension(true));
            var pipeline = pipe.Build();
            var result = Markdown.ToHtml(textBox1.Text,pipeline);
            result =  AddBasicHtml(result);
            webBrowser1.DocumentText = result;
            //Console.WriteLine(result);   // prints: <p>This is a text with some <em>emphasis</em></p>
        }

        private string AddBasicHtml(string content)
        {
            string template = @"<html><head><style>$1</style></head><body>$2</body></html>";
            var help = template.Replace("$1", System.IO.File.ReadAllText("default.css"));
            return help.Replace("$2", content);
        }
    }
}
