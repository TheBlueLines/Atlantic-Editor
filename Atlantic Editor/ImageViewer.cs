using System.Drawing.Drawing2D;

namespace Atlantic_Editor
{
	public partial class ImageViewer : Form
	{
		public ImageViewer(Image image)
		{
			InitializeComponent();
			picture.Image = image;
			picture.Refresh();
		}
		private void ImageViewer_Paint(object sender, PaintEventArgs e)
		{
			if (picture.Image != null)
			{
				e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
				e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
				e.Graphics.DrawImage(picture.Image, new Rectangle(0, 0, picture.Width, picture.Height), 0, 0, picture.Image.Width, picture.Image.Height, GraphicsUnit.Pixel);
			}
		}
	}
}