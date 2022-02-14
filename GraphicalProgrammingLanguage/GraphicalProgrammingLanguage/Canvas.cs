using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicalProgrammingLanguage
{
    internal class Canvas
    {
        const int DefaultXPos = 0;
        const int DefaultYPos = 0;
        const int DefaultPenWidth = 1;

        Graphics g;
        Pen Pen = new Pen(Color.Black, DefaultPenWidth);
        int XPos = DefaultXPos;
        int YPos = DefaultYPos;

        public Canvas(Graphics g)
        {
            this.g = g;
        }

        public Graphics GetGraphics() { return g; }

        public void DrawLine(int ToX, int ToY)
        {
            this.g.DrawLine(Pen, XPos, YPos, ToX, ToY);
            this.XPos = ToX;
            this.YPos = ToY;
        }
    }
}
