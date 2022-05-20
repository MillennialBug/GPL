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

        override public void Set(Color color, Graphics g, Boolean fill, params int[] parameters)
        {
            this.g = g;
            this.fill = fill;
            this.color = color;
            this.diameter = parameters[0];
            this.xPos = parameters[1];
            this.yPos = parameters[2];
        }

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
