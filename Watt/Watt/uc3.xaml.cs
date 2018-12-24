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
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    public partial class uc3 : UserControl
    {
        public uc3()
        {
            InitializeComponent();
        }

        public void DrawTicks(int count)
        {
            myCanvas.Children.Clear();
            if (count >= 3) DrawFigure("3Style");
            for (int i = 1; i < ((count) / 3); i++) DrawTick(i);
        }

        void DrawTick(int id)
        {
            var p1 = new Path
            {
                Style = (Style)FindResource("TickStyle"),
                Margin = new Thickness(id * 3+6, 4, 0, 0)
            };
            myCanvas.Children.Add(p1);
        }

        void DrawFigure(string name)
        {
            var p1 = new Path
            {
                Style = (Style)FindResource(name),
                Stretch= Stretch.Fill,
                Margin = new Thickness(0, 0, 0, 0),
                Width=28.0
            };
            myCanvas.Children.Add(p1);
        }

    }

}
