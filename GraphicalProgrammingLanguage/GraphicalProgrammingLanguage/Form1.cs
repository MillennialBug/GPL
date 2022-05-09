using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GraphicalProgrammingLanguage
{
    public partial class Form1 : Form
    {
        const int PaintingBitmapWidth = 1000;
        const int PaintingBitmapHeight = 1000;

        bool cleanProgram = true;

        Bitmap paintingBitmap = new Bitmap(PaintingBitmapWidth, PaintingBitmapHeight);
        Canvas paintingCanvas;
        Parser parser;

        public Form1()
        {
            InitializeComponent();
            paintingCanvas = Canvas.GetCanvas();
            paintingCanvas.SetGraphics(Graphics.FromImage(paintingBitmap));
            parser = Parser.GetParser();
            parser.SetCanvas(paintingCanvas);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            paintingCanvas.RestoreDefaultState();
            CheckButton_Click(sender, e);
            if (cleanProgram)
                parser.ParseLines(programBox.Lines, true);
            else
                MessageBox.Show("The program contains errors.\n\nPlease correct before running.", "Errors Found");
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
                if (commandBox.Text.ToLower().Equals("run"))
                    RunButton_Click(sender, e);
                else
                {
                    commandException.Lines = parser.ParseLines(commandBox.Lines, true);
                }
                
                if (commandException.Text == String.Empty) commandBox.Text = "";
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
            DialogResult res = MessageBox.Show("This will clear the program window and Canvas.\nContinue?", "Are you sure?", MessageBoxButtons.YesNo);
            if(res.Equals(DialogResult.Yes))
            {
                programBox.Clear();
                exceptionBox.Clear();
                parser.ParseLines("clear\nreset".Split('\n'), true);
                Refresh();
            }  
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            //Check code for errors
            exceptionBox.Lines = parser.ParseLines(programBox.Lines, false);
            CheckProgramIsClean();
            Refresh();
        }

        private void CheckProgramIsClean()
        {
            cleanProgram = true;
            foreach (String s in exceptionBox.Lines)
            {
                if (!s.Equals(String.Empty)) cleanProgram = false;
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveButton_Click(sender, e);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadButton_Click(sender, e);
        }
    }
}
