using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicalProgrammingLanguage
{
    public partial class Form1 : Form
    {
        const int PaintingBitmapWidth = 1240;
        const int PaintingBitmapHeight = 1240;

        Bitmap PaintingBitmap = new Bitmap(PaintingBitmapWidth, PaintingBitmapHeight);
        Canvas PaintingCanvas;
        ShapeFactory ShapeFactory;
        Graphics g;
        Parser parser;

        public Form1()
        {
            InitializeComponent();
            PaintingCanvas = new Canvas(Graphics.FromImage(PaintingBitmap));
            this.g = PaintingCanvas.GetGraphics();
            this.ShapeFactory = new ShapeFactory();
            parser = new Parser(PaintingCanvas);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parser.parseLines(programBox.Lines);
            Refresh();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImageUnscaled(PaintingBitmap, 0, 0);
        }

        private void CommandBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String Command = CommandBox.Text.Trim().ToLower();
                if (Command.Equals("line") == true)
                {
                    PaintingCanvas.DrawLine(200, 200);
                    Console.WriteLine("Line");
                }
                else if (Command.Equals("circle") == true)
                {
                    Shape s = (Shape) this.ShapeFactory.getShape(Command);
                    s.Set(Color.Red, 50, 50, 100);
                    s.Draw(this.g);

                }

                CommandBox.Text = "";
                Refresh();
            }
            
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Check code for errors
            }
        }
    }
}
