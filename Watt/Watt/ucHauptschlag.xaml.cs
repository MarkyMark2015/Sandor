using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Watt
{
    /// <summary>
    /// Interaktionslogik für Hauptschlag.xaml
    /// </summary>
    public partial class ucHauptschlag : UserControl
    {
        public ucHauptschlag()
        {
            InitializeComponent();
        }

        internal void SetHauptschlag(string v)
        {
            if (string.IsNullOrEmpty(v)) v="XX";
            var fileName = $@"C:\Users\mts20\source\repos\Watt\Watt\bin\Release\Res\Blatt\{v}.png";
            pic.Source = new BitmapImage(new Uri(fileName));
        }
    }
}
