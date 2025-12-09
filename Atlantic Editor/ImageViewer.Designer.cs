namespace Atlantic_Editor
{
	partial class ImageViewer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageViewer));
			picture = new PictureBox();
			((System.ComponentModel.ISupportInitialize)picture).BeginInit();
			SuspendLayout();
			// 
			// picture
			// 
			picture.Location = new Point(0, 0);
			picture.Name = "picture";
			picture.Size = new Size(512, 512);
			picture.TabIndex = 0;
			picture.TabStop = false;
			// 
			// ImageViewer
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(512, 512);
			Controls.Add(picture);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Name = "ImageViewer";
			Text = "Image Viewer";
			Paint += ImageViewer_Paint;
			((System.ComponentModel.ISupportInitialize)picture).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private PictureBox picture;
	}
}