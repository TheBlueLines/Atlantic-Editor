using System.Text;

namespace TTMC.BPList
{
	public class BPList
	{
		public readonly string signature;
		public readonly string version;
		public readonly int offsetTableOffsetSize;
		public readonly int objectRefSize;
		public readonly ulong numObjects;
		public readonly ulong topObjectOffset;
		public readonly ulong offsetTableStart;
		public BPList(Stream stream)
		{
			byte[] rawData = new byte[6];
			stream.ReadExactly(rawData, 0, rawData.Length);
			signature = Encoding.ASCII.GetString(rawData);
			if (signature == "bplist")
			{
				rawData = new byte[2];
				stream.ReadExactly(rawData, 0, rawData.Length);
				version = Encoding.ASCII.GetString(rawData);
				stream.Position = stream.Length - 32;
				rawData = new byte[6];
				stream.ReadExactly(rawData, 0, rawData.Length);
				if (!rawData.SequenceEqual(new byte[6]))
				{
					throw new("Invalid BPList Footer");
				}
				offsetTableOffsetSize = stream.ReadByte();
				objectRefSize = stream.ReadByte();
				rawData = new byte[8];
				stream.ReadExactly(rawData, 0, rawData.Length);
				Array.Reverse(rawData);
				numObjects = BitConverter.ToUInt64(rawData);
				stream.ReadExactly(rawData, 0, rawData.Length);
				Array.Reverse(rawData);
				topObjectOffset = BitConverter.ToUInt64(rawData);
				stream.ReadExactly(rawData, 0, rawData.Length);
				Array.Reverse(rawData);
				offsetTableStart = BitConverter.ToUInt64(rawData);
				return;
			}
			throw new("Unsupported file");
		}
	}
}