using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Graphics3Dto2D
{
    class DepthBuffer
    {

        public Coefficient_Of_Plane COPLANE;
        public DepthBuffer()
        {
            COPLANE = new Coefficient_Of_Plane();
        }
        public Coefficient_Of_Plane Coefficient_Value(_3Dpoint pt1, _3Dpoint pt2, _3Dpoint pt3)
        {
           // double Y = 0;
            Coefficient_Of_Plane pt;
            pt = new Coefficient_Of_Plane();
            pt.A = (pt2.z - pt3.z) * (pt1.y - pt2.y) - (pt1.z - pt2.z) * (pt2.y - pt3.y);
            pt.B = (pt2.x - pt3.x) * (pt1.z - pt2.z) - (pt1.x - pt2.x) * (pt2.z - pt3.z);
            pt.C = (pt2.y - pt3.y) * (pt1.x - pt2.x) - (pt1.y - pt2.y) * (pt2.x - pt3.x);
            pt.D = - pt1.x * (pt2.y * pt3.z - pt2.z * pt3.y) + pt1.y * (pt2.x * pt3.z - pt2.z * pt3.x) - pt1.z * (pt2.x * pt3.y - pt2.y * pt3.x);
            return pt;
           
        }
        public double DepthValue(Coefficient_Of_Plane surface,int x,int y)
        {
           
            double z = 200;
            if (surface.B != 0)
                z = (-surface.A * x - surface.C * y - surface.D) / surface.B;
            return z;
        }
        public bool CheckInside(Point point1, Point point2, Point point3, int x, int y)
        {
            float fxyC, fxy;
            fxyC = point3.Y * (point2.X - point1.X) - point3.X * (point2.Y - point1.Y) + point1.X * (point2.Y - point1.Y) - point1.Y * (point2.X - point1.X);
            fxy =  y * (point2.X - point1.X) - x * (point2.Y - point1.Y) + point1.X * (point2.Y - point1.Y) - point1.Y * (point2.X - point1.X);
            if (((fxyC <= 0) && (fxy <= 0)) || ((fxyC >= 0) && (fxy >= 0)))
                return true;
            else
                return false;
        }
        //Subroutine to check any point(x,y) inside the triangle
        public bool inside_triangle_check(Point pt1, Point pt2, Point pt3, int x, int y)
        {
            
            bool check = CheckInside(pt1, pt2, pt3, x, y) && CheckInside(pt2, pt3, pt1, x, y) && CheckInside(pt3, pt1, pt2, x, y);
            if (check == true) return true;
            else return false;
        }
    }
}
