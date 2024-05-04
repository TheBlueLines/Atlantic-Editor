using System.Drawing;
using System.Text;

namespace TTMC.WAD
{
	public class WAD
	{
		public List<Entry> entries = new();
		internal Stream stream;
		public WAD(Stream stream)
		{
			this.stream = stream;
			byte[] rawData = new byte[4];
			stream.Read(rawData, 0, rawData.Length);
			string szMagic = Encoding.UTF8.GetString(rawData);
			stream.Read(rawData, 0, rawData.Length);
			int nDir = BitConverter.ToInt32(rawData);
			stream.Read(rawData, 0, rawData.Length);
			int nDirOffset = BitConverter.ToInt32(rawData);
			if (szMagic == "WAD3" || szMagic == "IWAD")
			{
				for (int i = 0; i < nDir; i++)
				{
					entries.Add(new(stream, nDirOffset + i * (szMagic == "WAD3" ? 32 : 16), szMagic == "WAD3"));
				}
				return;
			}
			throw new("Unsupported file");
		}
		internal static string NullTerminated(byte[] data, int start = 0, int max = 0)
		{
			List<byte> bytes = (max != 0 ? data[start..][..max] : data[start..]).ToList();
			int count = bytes.Contains(0x00) ? bytes.IndexOf(0x00) : max;
			return Encoding.ASCII.GetString(data, start, count);
		}
		public void Close()
		{
			stream.Close();
			stream.Dispose();
		}
	}
	public class Entry
	{
		internal bool modern;
		internal Stream stream;
		internal int offset;
		public Entry(Stream stream, int offset, bool modern)
		{
			this.stream = stream;
			this.modern = modern;
			this.offset = offset;
		}
		internal int textureOffset
		{
			get
			{
				byte[] bytes = new byte[4];
				stream.Position = offset;
				stream.Read(bytes, 0, bytes.Length);
				return BitConverter.ToInt32(bytes);
			}
		}
		public Texture texture
		{
			get
			{
				return new(stream, this);
			}
		}
		public int diskSize
		{
			get
			{
				byte[] bytes = new byte[4];
				stream.Position = offset + 4;
				stream.Read(bytes, 0, bytes.Length);
				return BitConverter.ToInt32(bytes);
			}
		}
		public int size
		{
			get
			{
				byte[] bytes = new byte[4];
				stream.Position = offset + 8;
				stream.Read(bytes, 0, bytes.Length);
				return modern ? BitConverter.ToInt32(bytes) : size;
			}
		}
		public byte? type
		{
			get
			{
				stream.Position = offset + 12;
				int b = stream.ReadByte();
				return modern && b >= 0 ? (byte)b : null;
			}
		}
		public bool compression
		{
			get
			{
				stream.Position = offset + 13;
				int b = stream.ReadByte();
				return modern && b == 1;
			}
		}
		public string name
		{
			get
			{
				byte[] bytes = new byte[modern ? 16 : 8];
				stream.Position = offset + bytes.Length;
				stream.Read(bytes, 0, bytes.Length);
				return WAD.NullTerminated(bytes, 0, bytes.Length);
			}
		}
	}
	public class Texture
	{
		internal Stream stream;
		internal Entry entry;
		public Texture (Stream stream, Entry entry)
		{
			this.stream = stream;
			this.entry = entry;
		}
		internal int? mipOffset0
		{
			get
			{
				if (entry.type == 0x40 || entry.type == 0x43)
				{
					byte[] bytes = new byte[4];
					stream.Position = entry.textureOffset + 24;
					stream.Read(bytes, 0, bytes.Length);
					return BitConverter.ToInt32(bytes);
				}
				return null;
			}
		}
		internal int? mipOffset1
		{
			get
			{
				if (entry.type == 0x40 || entry.type == 0x43)
				{
					byte[] bytes = new byte[16];
					stream.Position = entry.textureOffset + 28;
					stream.Read(bytes, 0, bytes.Length);
					return BitConverter.ToInt32(bytes);
				}
				return null;
			}
		}
		internal int? mipOffset2
		{
			get
			{
				if (entry.type == 0x40 || entry.type == 0x43)
				{
					byte[] bytes = new byte[16];
					stream.Position = entry.textureOffset + 32;
					stream.Read(bytes, 0, bytes.Length);
					return BitConverter.ToInt32(bytes);
				}
				return null;
			}
		}
		internal int? mipOffset3
		{
			get
			{
				if (entry.type == 0x40 || entry.type == 0x43)
				{
					byte[] bytes = new byte[16];
					stream.Position = entry.textureOffset + 36;
					stream.Read(bytes, 0, bytes.Length);
					return BitConverter.ToInt32(bytes);
				}
				return null;
			}
		}
		public string? name
		{
			get
			{
				if (entry.type == 0x40 || entry.type == 0x43)
				{
					byte[] bytes = new byte[16];
					stream.Position = entry.textureOffset;
					stream.Read(bytes, 0, bytes.Length);
					return WAD.NullTerminated(bytes, entry.textureOffset);
				}
				return null;
			}
		}
		public List<Color>? palette
		{
			get
			{
				int paletteOffset = entry.textureOffset + entry.size - (256 * 3) - 2;
				byte[] bytes = new byte[256 * 3];
				stream.Position = paletteOffset;
				stream.Read(bytes, 0, bytes.Length);
				List<Color> colors = new();
				for (int j = 0; j < bytes.Length; j += 3)
				{
					colors.Add(Color.FromArgb(bytes[j], bytes[j + 1], bytes[j + 2]));
				}
				return colors;
			}
		}
		public int height
		{
			get
			{
				byte[] bytes = new byte[4];
				stream.Position = entry.textureOffset + (entry.type == 0x40 || entry.type == 0x43 ? 20 : 4);
				stream.Read(bytes, 0, bytes.Length);
				return BitConverter.ToInt32(bytes);
			}
		}
		public int width
		{
			get
			{
				byte[] bytes = new byte[4];
				stream.Position = entry.textureOffset + (entry.type == 0x40 || entry.type == 0x43 ? 16 : 0);
				stream.Read(bytes, 0, bytes.Length);
				return BitConverter.ToInt32(bytes);
			}
		}
		public byte[]? texture0
		{
			get
			{
				if ((entry.type == 0x40 || entry.type == 0x43) && mipOffset0 != null)
				{
					byte[] texture = new byte[width * height];
					stream.Position = entry.textureOffset + mipOffset0.Value;
					stream.Read(texture, 0, texture.Length);
					return texture;
				}
				byte[] bytes = new byte[width * height];
				stream.Position = entry.textureOffset + 8;
				stream.Read(bytes, 0, bytes.Length);
				return bytes;
			}
		}
		public byte[]? texture1
		{
			get
			{
				if (mipOffset1 != null)
				{
					byte[] bytes = new byte[width / 2 * height / 2];
					stream.Position = entry.textureOffset + mipOffset1.Value;
					stream.Read(bytes, 0, bytes.Length);
					return bytes;
				}
				return null;
			}
		}
		public byte[]? texture2
		{
			get
			{
				if (mipOffset2 != null)
				{
					byte[] bytes = new byte[width / 4 * height / 4];
					stream.Position = entry.textureOffset + mipOffset2.Value;
					stream.Read(bytes, 0, bytes.Length);
					return bytes;
				}
				return null;
			}
		}
		public byte[]? texture3
		{
			get
			{
				if (mipOffset3 != null)
				{
					byte[] bytes = new byte[width / 8 * height / 8];
					stream.Position = entry.textureOffset + mipOffset3.Value;
					stream.Read(bytes, 0, bytes.Length);
					return bytes;
				}
				return null;
			}
		}
	}
}