using System.Text;
using System.Xml.Linq;

namespace TTMC.WPK
{
	public class WPK
	{
		private string codename = "WPAK";
		private int codeversion = 1;
		private string? path = null;
		public string? signature = null;
		public int? version = null;
		public int? treeStart = null;
		public Stream? stream = null;
		private Dictionary<string, Content> contents = new();
		private Dictionary<string, byte[]> cache = new();
		public WPK(Stream? stream = null)
		{
			if (stream != null)
			{
				Load(stream);
			}
		}
		public void Close()
		{
			if (stream != null)
			{
				stream.Close();
				stream.Dispose();
			}
		}
		public bool AddContent(string name, byte[] data)
		{
			RemoveContent(name);
			return cache.TryAdd(name, data);
		}
		public bool AddContent(string name, string text)
		{
			return AddContent(name, Encoding.UTF8.GetBytes(text));
		}
		public void SetContent(string name, byte[] data)
		{
			RemoveContent(name);
			cache[name] = data;
		}
		public void SetContent(string name, string text)
		{
			SetContent(name, Encoding.UTF8.GetBytes(text));
		}
		public bool RemoveContent(string name)
		{
			if (cache.ContainsKey(name))
			{
				cache.Remove(name);
				return true;
			}
			if (contents.ContainsKey(name))
			{
				contents.Remove(name);
				return true;
			}
			return false;
		}
		public bool ContainsContent(string name)
		{
			return cache.ContainsKey(name) || contents.ContainsKey(name);
		}
		public byte[] GetContent(string name)
		{
			if (cache.ContainsKey(name))
			{
				return cache[name];
			}
			else if (contents.ContainsKey(name))
			{
				return contents[name].data;
			}
			throw new("Content not found");
		}
		public void Load(Stream stream)
		{
			this.stream = stream;
			byte[] data = new byte[4];
			stream.Read(data, 0, data.Length);
			signature = Encoding.UTF8.GetString(data);
			stream.Read(data, 0, data.Length);
			version = BitConverter.ToInt32(data);
			stream.Read(data, 0, data.Length);
			treeStart = BitConverter.ToInt32(data);
			if (signature == codename && version == codeversion)
			{
				LoadContents();
			}
		}
		public List<byte> Enhance(string prefix, string[] keys, List<byte> bytes)
		{
			List<byte> tree = new();
			for (int i = 0; i < keys.Length; i++)
			{
				string key = keys[i];
				if (key.Contains('\\'))
				{
					string master = key[prefix.Length..];
					string root = master[..(master.IndexOf('\\') + 1)];
					tree.AddRange(Encoding.UTF8.GetBytes(root));
					List<byte> temp = Enhance(root, keys.Where(x => x.StartsWith(root)).ToArray(), bytes);
					tree.AddRange(temp);
					i += temp.Count - 1;
				}
				else
				{
					byte[] data = cache[key];
					tree.AddRange([.. Encoding.UTF8.GetBytes(key), 0]);
					tree.AddRange(BitConverter.GetBytes(12 + bytes.Count));
					tree.AddRange(BitConverter.GetBytes(data.Length));
					bytes.AddRange(data);
				}
			}
			return tree;
		}
		public void Save(string path)
		{
			Stream fileStream = path == this.path && stream != null ? stream : File.Open(path, FileMode.OpenOrCreate);
			foreach (KeyValuePair<string, Content> content in contents)
			{
				if (!cache.ContainsKey(content.Key))
				{
					cache[content.Key] = content.Value.data;
				}
			}
			List<byte> bytes = new();
			List<byte> tree = Enhance(string.Empty, cache.Keys.ToArray(), bytes);
			foreach (KeyValuePair<string, byte[]> content in cache.OrderBy(x => x.Key))
			{
				string name = content.Key;
				if (name.Contains('.'))
				{
					byte[] data = content.Value;
					tree.AddRange([.. Encoding.UTF8.GetBytes(name), 0]);
					tree.AddRange(BitConverter.GetBytes(12 + bytes.Count));
					tree.AddRange(BitConverter.GetBytes(data.Length));
					bytes.AddRange(data);
				}
				else
				{
					//tree.AddRange([.. Encoding.UTF8.GetBytes(name), 0]);
					//bytes.AddRange(content.Value);
				}
			}
			fileStream.Position = 0;
			fileStream.Write(Encoding.UTF8.GetBytes(codename));
			fileStream.Write(BitConverter.GetBytes(codeversion));
			fileStream.Write(BitConverter.GetBytes(12 + bytes.Count()));
			fileStream.Write(bytes.ToArray(), 0, bytes.Count);
			fileStream.Write(tree.ToArray(), 0, tree.Count);
			fileStream.SetLength(12 + bytes.Count + tree.Count);
			fileStream.Close();
			this.path = path;
			stream = File.Open(path, FileMode.OpenOrCreate);
		}
		private void LoadContents(string prefix = "", int? directoryLength = null)
		{
			if (stream != null)
			{
				long max = directoryLength != null ? stream.Position + directoryLength.Value : stream.Length;
				while (stream.Position < max)
				{
					byte[] buffer = new byte[4];
					string name = GetString();
					if (name.EndsWith('\\'))
					{
						stream.Read(buffer, 0, buffer.Length);
						LoadContents(name, BitConverter.ToInt32(buffer));
					}
					else
					{
						stream.Read(buffer, 0, buffer.Length);
						int dataOffset = BitConverter.ToInt32(buffer);
						stream.Read(buffer, 0, buffer.Length);
						int dataLength = BitConverter.ToInt32(buffer);
						contents.Add(prefix + name, new(stream, dataOffset, dataLength));
					}
				}
			}
			throw new("Stream is null");
		}
		private string GetString()
		{
			if (stream != null)
			{
				List<byte> buffer = new List<byte>();
				while (true)
				{
					int b = stream.ReadByte();
					if (b <= 0 || b == 0x5C)
					{
						return Encoding.UTF8.GetString(buffer.ToArray(), 0, buffer.Count);
					}
					buffer.Add((byte)b);
				}
			}
			throw new("Stream is null");
		}
	}
	public class Content
	{
		private Stream stream;
		private int offset;
		private int length;
		public Content(Stream stream, int offset, int length)
		{
			this.stream = stream;
			this.offset = offset;
			this.length = length;
		}
		public byte[] data
		{
			get
			{
				stream.Position = offset;
				byte[] buffer = new byte[length];
				stream.Read(buffer, 0, length);
				return buffer;
			}
		}
	}
}