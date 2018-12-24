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
using System.Windows.Shapes;

namespace Watt
{
    /// <summary>
    /// Interaktionslogik für vwPlayer.xaml
    /// </summary>
    public partial class vwPlayer : Window
    {
        internal string PlayerAName;
        internal string PlayerBName;
        internal bool Reset;

        public vwPlayer()
        {
            InitializeComponent();
        }

        private void TouchOK(object sender, RoutedEventArgs e)
        {
            Reset = false;
            PlayerAName = PlayerA.Text;
            PlayerBName = PlayerB.Text;
            Close();
        }

        private void TouchReset(object sender, RoutedEventArgs e)
        {
            Reset = true;
            PlayerAName = PlayerA.Text;
            PlayerBName = PlayerB.Text;
            Close();
        }

    }
}
