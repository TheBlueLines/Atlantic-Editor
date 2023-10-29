using System.Text;

namespace TTMC.WPK
{
	public class WPK
	{
		internal byte[] data;
		public List<Content> contents = new();
		public WPK(byte[]? data = null)
		{
			if (data != null)
			{
				this.data = data;
				if (signature == "WPAK" && version == 1)
				{
					byte[] bytes = data[treeStart..];
					contents = DoWork(bytes);
				}
			}
			else
			{
				this.data = new byte[0];
			}
		}
		public byte[] MoreWork(string file)
		{
			return new byte[0];
		}
		public byte[] Compile()
		{
			List<byte> tree = new();
			List<byte> bytes = new();
			foreach (Content content in contents.OrderBy(x => x.name))
			{
				if (content.name.Contains('.'))
				{
					tree.AddRange(Encoding.UTF8.GetBytes(content.name));
					tree.Add(0x00);
					bytes.AddRange(content.data);
				}
				else
				{

				}
			}
			List<byte> response = new();
			response.AddRange(Encoding.UTF8.GetBytes("WPAK"));
			response.AddRange(BitConverter.GetBytes(1));
			response.AddRange(BitConverter.GetBytes(12 + 0));
			response.AddRange(bytes);
			response.AddRange(tree);
			return response.ToArray();
		}
		private List<Content> DoWork(byte[] data, string path = "")
		{
			List<Content> contents = new();
			int pointer = 0;
			while (data.Length > pointer)
			{
				string temp = path;
				string name = NullTerminated(data, pointer);
				pointer += name.Length + 1;
				temp = name + '\\';
				if (name.Contains('.'))
				{
					int dataOffset = BitConverter.ToInt32(data, pointer);
					int dataLength = BitConverter.ToInt32(data, pointer + 4);
					contents.Add(new(this, temp[..^1], dataOffset, dataLength));
					pointer += 8;
				}
				else
				{
					int directoryLength = BitConverter.ToInt32(data, pointer);
					pointer += 4;
					contents.AddRange(DoWork(data[pointer..][..directoryLength], temp));
					pointer += directoryLength;
				}
			}
			return contents;
		}
		private string signature
		{
			get
			{
				return Encoding.UTF8.GetString(data, 0, 4);
			}
		}
		private int version
		{
			get
			{
				return BitConverter.ToInt32(data, 4);
			}
		}
		private int treeStart
		{
			get
			{
				return BitConverter.ToInt32(data, 8);
			}
		}
		internal string NullTerminated(byte[] data, int start = 0, int max = 0)
		{
			List<byte> bytes = (max != 0 ? data[start..][..max] : data[start..]).ToList();
			int count = bytes.Contains(0x00) ? bytes.IndexOf(0x00) : max;
			return Encoding.ASCII.GetString(data, start, count);
		}
	}
	public class Content
	{
		private WPK wpk;
		public string name;
		public byte[]? value = null;
		private int offset;
		private int length;
		public Content(WPK wpk, string name, int offset, int length)
		{
			this.wpk = wpk;
			this.name = name;
			this.offset = offset;
			this.length = length;
		}
		public Content(WPK wpk, string name, byte[] data)
		{
			this.wpk = wpk;
			this.name = name;
			value = data;
		}
		public byte[] data
		{
			get
			{
				return value == null ? wpk.data[offset..][..length] : value;
			}
			set
			{
				this.value = value;
			}
		}
	}
}