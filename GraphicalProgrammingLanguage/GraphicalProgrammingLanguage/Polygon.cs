using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicalProgrammingLanguage
{
    internal class Polygon : Shape
    {
        protected int sides;
        protected int width;
        protected int upsideDown;
        protected Point[] points;

        public Polygon() { }

        override public void Set(Color colour, params int[] parameters)
        {
            this.color = colour;
            this.xPos = parameters[0];
            this.yPos = parameters[1];
            this.sides = parameters[2];
            this.width = parameters[3];
            this.upsideDown = parameters[4];
            this.points = new Point[sides];

            for (int i = 0; i < this.sides; i++)
            {
                // Super complicated formula for calculating each point of a regular polygon given a centre point.
                if (upsideDown == 1)
                {
                    //For an odd n sided polygon, this will put the odd vertex at the bottom.
                    points[i] = new Point((int)Math.Round((this.xPos + this.width * Math.Sin(2 * Math.PI * i / this.sides))), (int)Math.Round((this.yPos + this.width * Math.Cos(2 * Math.PI * i / this.sides))));
                }
                else
                {
                    //For an odd n sided polygon, this will put the odd vertex at the top.
                    points[i] = new Point((int)Math.Round((this.xPos - this.width * Math.Sin(2 * Math.PI * i / this.sides))), (int)Math.Round((this.yPos - this.width * Math.Cos(2 * Math.PI * i / this.sides))));
                }
            }
        }

        public override void Draw(Graphics g, Boolean fill)
        {
            if (!fill)
            {
                g.DrawPolygon(new Pen(this.color), this.points);
            }
            else
            {
                g.FillPolygon(new SolidBrush(this.color), this.points);
            }
            
        }

        public Point[] GetPoints() { return this.points; }

    }
}
