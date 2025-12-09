using System.Text;

namespace TTMC.ORE
{
	public class ORE
	{
		public Dir[] dirs = [];
		public Content[] contents = [];
		public ORE(Stream? stream = null)
		{
			if (stream != null)
			{
				OpenStream(stream);
			}
		}
		public void OpenStream(Stream stream)
		{
			byte[] raw = new byte[4];
			stream.ReadExactly(raw, 0, raw.Length);
			int contentStart = BitConverter.ToInt32(raw);
			stream.ReadExactly(raw, 0, raw.Length);
			int numberOfDirectories = BitConverter.ToInt32(raw);
			dirs = new Dir[numberOfDirectories];
			for (int i = 0; i < numberOfDirectories; i++)
			{
				string name = ReadString(stream);
				stream.ReadExactly(raw, 0, raw.Length);
				int offset = BitConverter.ToInt32(raw);
				dirs[i] = new(stream, name, offset);
			}
			stream.ReadExactly(raw, 0, raw.Length);
			uint numberOfContents = BitConverter.ToUInt32(raw);
			contents = new Content[numberOfContents];
			for (int i = 0; i < numberOfContents; i++)
			{
				string contentName = ReadString(stream);
				stream.ReadExactly(raw, 0, raw.Length);
				int contentOffset = BitConverter.ToInt32(raw);
				stream.ReadExactly(raw, 0, raw.Length);
				int contentSize = BitConverter.ToInt32(raw);
				contents[i] = new(stream, contentName, contentStart + contentOffset, contentSize);
			}
		}
		internal static string ReadString(Stream stream)
		{
			string name = string.Empty;
			int x = -1;
			while (x != 0)
			{
				x = stream.ReadByte();
				if (x != 0)
				{
					name += (char)x;
				}
			}
			return name;
		}
		public byte[] ToByteArray()
		{
			List<byte> file = [.. BitConverter.GetBytes(dirs.Length)];
			int[] returnDirs = new int[dirs.Length];
			int[] returnConents = new int[contents.Length];
			for (int i = 0; i < dirs.Length; i++)
			{
				Dir dir = dirs[i];
				file.AddRange(Encoding.ASCII.GetBytes(dir.name));
				file.AddRange(new byte[5]);
				returnDirs[i] = file.Count;
			}
			file.AddRange(BitConverter.GetBytes(contents.Length));
			int checkpoint = file.Count;
			file.InsertRange(0, BitConverter.GetBytes(checkpoint));
			for (int i = 0; i < contents.Length; i++)
			{
				Content content = contents[i];
				file.AddRange(Encoding.ASCII.GetBytes(content.name));
				file.Add(0x00);
				returnConents[i] = file.Count;
				file.AddRange(new byte[8]);
			}
			for (int i = 0; i < contents.Length; i++)
			{
				Content content = contents[i];
				file.InsertRange(returnConents[i], BitConverter.GetBytes(file.Count - checkpoint));
				file.InsertRange(returnConents[i] + 4, BitConverter.GetBytes(content.size));
				file.RemoveRange(returnConents[i] + 8, 8);
				file.AddRange(content.data);
			}
			for (int i = 0; i < dirs.Length; i++)
			{
				file.InsertRange(returnDirs[i], BitConverter.GetBytes(file.Count));
				file.RemoveRange(returnDirs[i] + 4, 4);
				Dir dir = dirs[i];
				file.AddRange(BitConverter.GetBytes((ulong)8));
				int dirStart = file.Count;
				Content[] dirContents = dir.contents;
				file.AddRange(BitConverter.GetBytes(dirContents.Length));
				returnConents = new int[dirContents.Length];
				for (int j = 0; j < dirContents.Length; j++)
				{
					Content content = dirContents[j];
					file.AddRange(Encoding.ASCII.GetBytes(content.name));
					file.Add(0x00);
					returnConents[j] = file.Count;
					file.AddRange(new byte[8]);
				}
				for (int j = 0; j < dirContents.Length; j++)
				{
					Content content = dirContents[j];
					file.InsertRange(returnConents[j], BitConverter.GetBytes(file.Count - dirStart));
					file.InsertRange(returnConents[j] + 4, BitConverter.GetBytes(content.size));
					file.RemoveRange(returnConents[j] + 8, 8);
					file.AddRange(content.data);
				}
			}
			return file.ToArray();
		}
	}
	public class Dir
	{
		private Stream? stream;
		public string name;
		internal int offset;
		private Content[]? main;
		public Dir(Stream stream, string name, int offset)
		{
			this.stream = stream;
			this.name = name;
			this.offset = offset;
		}
		public Dir(string name, Content[] contents)
		{
			this.name = name;
			main = contents;
		}
		public Content[] contents
		{
			get
			{
				if (main == null)
				{
					if (stream != null)
					{
						stream.Position = offset;
						byte[] raw = new byte[8];
						stream.ReadExactly(raw, 0, raw.Length);
						ulong signature = BitConverter.ToUInt64(raw);
						if (signature != 8)
						{
							throw new Exception("Invalid ORE Dir signature.");
						}
						long dirStart = stream.Position;
						raw = new byte[4];
						stream.ReadExactly(raw, 0, raw.Length);
						uint numberOfContents = BitConverter.ToUInt32(raw);
						main = new Content[numberOfContents];
						for (int i = 0; i < numberOfContents; i++)
						{
							string contentName = ORE.ReadString(stream);
							stream.ReadExactly(raw, 0, raw.Length);
							int contentOffset = BitConverter.ToInt32(raw);
							stream.ReadExactly(raw, 0, raw.Length);
							int contentSize = BitConverter.ToInt32(raw);
							main[i] = new(stream, contentName, (int)(dirStart + contentOffset), contentSize);
						}
					}
					else
					{
						throw new Exception("Stream is null.");
					}
				}
				return main;
			}
		}
	}
	public class Content
	{
		private Stream? stream;
		public string name;
		internal int offset;
		public int syze;
		private byte[]? main;
		public Content(Stream stream, string name, int offset, int size)
		{
			this.stream = stream;
			this.name = name;
			this.offset = offset;
			syze = size;
		}
		public Content(string name, byte[] data)
		{
			this.name = name;
			main = data;
		}
		public byte[] data
		{
			get
			{
				if (main == null)
				{
					if (stream != null)
					{
						stream.Position = offset;
						byte[] raw = new byte[size];
						stream.ReadExactly(raw, 0, raw.Length);
						return raw;
					}
					else
					{
						throw new Exception("Stream is null.");
					}
				}
				return main;
			}
		}
		public int size
		{
			get
			{
				if (main != null)
				{
					return main.Length;
				}
				return syze;
			}
		}
	}
}
