namespace Atlantic_Editor
{
	partial class TextureBrowser
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextureBrowser));
			list = new ListBox();
			picture = new PictureBox();
			search = new TextBox();
			progressBar1 = new ProgressBar();
			menuStrip1 = new MenuStrip();
			openToolStripMenuItem = new ToolStripMenuItem();
			saveToolStripMenuItem = new ToolStripMenuItem();
			imagesToolStripMenuItem = new ToolStripMenuItem();
			selectedToolStripMenuItem = new ToolStripMenuItem();
			toolsToolStripMenuItem = new ToolStripMenuItem();
			colorToolStripMenuItem = new ToolStripMenuItem();
			openFileDialog1 = new OpenFileDialog();
			folderBrowserDialog1 = new FolderBrowserDialog();
			saveFileDialog1 = new SaveFileDialog();
			colorDialog1 = new ColorDialog();
			((System.ComponentModel.ISupportInitialize)picture).BeginInit();
			menuStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// list
			// 
			list.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			list.Font = new Font("Roboto", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
			list.FormattingEnabled = true;
			list.ItemHeight = 33;
			list.Location = new Point(12, 73);
			list.Name = "list";
			list.Size = new Size(400, 466);
			list.TabIndex = 0;
			list.SelectedIndexChanged += listBox1_SelectedIndexChanged;
			// 
			// picture
			// 
			picture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			picture.BorderStyle = BorderStyle.FixedSingle;
			picture.Location = new Point(418, 27);
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
			search.Size = new Size(400, 40);
			search.TabIndex = 2;
			search.TextChanged += search_TextChanged;
			// 
			// progressBar1
			// 
			progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			progressBar1.Enabled = false;
			progressBar1.Location = new Point(12, 545);
			progressBar1.Name = "progressBar1";
			progressBar1.Size = new Size(918, 23);
			progressBar1.TabIndex = 3;
			// 
			// menuStrip1
			// 
			menuStrip1.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, toolsToolStripMenuItem });
			menuStrip1.Location = new Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Size = new Size(942, 24);
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
			// saveToolStripMenuItem
			// 
			saveToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { imagesToolStripMenuItem, selectedToolStripMenuItem });
			saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			saveToolStripMenuItem.Size = new Size(52, 20);
			saveToolStripMenuItem.Text = "Export";
			// 
			// imagesToolStripMenuItem
			// 
			imagesToolStripMenuItem.Name = "imagesToolStripMenuItem";
			imagesToolStripMenuItem.Size = new Size(118, 22);
			imagesToolStripMenuItem.Text = "All";
			imagesToolStripMenuItem.Click += imagesToolStripMenuItem_Click;
			// 
			// selectedToolStripMenuItem
			// 
			selectedToolStripMenuItem.Name = "selectedToolStripMenuItem";
			selectedToolStripMenuItem.Size = new Size(118, 22);
			selectedToolStripMenuItem.Text = "Selected";
			selectedToolStripMenuItem.Click += selectedToolStripMenuItem_Click;
			// 
			// toolsToolStripMenuItem
			// 
			toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { colorToolStripMenuItem });
			toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			toolsToolStripMenuItem.Size = new Size(47, 20);
			toolsToolStripMenuItem.Text = "Tools";
			// 
			// colorToolStripMenuItem
			// 
			colorToolStripMenuItem.Name = "colorToolStripMenuItem";
			colorToolStripMenuItem.Size = new Size(180, 22);
			colorToolStripMenuItem.Text = "Filter";
			colorToolStripMenuItem.Click += filterToolStripMenuItem_Click;
			// 
			// openFileDialog1
			// 
			openFileDialog1.FileName = "openFileDialog1";
			openFileDialog1.Filter = "WAD3|*.wad|All files|*.*";
			// 
			// saveFileDialog1
			// 
			saveFileDialog1.Filter = "Bitmap Image|*.bmp|Portable Network Graphic|*.png|JPEG Image|*.jpeg|Icon File|*.ico|Windows Metafile|*.wmf|Graphical Interchange Format File|*.gif|Tagged Image File Format|*.tiff|All files|*.*";
			// 
			// Main
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(942, 576);
			Controls.Add(progressBar1);
			Controls.Add(search);
			Controls.Add(picture);
			Controls.Add(list);
			Controls.Add(menuStrip1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStrip1;
			Name = "Main";
			Text = "Atlantic Editor";
			FormClosing += Main_FormClosing;
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
		private ToolStripMenuItem saveToolStripMenuItem;
		private ToolStripMenuItem imagesToolStripMenuItem;
		private FolderBrowserDialog folderBrowserDialog1;
		private ToolStripMenuItem selectedToolStripMenuItem;
		private SaveFileDialog saveFileDialog1;
		private ToolStripMenuItem toolsToolStripMenuItem;
		private ToolStripMenuItem colorToolStripMenuItem;
		private ColorDialog colorDialog1;
	}
}