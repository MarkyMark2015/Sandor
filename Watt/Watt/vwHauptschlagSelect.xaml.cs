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
    /// Interaktionslogik für vwHauptschlagSelect.xaml
    /// </summary>
    public partial class vwHauptschlagSelect : Window
    {

        private string _farbe;
        private string _wert;

        public string Result;

        public vwHauptschlagSelect()
        {
            InitializeComponent();
        }

        private void btnWert_Click(object sender, RoutedEventArgs e)
        {
            _wert = ((Button)sender).Tag.ToString();
            CloseOnOk();
        }

        private void CloseOnOk()
        {
            if (_farbe!=null && _wert!=null)
            {
                Result = _farbe + _wert;
                this.Close();
            }
        }


        private void ButtonFarbe(object sender, RoutedEventArgs e)
        {
            _farbe = ((Button)sender).Tag.ToString();
            CloseOnOk();
        }
    }

}
