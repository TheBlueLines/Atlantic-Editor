namespace Atlantic_Editor
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            list = new ListBox();
            picture = new PictureBox();
            search = new TextBox();
            progressBar1 = new ProgressBar();
            menuStrip1 = new MenuStrip();
            openToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)picture).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // list
            // 
            list.Font = new Font("Roboto", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            list.FormattingEnabled = true;
            list.ItemHeight = 33;
            list.Location = new Point(12, 73);
            list.Name = "list";
            list.Size = new Size(200, 466);
            list.TabIndex = 0;
            list.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // picture
            // 
            picture.BackgroundImageLayout = ImageLayout.Stretch;
            picture.BorderStyle = BorderStyle.FixedSingle;
            picture.Location = new Point(217, 27);
            picture.Name = "picture";
            picture.Size = new Size(512, 512);
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            picture.TabIndex = 1;
            picture.TabStop = false;
            picture.Paint += picture_Paint;
            // 
            // search
            // 
            search.Font = new Font("Roboto", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            search.Location = new Point(12, 27);
            search.Name = "search";
            search.Size = new Size(200, 40);
            search.TabIndex = 2;
            search.TextChanged += search_TextChanged;
            // 
            // progressBar1
            // 
            progressBar1.Enabled = false;
            progressBar1.Location = new Point(11, 545);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(718, 23);
            progressBar1.TabIndex = 3;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(741, 24);
            menuStrip1.TabIndex = 4;
            menuStrip1.Text = "menuStrip1";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(48, 20);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.Filter = "WAD3|*.wad|All files|*.*";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(741, 576);
            Controls.Add(progressBar1);
            Controls.Add(search);
            Controls.Add(picture);
            Controls.Add(list);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "Main";
            Text = "Atlantic Editor";
            ((System.ComponentModel.ISupportInitialize)picture).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox list;
        private PictureBox picture;
        private TextBox search;
        private ProgressBar progressBar1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem openToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
    }
}