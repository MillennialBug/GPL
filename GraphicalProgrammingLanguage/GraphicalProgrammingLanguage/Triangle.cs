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
