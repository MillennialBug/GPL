﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class Star : Shape
    {
        protected Polygon inner;
		protected Point[] innerPoints;
        protected Polygon outer;
		protected Point[] outerPoints;
		protected int sides;
		protected int width;
		protected Point[] points;

        public Star() { }

        public override void Set(Color colour, params int[] parameters)
        {
			int[] params1 = new int[] { parameters[0], parameters[1], parameters[2], parameters[3], 0 };
			outer = new Polygon();
			outer.Set(this.color, params1);
			outerPoints = outer.GetPoints();
			int[] params2 = new int[] { parameters[0], parameters[1], parameters[2], parameters[3] / 2, 1 };
			inner = new Polygon();
			inner.Set(this.color, params2);
			innerPoints = inner.GetPoints();
			Polygon star = new Polygon();
			points = new Point[this.sides];
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
    }
}
