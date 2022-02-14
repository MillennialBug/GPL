using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicalProgrammingLanguage
{
    internal interface ShapeInterface
    {
        void Set(Color colour, params int[] parameters);
        void Draw(Graphics g);
    }
}
