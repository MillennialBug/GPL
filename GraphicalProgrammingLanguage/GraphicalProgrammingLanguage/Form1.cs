using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        Bitmap paintingBitmap = new Bitmap(PaintingBitmapWidth, PaintingBitmapHeight);
        Canvas paintingCanvas;
        ShapeFactory shapeFactory;
        Graphics g;
        Parser parser;

        public Form1()
        {
            InitializeComponent();
            paintingCanvas = new Canvas(Graphics.FromImage(paintingBitmap));
            this.g = paintingCanvas.GetGraphics();
            this.shapeFactory = new ShapeFactory();
            parser = new Parser(paintingCanvas);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            paintingCanvas.RestoreDefaultState();
            parser.parseLines(programBox.Lines);
            Refresh();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImageUnscaled(paintingBitmap, 0, 0);
        }

        private void CommandBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                parser.parseLines(CommandBox.Lines);
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

        private void load_button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.Filter = "gpl files (*.gpl)|*.gpl|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    programBox.Text = File.ReadAllText(openFileDialog.FileName);
                }
            }
            Refresh();
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveFileDialog.Filter = "gpl files (*.gpl)|*.gpl|All files (*.*)|*.*";
                saveFileDialog.RestoreDirectory = true;

                if(saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, programBox.Text);
                }
            }
        }
    }
}
