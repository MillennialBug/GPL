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
        protected int size;

        public Circle() { }

        override public void Set(Color colour, params int[] parameters)
        {
            this.colour = colour;
            this.XPos = parameters[0];
            this.YPos = parameters[1];
            this.size = parameters[2];
        }

        override public void Draw(Graphics g)
        {
            g.DrawEllipse(new Pen(this.colour),new Rectangle(this.XPos, this.YPos, this.size, this.size));
        }
    }
}
