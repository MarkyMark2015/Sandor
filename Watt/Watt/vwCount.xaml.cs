using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Watt
{
    /// <summary>
    /// Interaktionslogik für vwCount.xaml
    /// </summary>
    public partial class vwCount : Window
    {

        internal int Count;

        public vwCount()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            Count = int.Parse(btn.Tag.ToString());
            this.Close();
        }

        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            Count = int.Parse(btn.Tag.ToString());
            this.Close();
        }
    }
}
