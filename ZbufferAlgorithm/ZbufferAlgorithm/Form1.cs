using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Windows.Media;

namespace Graphics3Dto2D
{ 
    public partial class Form1 : Form
    {
        private Bitmap m_Canvas;
        

        private  DepthBuffer depthbuffer;
            
            Coefficient_Of_Plane[] surface= new Coefficient_Of_Plane[12];
            double [] depth=new double[12];
            _3Dpoint[] Dpoint =new _3Dpoint[8];
            int[,] _2dpoint = new int[8,2];

        private _3Dpoint temp;

        private  double neg = -20;
        private  double pov =  20;
        private  double near = 100;
        private  double far =  140;
        private  Projection proc;
            
        private  double z_buffer;
        private  Color Finalcolor;
        private  Color []color=new Color[12];
        
        public Form1()
        {
            InitializeComponent();
            CenterToScreen();
            SetStyle(ControlStyles.ResizeRedraw, true);


            Dpoint[0] = new _3Dpoint(neg, near, pov);
            Dpoint[1] = new _3Dpoint(pov, near, pov);
            Dpoint[2] = new _3Dpoint(pov, near, neg);
            Dpoint[3] = new _3Dpoint(neg, near, neg);
            Dpoint[4] = new _3Dpoint(neg + 20, far, pov + 20);
            Dpoint[5] = new _3Dpoint(pov + 20, far, pov + 20);
            Dpoint[6] = new _3Dpoint(pov + 20, far, neg + 20);
            Dpoint[7] = new _3Dpoint(neg + 20, far, neg + 20);

           
          

            depthbuffer = new DepthBuffer();
            proc = new Projection();



        }
        private void DrawCube()
        {
           
            ////front color
            //color[0] = Color.Blue;
            //color[1] = Color.Blue;
            ////left color
            //color[2] = Color.Red;
            //color[3] = Color.Red;
            ////right color
            //color[4] = Color.Green;
            //color[5] = Color.Green;
            ////top color
            //color[6] = Color.Orange;
            //color[7] = Color.Orange;
            ////bottom color
            //color[8] = Color.Yellow;
            //color[9] = Color.Yellow;
            ////back color
            //color[10] = Color.White;
            //color[11] = Color.White;

            for (int i = 0; i < 8; i++)
            {
                temp = new _3Dpoint(Dpoint[i]);
                if (proc.Trans_Point(temp))
                {
                    _2dpoint[i, 0] = proc.p1.h;
                    _2dpoint[i, 1] = proc.p1.v;
                }
                else
                {
                    MessageBox.Show("conversion is invalid");
                    break;
                }
            }

            Point point1 = new Point(_2dpoint[0, 0], _2dpoint[0, 1]);
            Point point2 = new Point(_2dpoint[1, 0], _2dpoint[1, 1]);
            Point point3 = new Point(_2dpoint[2, 0], _2dpoint[2, 1]);
            Point point4 = new Point(_2dpoint[3, 0], _2dpoint[3, 1]);
            Point point5 = new Point(_2dpoint[4, 0], _2dpoint[4, 1]);
            Point point6 = new Point(_2dpoint[5, 0], _2dpoint[5, 1]);
            Point point7 = new Point(_2dpoint[6, 0], _2dpoint[6, 1]);
            Point point8 = new Point(_2dpoint[7, 0], _2dpoint[7, 1]);
          

            //front surface dividing into two triangle
            surface[0] = depthbuffer.Coefficient_Value(Dpoint[0], Dpoint[1], Dpoint[2]); color[0] = Color.Blue;
            surface[1] = depthbuffer.Coefficient_Value(Dpoint[2], Dpoint[3], Dpoint[0]); color[1] = Color.Blue;

            //left surface dividing into two triangle
            surface[2] = depthbuffer.Coefficient_Value(Dpoint[0], Dpoint[4], Dpoint[7]); color[2] = Color.Red;
            surface[3] = depthbuffer.Coefficient_Value(Dpoint[7], Dpoint[3], Dpoint[0]); color[3] = Color.Red;

            //right surface dividing into two triangle
            surface[4] = depthbuffer.Coefficient_Value(Dpoint[1], Dpoint[5], Dpoint[6]); color[4] = Color.Green;
            surface[5] = depthbuffer.Coefficient_Value(Dpoint[6], Dpoint[2], Dpoint[1]); color[5] = Color.Green;

            //top surface dividing into two triangle
            surface[6] = depthbuffer.Coefficient_Value(Dpoint[0], Dpoint[1], Dpoint[5]); color[6] = Color.Orange;
            surface[7] = depthbuffer.Coefficient_Value(Dpoint[5], Dpoint[4], Dpoint[0]); color[7] = Color.Orange;

            //bottom surface dividing into two triangle
            surface[8] = depthbuffer.Coefficient_Value(Dpoint[3], Dpoint[2], Dpoint[6]); color[8] = Color.Yellow;
            surface[9] = depthbuffer.Coefficient_Value(Dpoint[6], Dpoint[7], Dpoint[3]); color[9] = Color.Yellow;

            //back surface dividing into two triangle
            surface[10] = depthbuffer.Coefficient_Value(Dpoint[4], Dpoint[5], Dpoint[6]); color[10] = Color.White;
            surface[11] = depthbuffer.Coefficient_Value(Dpoint[6], Dpoint[7], Dpoint[4]); color[11] = Color.White;
            


            m_Canvas = new Bitmap(1440, 920); // Doesn't have to be initialized here

            for (int x = 320; x < 1120; x++)
            {
                for (int y = 20; y < 820; y++)
                {
                    for (int numsurf = 0; numsurf < 12; numsurf++)
                    {
                        depth[numsurf] =200;
                        Finalcolor = Color.Gray;
                    }
                    //front
                    if (depthbuffer.inside_triangle_check(point1,point2,point3, x, y))
                        depth[0] = depthbuffer.DepthValue(surface[0], 0, 0);
                    if (depthbuffer.inside_triangle_check(point3, point4, point1, x, y))
                        depth[1] = depthbuffer.DepthValue(surface[1], 0, 0);

                    //left
                    if (depthbuffer.inside_triangle_check(point1, point5, point8, x, y))
                        depth[2] = depthbuffer.DepthValue(surface[2], 0, 0);
                    if (depthbuffer.inside_triangle_check(point8, point4, point1, x, y))
                        depth[3] = depthbuffer.DepthValue(surface[3], 0, 0);

                    //right
                    if (depthbuffer.inside_triangle_check(point2, point6, point7, x, y))
                        depth[4] = depthbuffer.DepthValue(surface[4], 0, 0);
                    if (depthbuffer.inside_triangle_check(point7, point3, point2, x, y))
                        depth[5] = depthbuffer.DepthValue(surface[5], 0, 0);

                    //top
                    if (depthbuffer.inside_triangle_check(point1, point2, point6, x, y))
                        depth[6] = depthbuffer.DepthValue(surface[6], 0, 0);
                    if (depthbuffer.inside_triangle_check(point6, point5, point1, x, y))
                        depth[7] = depthbuffer.DepthValue(surface[7], 0, 0);

                    //bottom
                    if (depthbuffer.inside_triangle_check(point4, point3, point7, x, y))
                        depth[8] = depthbuffer.DepthValue(surface[8], 0, 0);
                    if (depthbuffer.inside_triangle_check(point7, point8, point4, x, y))
                        depth[9] = depthbuffer.DepthValue(surface[9],0, 0);

                    //back
                    if (depthbuffer.inside_triangle_check(point5, point6, point7, x, y))
                        depth[10] = depthbuffer.DepthValue(surface[10], 0, 0);
                    if (depthbuffer.inside_triangle_check(point7, point8, point5, x, y))
                        depth[11] = depthbuffer.DepthValue(surface[11], 0, 0);
                    z_buffer = 200;
                    Finalcolor = Color.Gray;
                    for (int numsurf = 0; numsurf < 12; numsurf++)
                    {
                        
                        if (depth[numsurf] < z_buffer)
                        {
                            z_buffer = depth[numsurf];
                            Finalcolor=color[numsurf];
                        }
                    }

                    m_Canvas.SetPixel(x, y, Finalcolor);
                }
            }

            SetCanvasAsImage();

        }
        public void SetCanvasAsImage()
        {
            pictureBox1.Image = m_Canvas;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            DrawCube();
        }

    }
}
