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
        protected int XPos, YPos;
        protected Color colour;

        public Shape()
        {
        }

        abstract public void Set(Color colour, params int[] parameters);

        abstract public void Draw(Graphics g);

    }
}
