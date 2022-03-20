using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GraphicalProgrammingLanguage
{
    public partial class Form1 : Form
    {
        const int PaintingBitmapWidth = 1240;
        const int PaintingBitmapHeight = 1240;

        Bitmap paintingBitmap = new Bitmap(PaintingBitmapWidth, PaintingBitmapHeight);
        Canvas paintingCanvas;
        Parser parser;

        public Form1()
        {
            InitializeComponent();
            paintingCanvas = new Canvas(Graphics.FromImage(paintingBitmap));
            parser = new Parser(paintingCanvas);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            paintingCanvas.RestoreDefaultState();
            exceptionBox.Lines = parser.parseLines(programBox.Lines, true);
            Refresh();
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImageUnscaled(paintingBitmap, 0, 0);
        }

        private void CommandBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                parser.parseLines(commandBox.Lines, true);
                commandBox.Text = "";
                Refresh();
            }
            
        }

        private void ProgramBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CheckButton_Click(sender, e);
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
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

        private void SaveButton_Click(object sender, EventArgs e)
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

        private void ClearButton_Click(object sender, EventArgs e)
        {
            programBox.Clear();
            exceptionBox.Clear();
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            //Check code for errors
            exceptionBox.Lines = parser.parseLines(programBox.Lines, false);
            Refresh();
        }
    }
}
