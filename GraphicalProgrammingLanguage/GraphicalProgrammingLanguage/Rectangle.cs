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

        override public void Set(Color color, params int[] parameters)
        {
            this.color = color;
            this.width = parameters[0];
            this.height = parameters[1];
            this.xPos = parameters[2];
            this.yPos = parameters[3];
        }

        override public void Draw(Graphics g, Boolean fill)
        {
            if (!fill)
            {
                g.DrawRectangle(new Pen(this.color), new System.Drawing.Rectangle(this.xPos - (this.width / 2), this.yPos - (this.height / 2), this.width, this.height));
            }
            else
            {
                g.FillRectangle(new SolidBrush(this.color), new System.Drawing.Rectangle(this.xPos - (this.width / 2), this.yPos - (this.height / 2), this.width, this.height));
            }
        }
    }
}
