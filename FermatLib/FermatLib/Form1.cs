using System;
using System.Drawing;
using System.Windows.Forms;

namespace FermatLib
{
    // https://twitter.com/fermatslibrary/status/1036234798904299521


    public partial class Form1 : Form
    {
        // Parameter
        private double _zoom = 3 * PI; // Ausschnitt-Breite
        private double _dx = 0; // Verschiebung 
        private double _dy = 0; // 10*PI; 

        private ColorHelper _ch = new ColorHelper();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pic.Paint += Pic_Paint;
            this.Height = 800;
            this.Width = 800;
        }

        private void Pic_Paint(object sender, PaintEventArgs e)
        {
            // draw
            pic.SuspendLayout();
            for (int x = 0; x < pic.Width; x++)
                for (int y = 0; y < pic.Height; y++)
                {
                    Brush aBrush;
                    aBrush = new SolidBrush(GetColor(x, y));
                    Graphics g = e.Graphics;
                    g.FillRectangle(aBrush, x, y, 1, 1);
                }
            pic.ResumeLayout();
        }

        const double PI = Math.PI;

        private Color GetColor(int x, int y)
        {
            double xRad = _zoom * x / pic.Width - _zoom / 2 + _dx;
            double yRad = _zoom * y / pic.Height - _zoom / 2 + _dy;
            return GetColorByRad(xRad, yRad);
        }


        private Color GetColorByRad(double xRad, double yRad)
        {
            // Ausrechnen
            double f1 = Math.Sin(Math.Sin(xRad * (Math.Sin(yRad) - Math.Cos(xRad))));
            double f2 = Math.Cos(Math.Cos(yRad * (Math.Cos(xRad) - Math.Sin(yRad))));
            var result = f1 - f2;
            //System.IO.File.AppendAllText("out.txt", $"{xRad}\t{yRad}\t{result}");
            //Debug.WriteLine($"{xRad}\t{yRad}\t{result}");
            return _ch.GetColor(result);
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            _zoom -= PI;
            pic.Invalidate();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            _zoom += PI;
            pic.Invalidate();
        }

        // 



    }
}
