using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    public class Star : Shape
    {
        protected Polygon inner;
		protected Point[] innerPoints;
        protected Polygon outer;
		protected Point[] outerPoints;
		protected Point[] points;
		protected int sides;

        public Star() { }

		/// <summary>
		/// Takes in a color and a list of integer parameters and sets the internal properties of the class accordingly.
		/// For Star, this creates 2 Polygon objects which are used to calculate how to draw the Star shape.
		/// </summary>
		/// <param name="colour">The Color in which to Draw the Shape.</param>
		/// <param name="g">Graphics context for the drawing.</param>
		/// <param name="fill">Boolean determining whether the drawing should be outline or filled.</param>
		/// <param name="parameters">An int array which must contain Number of Points, Width, X Co-ord, Y Co-ord for the Shape in that order.</param>
		public override void Set(Color color, Graphics g, Boolean fill, params int[] parameters)
        {
			this.g = g;
			this.fill = fill;
			this.sides = parameters[0];
			this.color = color;
			int[] params1 = new int[] { parameters[0], parameters[1], parameters[2], parameters[3] };
			outer = new Polygon();
			outer.Set(this.color, this.g, this.fill, params1);
			outerPoints = outer.GetPoints();
			int[] params2 = new int[] { parameters[0], parameters[1] / 2, parameters[2], parameters[3], 1 };
			inner = new Polygon();
			inner.Set(this.color, this.g, this.fill, params2);
			innerPoints = inner.GetPoints();
			points = new Point[this.sides*2];
			/*
			 * This took a while to figure out.
			 * Polygons are drawn from their odds point (for odd n) anti clockwise.
			 * To form the outline of a star we need to loop through each vertex in both polygons
			 * starting at vertex 0 for the larger outer polygon and the n/2 (rounded up) vertex for
			 * the smaller inner polygon.
			 * Another issue I encountered was that even sided polygons line up, meaning the vertices don't
			 * form triangles like they do in odd n polygons. To fix this I had to work out the mid point
			 * between the 2 vertices either side of the larger polygons vertex we are currently on.
			 * The % calculations mean we avoid any ArrayOutOfBounds exceptions.
			 */
			int point = 0;
			for (int i = 0; i < this.sides; i++)
			{
				this.points[point++] = new Point(outerPoints[i].X, outerPoints[i].Y);
				int j = (int)((Math.Ceiling(this.sides / 2.0)) + i) % this.sides;
				if (this.sides % 2 != 0)
					// for odd n just use the inner vertex
					points[point++] = new Point(innerPoints[j].X, innerPoints[j].Y);
				else
					// for even n get the midpoint
					points[point++] = new Point((innerPoints[j].X + innerPoints[(j+1) % this.sides].X) / 2, (innerPoints[j].Y + innerPoints[(j+1) % this.sides].Y) / 2);
			}
		}

		/// <summary>
		/// Draws the shape at the specified location and with the given measurements and fill status.
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

        public override void Execute()
        {
			Draw();
        }
    }
}
