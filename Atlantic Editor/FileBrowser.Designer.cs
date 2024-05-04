namespace Atlantic_Editor
{
    partial class FileBrowser
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileBrowser));
			tree = new TreeView();
			menu = new MenuStrip();
			fileToolStripMenuItem = new ToolStripMenuItem();
			openToolStripMenuItem = new ToolStripMenuItem();
			exportToolStripMenuItem = new ToolStripMenuItem();
			selectedToolStripMenuItem = new ToolStripMenuItem();
			allToolStripMenuItem = new ToolStripMenuItem();
			aboutToolStripMenuItem = new ToolStripMenuItem();
			menu.SuspendLayout();
			SuspendLayout();
			// 
			// tree
			// 
			tree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			tree.Font = new Font("Roboto", 20.25F);
			tree.Location = new Point(12, 27);
			tree.Name = "tree";
			tree.Size = new Size(776, 411);
			tree.TabIndex = 0;
			// 
			// menu
			// 
			menu.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, exportToolStripMenuItem, aboutToolStripMenuItem });
			menu.Location = new Point(0, 0);
			menu.Name = "menu";
			menu.Size = new Size(800, 24);
			menu.TabIndex = 1;
			menu.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem });
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Size = new Size(37, 20);
			fileToolStripMenuItem.Text = "File";
			// 
			// openToolStripMenuItem
			// 
			openToolStripMenuItem.Name = "openToolStripMenuItem";
			openToolStripMenuItem.Size = new Size(103, 22);
			openToolStripMenuItem.Text = "Open";
			openToolStripMenuItem.Click += openToolStripMenuItem_Click;
			// 
			// exportToolStripMenuItem
			// 
			exportToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { selectedToolStripMenuItem, allToolStripMenuItem });
			exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			exportToolStripMenuItem.Size = new Size(52, 20);
			exportToolStripMenuItem.Text = "Export";
			// 
			// selectedToolStripMenuItem
			// 
			selectedToolStripMenuItem.Name = "selectedToolStripMenuItem";
			selectedToolStripMenuItem.Size = new Size(118, 22);
			selectedToolStripMenuItem.Text = "Selected";
			selectedToolStripMenuItem.Click += selectedToolStripMenuItem_Click;
			// 
			// allToolStripMenuItem
			// 
			allToolStripMenuItem.Name = "allToolStripMenuItem";
			allToolStripMenuItem.Size = new Size(118, 22);
			allToolStripMenuItem.Text = "All";
			allToolStripMenuItem.Click += allToolStripMenuItem_Click;
			// 
			// aboutToolStripMenuItem
			// 
			aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			aboutToolStripMenuItem.Size = new Size(52, 20);
			aboutToolStripMenuItem.Text = "About";
			aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
			// 
			// FileBrowser
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(tree);
			Controls.Add(menu);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menu;
			Name = "FileBrowser";
			Text = "Atlantic Editor";
			menu.ResumeLayout(false);
			menu.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TreeView tree;
        private MenuStrip menu;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem selectedToolStripMenuItem;
        private ToolStripMenuItem allToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
		private ToolStripMenuItem aboutToolStripMenuItem;
	}
}