using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Class to create an equilateral Polygon.
    /// </summary>
    public class Polygon : Shape
    {
        protected int sides;
        protected int width;
        protected int upsideDown = 0;
        protected Point[] points;

        public Polygon() { }

        /// <summary>
        /// Takes in a color and a list of integer parameters and sets the internal properties of the class accordingly.
        /// </summary>
        /// <param name="colour"></param>
        /// <param name="g">Graphics context for the drawing.</param>
        /// <param name="fill">Boolean determining whether the drawing should be outline or filled.</param>
        /// <param name="parameters"></param>
        override public void Set(Color colour, Graphics g, Boolean fill, params int[] parameters)
        {
            this.g = g;
            this.fill = fill;
            this.color = colour;
            this.sides = parameters[0];
            this.width = parameters[1];
            this.xPos = parameters[2];
            this.yPos = parameters[3];
            if (parameters.Length == 5) this.upsideDown = parameters[4]; //Makes the upsidedown parameter optional.
            this.points = new Point[this.sides];

            for (int i = 0; i < this.sides; i++)
            {
                // Formula for calculating each point of a regular polygon given a centre point.
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

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            if (!this.fill)
            {
                this.g.DrawPolygon(new Pen(this.color), this.points);
            }
            else
            {
                this.g.FillPolygon(new SolidBrush(this.color), this.points);
            }
            
        }

        /// <summary>
        /// Returns a Point array containing the verticies of the Polygon.
        /// </summary>
        /// <returns>Point array of verticies.</returns>
        public Point[] GetPoints() { return this.points; }

        public override void Execute()
        {
            Draw();
        }

    }
}
