using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    abstract public class Shape : Command
    {
        protected int xPos, yPos;
        protected Color color;
        protected Graphics g;
        protected Boolean fill;

        public Shape() { }

        abstract public void Set(Color color, Graphics g, Boolean fill, params int[] parameters );

        abstract public void Draw();

        abstract public override void Execute();

    }
}
