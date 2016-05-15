using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsPlatformer
{
    struct Rect
    {
        public Vector Center;
        public Size Size;

        public Rect(Vector center, Size size)
        {
            Center = center;
            Size = size;
        }

        public Rect(double x, double y, double width, double height)
        {
            Center = new Vector(x, y);
            Size = new Size(width, height);
        }
    }
}
