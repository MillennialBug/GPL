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

        /// <summary>
        /// Constructor. Initializes component. Gets Singleton Canvas and Parser instances. Sets Graphics context for Canvas.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            paintingCanvas = Canvas.GetCanvas();
            paintingCanvas.SetGraphics(Graphics.FromImage(paintingBitmap));
            parser = Parser.GetParser();
            parser.SetCanvas(paintingCanvas);
        }

        /// <summary>
        /// Runs a program entered into the ProgramBox.
        /// First resets canvas to default settings. I.E. Pen black, 0,0 cursor co-ords and blank Canvas.
        /// Also checks for errors in program first and does not execute if any are found.
        /// </summary>
        /// <param name="sender">Object that sent to event</param>
        /// <param name="e">Event args</param>
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

        /// <summary>
        /// Paints the contents of the paintingBitmap onto the Canvas.
        /// </summary>
        /// <param name="sender">Object that sent to event</param>
        /// <param name="e">Event args</param>
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImageUnscaled(paintingBitmap, 0, 0);
        }

        /// <summary>
        /// Listens for a keyboard key press when in the single line command box, specifically checking for Enter key.
        /// Runs the main programBox program if 'run' command is entered into the command box, 
        /// otherwise runs the single line command.
        /// </summary>
        /// <param name="sender">Object that sent to event</param>
        /// <param name="e">Event args</param>
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

        /// <summary>
        /// Listening for a Keyboard key in the programBox. If Enter key is pressed, the entire program is checked for errors.
        /// </summary>
        /// <param name="sender">Object that sent to event</param>
        /// <param name="e">Event args</param>
        private void ProgramBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CheckButton_Click(sender, e);
            }
        }

        /// <summary>
        /// Loads a file into the programBox.
        /// </summary>
        /// <param name="sender">Object that sent to event</param>
        /// <param name="e">Event args</param>
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

        /// <summary>
        /// Saves a program entered into the programBox.
        /// </summary>
        /// <param name="sender">Object that sent to event</param>
        /// <param name="e">Event args</param>
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

        /// <summary>
        /// Clears the programBox and the Canvas. Also runs the clear and reset commands (Essentially resets the program to it's default state).
        /// </summary>
        /// <param name="sender">Object that sent to event</param>
        /// <param name="e">Event args</param>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("This will clear the program window and Canvas.\nContinue?", "Are you sure?", MessageBoxButtons.YesNo);
            if(res.Equals(DialogResult.Yes))
            {
                programBox.Clear();
                exceptionBox.Clear();
                paintingCanvas.RestoreDefaultState();
                Refresh();
            }  
        }

        /// <summary>
        /// Checks the program entered into programBox for errors. Pretty much redundant as line-by-line checking is available.
        /// </summary>
        /// <param name="sender">Object that sent to event</param>
        /// <param name="e">Event args</param>
        private void CheckButton_Click(object sender, EventArgs e)
        {
            //Check code for errors
            exceptionBox.Lines = parser.ParseLines(programBox.Lines, false);
            CheckProgramIsClean();
            Refresh();
        }

        /// <summary>
        /// Loops through the lines in the exceptionBox to determine if any errors are present. Returns true if there is.
        /// </summary>
        private void CheckProgramIsClean()
        {
            cleanProgram = true;
            foreach (String s in exceptionBox.Lines)
            {
                if (!s.Equals(String.Empty)) cleanProgram = false;
            }
        }

        /// <summary>
        /// Menu item that just calls the SaveButton_Click method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveButton_Click(sender, e);
        }

        /// <summary>
        /// Menu item that just calls the LoadButton_Click method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadButton_Click(sender, e);
        }
    }
}
