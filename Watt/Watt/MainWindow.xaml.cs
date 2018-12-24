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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Watt
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ucCounterA(object sender, MouseButtonEventArgs e)
        {
            AddCount(PlayerA, true);
        }

        private void ucCounterB(object sender, MouseButtonEventArgs e)
        {
            AddCount(PlayerB, false);
        }

        void AddCount(ucCounter cnt, bool showLeft)
        {
            const int delta = 150;
            var wnd = new vwCount();
            wnd.Owner = null; // this;
            if (showLeft)
            {
                wnd.Left = this.Left + delta;
            }
            else
            {
                wnd.Left = this.Width - wnd.Width - delta;
            }
            wnd.Top = this.Top + (this.Height - wnd.Height) / 2;
            wnd.ShowDialog();
            cnt.AddCount(wnd.Count);
        }

        private void btnPlayer(object sender, MouseButtonEventArgs e)
        {
            var wnd = new vwPlayer();
            wnd.Owner = this;
            wnd.PlayerA.Text = PlayerAName.Text;
            wnd.PlayerB.Text = PlayerBName.Text;
            wnd.ShowDialog();
            if (wnd.Reset)
            {
                PlayerA.ResetCount();
                PlayerB.ResetCount();
                this.Hauptschlag.SetHauptschlag(null);
            }
            PlayerAName.Text = wnd.PlayerAName;
            PlayerBName.Text = wnd.PlayerBName;
        }

        private void btnHauptschlag(object sender, MouseButtonEventArgs e)
        {
            var wnd = new vwHauptschlagSelect();
            wnd.Owner = this;
            wnd.ShowDialog();
            this.Hauptschlag.SetHauptschlag(wnd.Result);
        }
    }
}
