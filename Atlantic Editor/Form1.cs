using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using TTMC.WAD;

namespace Atlantic_Editor
{
	public partial class Main : Form
	{
		private WAD? wad = null;
		public Main(string? firstLoad = null)
		{
			InitializeComponent();
			if (firstLoad != null)
			{
				LoadFile(firstLoad);
			}
		}
		private void LoadFile(string path)
		{
			list.Items.Clear();
			Stream stream = File.OpenRead(path);
			wad = new(stream);
			ReloadList();
		}
		public void ReloadList(string? search = null)
		{
			if (wad != null)
			{
				list.Items.Clear();
				foreach (Entry entry in wad.entries)
				{
					if (!string.IsNullOrEmpty(entry.name) && (string.IsNullOrEmpty(search) || entry.name.ToLower().Contains(search.ToLower())))
					{
						list.Items.Add(entry.name);
					}
				}
			}
		}
		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			Bitmap? image = SelectedImage();
			if (image != null)
			{
				picture.Image = image;
			}
		}
		private Bitmap MakeImage(Texture texture)
		{
			Bitmap image = new Bitmap(texture.width, texture.height);
			List<Color>? palette = texture.palette;
			byte[]? nzx = texture.texture0;
			if (nzx != null && palette != null)
			{
				for (int i = 0; i < texture.height * texture.width; i++)
				{
					image.SetPixel(i % texture.width, i / texture.width, palette[nzx[i]]);
				}
			}
			return image;
		}
		private void picture_Paint(object sender, PaintEventArgs e)
		{
			if (picture.Image != null)
			{
				e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
				e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
				e.Graphics.DrawImage(picture.Image, new Rectangle(0, 0, picture.Width, picture.Height), 0, 0, picture.Image.Width, picture.Image.Height, GraphicsUnit.Pixel);
			}
		}
		private void search_TextChanged(object sender, EventArgs e)
		{
			ReloadList(search.Text);
		}
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				LoadFile(openFileDialog1.FileName);
			}
		}
		private void imagesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				Task task = new(() => SaveImagesToFolder(folderBrowserDialog1.SelectedPath));
				task.Start();
			}
		}
		private void selectedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			saveFileDialog1.FileName = list.SelectedItem as string;
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				Bitmap? selected = SelectedImage();
				if (selected != null)
				{
					selected.Save(saveFileDialog1.FileName);
				}
			}
		}
		private Bitmap? SelectedImage()
		{
			if (wad != null && list.SelectedIndex >= 0)
			{
				string? nzx = list.SelectedItem as string;
				if (!string.IsNullOrEmpty(nzx))
				{
					Entry? entry = wad.entries.Where(x => x.name == nzx).FirstOrDefault();
					if (entry != null)
					{
						Texture? texture = entry.texture;
						if (texture != null)
						{
							return MakeImage(texture);
						}
					}
				}
			}
			return null;
		}
		private void SaveImagesToFolder(string path)
		{
			if (wad != null)
			{
				progressBar1.Enabled = true;
				int all = wad.entries.Count;
				for (int i = 0; i < all; i++)
				{
					Texture? texture = wad.entries[i].texture;
					if (texture != null)
					{
						Bitmap image = MakeImage(texture);
						string temp = path + Path.DirectorySeparatorChar + texture.name + ".bmp";
						image.Save(temp, ImageFormat.Bmp);
						progressBar1.Value = (int)((float)i / (float)all * 100);
					}
				}
				progressBar1.Enabled = false;
				progressBar1.Value = 0;
			}
		}
	}
}