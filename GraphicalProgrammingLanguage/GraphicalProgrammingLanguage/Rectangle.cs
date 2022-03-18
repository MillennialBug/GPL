using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicalProgrammingLanguage
{
    internal class Rectangle : Shape
    {
        protected int width;
        protected int height;

        public Rectangle() { }

        override public void Set(Color colour, params int[] parameters)
        {
            this.colour = colour;
            this.width = parameters[0];
            this.height = parameters[1];
            this.XPos = parameters[2];
            this.YPos = parameters[3];
        }

        override public void Draw(Graphics g)
        {
            g.DrawRectangle(new Pen(this.colour), new System.Drawing.Rectangle(this.XPos - (this.width/2), this.YPos - (this.height/2), this.width, this.height));
        }
    }
}
