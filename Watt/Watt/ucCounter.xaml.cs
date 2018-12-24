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
    /// Interaktionslogik für ucCounter.xaml
    /// </summary>
    public partial class ucCounter : UserControl
    {

        private int _count2 = 0;
        private int _count3 = 0;

        public ucCounter()
        {
            InitializeComponent();
        }

        public void ResetCount()
        {
            _count2 = 0;
            _count3 = 0;
            Refresh();
        }

        public void AddCount(int count)
        {
            if (!Check(count)) return;
            if (Math.Abs(count) == 2)
            {
                if (count < 0 && _count2 < 2) return;
                _count2 += count;
            }
            if (Math.Abs(count) == 3)
            {
                if (count < 0 && _count3 < 3) return;
                _count3 += count;
            }
            Refresh();
        }

        private bool Check(int count)
        {
            if (_count2 + _count3 + count < 0) return false;
            if (_count2 + _count3 + count > 14) return false;
            return true;
        }

        private void Refresh()
        {
            this.count2.DrawTicks(_count2);
            this.count3.DrawTicks(_count3);
            this.counter.Text = (_count2 + _count3).ToString();
        }

    }
}
