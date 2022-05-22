using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicalProgrammingLanguage
{
    public class Circle : Shape
    {
        protected int diameter;

        public Circle() { }

        /// <summary>
        /// Takes in a color and a list of integer parameters and sets the internal properties of the class accordingly.
        /// </summary>
        /// <param name="colour">The Color in which to Draw the Shape.</param>
        /// <param name="g">Graphics context for the drawing.</param>
        /// <param name="fill">Boolean determining whether the drawing should be outline or filled.</param>
        /// <param name="parameters">An int array which must contain Diameter, X Co-ord, Y Co-ord for the Shape in that order.</param>
        override public void Set(Color color, Graphics g, Boolean fill, params int[] parameters)
        {
            this.g = g;
            this.fill = fill;
            this.color = color;
            this.diameter = parameters[0];
            this.xPos = parameters[1];
            this.yPos = parameters[2];
        }

        /// <summary>
        /// Draws the shape at the specified location and with the given measurements and fill status.
        /// </summary>
        override public void Draw()
        {
            if (!this.fill)
            {
                this.g.DrawEllipse(new Pen(this.color), new System.Drawing.Rectangle(this.xPos - (this.diameter / 2), this.yPos - (this.diameter / 2), this.diameter, this.diameter));
            }
            else
            {
                this.g.FillEllipse(new SolidBrush(this.color), new System.Drawing.Rectangle(this.xPos - (this.diameter / 2), this.yPos - (this.diameter / 2), this.diameter, this.diameter));
            }
        }

        public override void Execute()
        {
            Draw();
        }
    }
}
