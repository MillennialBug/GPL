namespace GraphicalProgrammingLanguage
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.programBox = new System.Windows.Forms.TextBox();
            this.commandBox = new System.Windows.Forms.TextBox();
            this.runButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.exceptionBox = new System.Windows.Forms.TextBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.checkButton = new System.Windows.Forms.Button();
            this.commandException = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // programBox
            // 
            this.programBox.AcceptsReturn = true;
            this.programBox.Location = new System.Drawing.Point(8, 26);
            this.programBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.programBox.Multiline = true;
            this.programBox.Name = "programBox";
            this.programBox.Size = new System.Drawing.Size(241, 536);
            this.programBox.TabIndex = 0;
            this.programBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProgramBox_KeyDown);
            // 
            // commandBox
            // 
            this.commandBox.Location = new System.Drawing.Point(8, 636);
            this.commandBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.commandBox.Name = "commandBox";
            this.commandBox.Size = new System.Drawing.Size(241, 20);
            this.commandBox.TabIndex = 2;
            this.commandBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CommandBox_KeyDown);
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(83, 583);
            this.runButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(71, 38);
            this.runButton.TabIndex = 3;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(237, 583);
            this.saveButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(74, 38);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(315, 583);
            this.loadButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(76, 38);
            this.loadButton.TabIndex = 5;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // exceptionBox
            // 
            this.exceptionBox.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.exceptionBox.ForeColor = System.Drawing.Color.Red;
            this.exceptionBox.Location = new System.Drawing.Point(264, 26);
            this.exceptionBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.exceptionBox.Multiline = true;
            this.exceptionBox.Name = "exceptionBox";
            this.exceptionBox.Size = new System.Drawing.Size(241, 536);
            this.exceptionBox.TabIndex = 6;
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(157, 583);
            this.clearButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(76, 38);
            this.clearButton.TabIndex = 7;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // checkButton
            // 
            this.checkButton.Location = new System.Drawing.Point(8, 583);
            this.checkButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(71, 38);
            this.checkButton.TabIndex = 8;
            this.checkButton.Text = "Check";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // commandException
            // 
            this.commandException.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.commandException.ForeColor = System.Drawing.Color.Red;
            this.commandException.Location = new System.Drawing.Point(264, 636);
            this.commandException.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.commandException.Name = "commandException";
            this.commandException.Size = new System.Drawing.Size(241, 20);
            this.commandException.TabIndex = 9;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(1201, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 22);
            this.toolStripMenuItem1.Text = "File";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.fileToolStripMenuItem.Text = "Save";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox.Location = new System.Drawing.Point(517, 26);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(10);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(669, 627);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(1201, 669);
            this.Controls.Add(this.commandException);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.exceptionBox);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.commandBox);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.programBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox programBox;
        private System.Windows.Forms.TextBox commandBox;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.TextBox exceptionBox;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.TextBox commandException;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox;
    }
}

