using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp15
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        double[] rotZ(double x, double y, double z, double r)
        {
            double a = Math.Atan2(y, x);
            double h = Math.Sqrt(x * x + y * y);

            return new double[] { h * Math.Cos(a - r), h * Math.Sin(a - r), z };
        }

        double[] rotX(double x, double y, double z, double r)
        {
            double a = Math.Atan2(z, y);
            double h = Math.Sqrt(z * z + y * y);

            return new double[] { x, h * Math.Cos(a - r), h * Math.Sin(a - r) };
        }

        double[] rotY(double x, double y, double z, double r)
        {
            double a = Math.Atan2(z, x);
            double h = Math.Sqrt(z * z + x * x);

            return new double[] { h * Math.Cos(a - r), y, h * Math.Sin(a - r) };
        }

        /*
        double[] rot(double x, double y, double z, double rx, double ry, double rz)
        {
            return
        }*/

        double[] ptc(double x, double y)
        {
            return new double[] { this.Width/2 + x, this.Height/2 - y };
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush b = new SolidBrush(Color.Black);
            Pen p = new Pen(Color.Black);
            p.Width = 10;

            for(int i = 0; i < 360; i++)
            {

                g.Clear(Color.White);

                double[] x1 = rotZ(100, 0, 0, i * Math.PI / 180);
                x1 = rotX(x1[0], x1[1], x1[2], i / 4 * Math.PI / 180);
                double[] x1p = ptc(x1[0], x1[2]);


                double[] x2 = rotZ(-100, 0, 0, i * Math.PI / 180);
                x2 = rotX(x2[0], x2[1], x2[2], i / 4 * Math.PI / 180);
                double[] x2p = ptc(x2[0], x2[2]);

                double[] y1 = rotZ(0, 100, 0, i * Math.PI / 180);
                y1 = rotX(y1[0], y1[1], y1[2], i / 4 * Math.PI / 180);
                double[] y1p = ptc(y1[0], y1[2]);


                double[] y2 = rotZ(0, -100, 0, i * Math.PI / 180);
                y2 = rotX(y2[0], y2[1], y2[2], i / 4 * Math.PI / 180);
                double[] y2p = ptc(y2[0], y2[2]);

                double[] z1 = rotZ(0, 0, 100, i * Math.PI / 180);
                z1 = rotX(z1[0], z1[1], z1[2], i / 4 * Math.PI / 180);
                double[] z1p = ptc(z1[0], z1[2]);

                double[] z2 = rotZ(0, 0, -100, i * Math.PI / 180);
                z2 = rotX(z2[0], z2[1], z2[2], i / 4 * Math.PI / 180);
                double[] z2p = ptc(z2[0], z2[2]);


                g.DrawLine(p, (int) x1p[0], (int) x1p[1], (int) x2p[0], (int) x2p[1]);
                g.DrawLine(p, (int) y1p[0], (int) y1p[1], (int) y2p[0], (int) y2p[1]);
                g.DrawLine(p, (int) z1p[0], (int) z1p[1], (int) z2p[0], (int) z2p[1]);

                Thread.Sleep(20);

            }
        }
    }
}
