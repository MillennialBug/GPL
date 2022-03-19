using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicalProgrammingLanguage
{
    internal class Canvas
    {
        const int DEFAULT_X_POS = 0;
        const int DEFAULT_Y_POS = 0;
        const int DEFAULT_PEN_WIDTH = 1;

        Graphics g;
        Color color;
        Pen pen;
        int xPos = DEFAULT_X_POS;
        int yPos = DEFAULT_Y_POS;
        bool fill = false;

        public Canvas(Graphics g)
        {
            this.g = g;
            this.color = Color.Black;
            this.pen = new Pen(this.color);
        }

        public Graphics GetGraphics() { return g; }

        public void DrawLine(List<int> args)
        {
            this.g.DrawLine(pen, xPos, yPos, args[0], args[1]);
            this.xPos = args[0];
            this.yPos = args[1];
        }

        public void DrawShape(Shape shape, List<int> args)
        {
            args.Add(xPos);
            args.Add(yPos);
            shape.Set(Color.Black, args.ToArray());
            shape.Draw(this.g, fill);
        }

        public void Reset()
        {
            this.xPos = DEFAULT_X_POS;
            this.yPos = DEFAULT_Y_POS;
        }

        public void Clear()
        {
            this.g.Clear(Color.White);
        }

        public void MoveCursor(List<int> args)
        {
            this.xPos = args[0];
            this.yPos = args[1];
        }

        public void SetColor(Color color)
        {
            this.pen = new Pen(color);
        }

        public void SetFill(String state)
        {
            if(String.Equals(state, "on"))
            {
                fill = true;
            }
            else
            {
                fill = false;
            }
        }

        public void RestoreDefaultState()
        {
            this.SetFill("off");
            this.pen = new Pen(Color.Black, DEFAULT_PEN_WIDTH);
            this.Reset();
            this.Clear();
        }
    }
}
