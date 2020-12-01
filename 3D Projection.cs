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

        double yaw = 0;
        double pitch = 0;

        double cameraX = 0;
        double cameraY = 0;
        double cameraZ = 0;


        bool mouseDown = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Rotation about the axes
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

        double[] rot(double x, double y, double z, double rx, double ry, double rz)
        {
            double[] point = new double[] { x - cameraX, y - cameraY, z - cameraZ };
            point = rotX(point[0], point[1], point[2], rx);
            point = rotY(point[0], point[1], point[2], ry);
            point = rotZ(point[0], point[1], point[2], rz);
            return point;
        }

        //Convert a (x,y) point to screen coordinates
        double[] ptc(double x, double y)
        {
            return new double[] { this.Width/2 + (x * this.Height / 2), this.Height/2 - (y * this.Height / 2) };
        }

        //Add depth
        double[] to2d(double x, double y, double z)
        {

            double d = y * Math.Tan(50 * Math.PI / 180);
            double newx = x / d;
            double newy = z / d;

            return new double[] { newx, newy };
        }

        //Drawing
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush b = new SolidBrush(Color.Green);
            Pen p = new Pen(Color.Black);
            p.Width = 5;

            g.Clear(Color.White);
            
            connectLine(1, 1, 1, 0, 1, 1);
            connectLine(0, 1, 1, 0, 1, 0);
            connectLine(0, 1, 0, 1, 1, 0);
            connectLine(1, 1, 0, 1, 1, 1);

            connectLine(1, 2, 1, 0, 2, 1);
            connectLine(0, 2, 1, 0, 2, 0);
            connectLine(0, 2, 0, 1, 2, 0);
            connectLine(1, 2, 0, 1, 2, 1);
            

            connectLine(1, 2, 1, 1, 1, 1);
            connectLine(0, 2, 1, 0, 1, 1);
            connectLine(0, 2, 0, 0, 1, 0);
            connectLine(1, 2, 0, 1, 1, 0);
            

            void connectLine(double x, double y, double z, double x2, double y2, double z2)
            {
                //Try to convert pitch into rotations about the x and y axes, I'm having some trouble here.
                double rotX = pitch * Math.Cos(-yaw);
                double rotY = pitch * Math.Sin(-yaw);

                double[] point1 = rot(x, y, z, rotX, rotY, yaw);
                point1 = to2d(point1[0], point1[1], point1[2]);
                point1 = ptc(point1[0], point1[1]);

                double[] point2 = rot(x2, y2, z2, rotX, rotY, yaw);
                point2 = to2d(point2[0], point2[1], point2[2]);
                point2 = ptc(point2[0], point2[1]);

                double i = 0.1;

                double slopeX = (x2 - x) * i;
                double slopeY = (y2 - y) * i;
                double slopeZ = (z2 - z) * i;

                //Draw each pixel of the line point by point, as I plan on having something where
                //you can click on the line to see the coordinates at that point

                for (double t = 0; t <= 1 / i; t += i)
                {

                    double[] point = rot(x + t*slopeX, y + t*slopeY, z + t*slopeZ, rotX, rotY, yaw);

                    if (point[1] <= 0.01) continue;

                    double[] pointp = to2d(point[0], point[1], point[2]);

                    double[] pointpc= ptc(pointp[0], pointp[1]);

                    g.FillEllipse(b, (int) pointpc[0] - 5, (int) pointpc[1] - 5, (int) 10, (int) 10);
                    
                }

            }

            g.FillEllipse(b, this.Width / 2 - 5, this.Height / 2 - 5, 10, 10);
        }

        //Turning
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            Thread TH = new Thread(mouseHeld);
            TH.Start();

        }

        void mouseHeld()
        {
            double mx = Cursor.Position.X;
            double my = Cursor.Position.Y;

            while (mouseDown)
            {

                yaw -= (Cursor.Position.X - mx) * 0.01;
                pitch -= (Cursor.Position.Y - my) * 0.01;

                repaint();
                
                mx = Cursor.Position.X;
                my = Cursor.Position.Y;

                Thread.Sleep(20);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        void repaint()
        {
            Invalidate();
        }


        //Movement
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {    
                double zvel = 0.1 * Math.Sin(pitch);
                double xyvel = 0.1 * Math.Cos(pitch);

                cameraY += xyvel * Math.Sin(yaw + Math.PI / 2);
                cameraX += xyvel * Math.Cos(yaw + Math.PI / 2);

                cameraZ += zvel;
            }

            if (e.KeyChar == 'a')
            {
                cameraY -= 0.1 * Math.Sin(yaw);
                cameraX -= 0.1 * Math.Cos(yaw);
               
            }

            if (e.KeyChar == 'd')
            {
                cameraY += 0.1 * Math.Sin(yaw);
                cameraX += 0.1 * Math.Cos(yaw);
            }

            if (e.KeyChar == 's')
            {
                
                double zvel = 0.1 * Math.Sin(pitch);
                double xyvel = 0.1 * Math.Cos(pitch);

                cameraY -= xyvel * Math.Sin(yaw + Math.PI / 2);
                cameraX -= xyvel * Math.Cos(yaw + Math.PI / 2);

                cameraZ -= zvel;
            }

            if (e.KeyChar == ' ')
            {
                cameraZ += 0.1;
            }

            repaint();
        }
    }
}
