using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using TTMC.WAD;

namespace Atlantic_Editor
{
	public partial class TextureBrowser : Form
	{
		private Color reColor = Color.White;
		private WAD? wad = null;
		public TextureBrowser(string? firstLoad = null)
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
				for (int y = 0; y < texture.height; y++)
				{
					for (int x = 0; x < texture.width; x++)
					{
						int index = nzx[y * texture.width + x];
						Color color = palette[index];
						image.SetPixel(x, y, reColor == Color.White ? color : Color.FromArgb(255, int.Clamp(color.R - 255 + reColor.R, 0, 255), int.Clamp(color.G - 255 + reColor.G, 0, 255), int.Clamp(color.B - 255 + reColor.B, 0, 255)));
					}
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
					switch (Path.GetExtension(saveFileDialog1.FileName).ToLower())
					{
						case ".bmp":
							selected.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
							return;
						case ".png":
							selected.Save(saveFileDialog1.FileName, ImageFormat.Png);
							return;
						case ".jpeg":
							selected.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
							return;
						case ".ico":
							selected.Save(saveFileDialog1.FileName, ImageFormat.Icon);
							return;
						case ".wmf":
							selected.Save(saveFileDialog1.FileName, ImageFormat.Wmf);
							return;
						case ".gif":
							selected.Save(saveFileDialog1.FileName, ImageFormat.Gif);
							return;
						case ".tiff":
							selected.Save(saveFileDialog1.FileName, ImageFormat.Tiff);
							return;
						default:
							selected.Save(saveFileDialog1.FileName);
							return;
					}

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
					if (entry != default)
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
		private void Main_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (wad != null)
			{
				wad.Close();
			}
		}
		private void filterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				reColor = colorDialog1.Color;
				listBox1_SelectedIndexChanged(sender, e);
			}
		}
	}
}