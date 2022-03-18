using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicalProgrammingLanguage
{
    internal class Circle : Shape
    {
        protected int diameter;

        public Circle() { }

        override public void Set(Color colour, params int[] parameters)
        {
            this.colour = colour;
            this.diameter = parameters[0];
            this.XPos = parameters[1];
            this.YPos = parameters[2];
        }

        override public void Draw(Graphics g)
        {
            g.DrawEllipse(new Pen(this.colour),new System.Drawing.Rectangle(this.XPos - (this.diameter/2), this.YPos - (this.diameter / 2), this.diameter, this.diameter));
        }
    }
}
