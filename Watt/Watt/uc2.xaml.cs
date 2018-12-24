using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Watt
{
    /// <summary>
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    public partial class uc2 : UserControl
    {
        public uc2()
        {
            InitializeComponent();
        }

        public void DrawTicks(int count)
        {
            myCanvas.Children.Clear();
            if (count>=2) DrawFigure("2Style");
            for (int i = 1; i < ((count)/2); i++) DrawTick(i);
        }

        void DrawTick(int id)
        {
            var p1 = new Path
            {
                Style = (Style)FindResource("TickStyle"),
                Margin = new Thickness(id * 3+5, 4, 0, 0)
            };
            myCanvas.Children.Add(p1);
        }

        void DrawFigure(string name)
        {
            var p1 = new Path
            {
                Style = (Style)FindResource(name)
            };
            myCanvas.Children.Add(p1);
        }

    }
}
