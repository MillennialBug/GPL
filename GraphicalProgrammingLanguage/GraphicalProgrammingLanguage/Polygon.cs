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
            this.colour = colour;
            this.XPos = parameters[0];
            this.YPos = parameters[1];
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
                    points[i] = new Point((int)Math.Round((this.XPos + this.width * Math.Sin(2 * Math.PI * i / this.sides))), (int)Math.Round((this.YPos + this.width * Math.Cos(2 * Math.PI * i / this.sides))));
                }
                else
                {
                    //For an odd n sided polygon, this will put the odd vertex at the top.
                    points[i] = new Point((int)Math.Round((this.XPos - this.width * Math.Sin(2 * Math.PI * i / this.sides))), (int)Math.Round((this.YPos - this.width * Math.Cos(2 * Math.PI * i / this.sides))));
                }
            }
        }

        public override void Draw(Graphics g)
        {
            g.DrawPolygon(new Pen(this.colour), this.points);
        }

        public Point[] GetPoints() { return this.points; }

    }
}
