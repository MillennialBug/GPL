using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    abstract internal class Shape : ShapeInterface
    {
        protected int xPos, yPos;
        protected Color color;

        public Shape() { }

        abstract public void Set(Color color, params int[] parameters);

        abstract public void Draw(Graphics g, Boolean fill);

    }
}
