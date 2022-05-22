using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicalProgrammingLanguage
{
    public class Triangle : Polygon
    {
        /// <summary>
        /// Takes in a color and a list of integer parameters and sets the internal properties of the class accordingly.
        /// For Triangle, this calls the Polygon Set method setting the sides to 3 in all cases.
        /// </summary>
        /// <param name="colour">The Color in which to Draw the Shape.</param>
        /// <param name="g">Graphics context for the drawing.</param>
        /// <param name="fill">Boolean determining whether the drawing should be outline or filled.</param>
        /// <param name="parameters">An int array which must contain Width, X Co-ord, Y Co-ord for the Shape in that order.</param>
        override public void Set(Color color, Graphics g, Boolean fill, params int[] parameters)
        {
            this.g = g;
            this.fill = fill;
            this.color = color;
            int[] params1 = new int[4] { 3, parameters[0], parameters[1], parameters[2] };
            base.Set(this.color, this.g, this. fill, params1);
        }
    }
}
