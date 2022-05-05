using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// The Canvas class acts as a controller for drawing onto a Graphics context.
    /// </summary>
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

        private static Canvas canvas = new Canvas();

        /// <summary>
        /// Constructor for Canvas taking a single argument. Also sets default color and default pen.
        /// </summary>
        /// <param name="g">Graphics context for drawing to.</param>
        private Canvas()
        {
            this.color = Color.Black;
            this.pen = new Pen(this.color);
        }

        public static Canvas GetCanvas() { return canvas; }

        public void SetGraphics(Graphics g) { this.g = g; }
        /// <summary>
        /// Returns the Canvas' Graphics context so it can be used by other Classes if necessary.
        /// </summary>
        /// <returns>The Canvas' Graphics context.</returns>
        public Graphics GetGraphics() { return g; }

        /// <summary>
        /// Draws a line on the Canvas between 2 given points using the current Pen.
        /// </summary>
        /// <param name="args">A List of integer values. Needs to hold 2 values; the destination X and Y co-ordiantes.</param>
        public void DrawLine(List<int> args)
        {
            this.g.DrawLine(pen, xPos, yPos, args[0], args[1]);
            this.xPos = args[0];
            this.yPos = args[1];
        }

        /// <summary>
        /// Draws a given shape using the Shape object's Draw method. Also calls Set method on Shape to set the properties of the shape.
        /// </summary>
        /// <param name="shape">Shape object to be drawn</param>
        /// <param name="args">List of integer values to be passed into the Shape object's Set method. 
        /// Must match order in Shape object's implementation of Set.
        /// xPos and yPos are added to the list in this method before Set is called.</param>
        public void DrawShape(Shape shape, List<int> args)
        {
            args.Add(xPos);
            args.Add(yPos);
            shape.Set(this.color, args.ToArray());
            shape.Draw(this.g, fill);
        }

        /// <summary>
        /// Resets the cursor position to it's origin, 0,0.
        /// </summary>
        public void Reset()
        {
            this.xPos = DEFAULT_X_POS;
            this.yPos = DEFAULT_Y_POS;
        }

        /// <summary>
        /// Clears the drawing area of all drawings. Restores whole bitmap to Color.White.
        /// </summary>
        public void Clear()
        {
            this.g.Clear(Color.White);
        }

        /// <summary>
        /// Moves the cursor to a given location. 
        /// </summary>
        /// <param name="args">A List of integer values. Must hold 2 values, X and Y co-ordinates.</param>
        public void MoveCursor(List<int> args)
        {
            this.xPos = args[0];
            this.yPos = args[1];
        }

        /// <summary>
        /// Sets the color of the Pen object to the given color.
        /// </summary>
        /// <param name="color">Color to set.</param>
        public void SetColor(Color color)
        {
            this.color = color;
            this.pen = new Pen(this.color);
        }

        /// <summary>
        /// Sets the value of the 'fill' boolean. This controls whether Shapes are drawn as outlines or filled.
        /// </summary>
        /// <param name="state">Used to set the boolean. 'on' translates to True, 'off' to False.</param>
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

        /// <summary>
        /// Essentailly sets the program back to it's original state; Fill off, Pen Color.Black and calls Reset() and Clear().
        /// </summary>
        public void RestoreDefaultState()
        {
            this.SetFill("off");
            this.pen = new Pen(Color.Black, DEFAULT_PEN_WIDTH);
            this.Reset();
            this.Clear();
        }
    }
}
