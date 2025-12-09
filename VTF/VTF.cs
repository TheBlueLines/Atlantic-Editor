using System.Text;

namespace TTMC.VTF
{
	public class VTF
	{
		public readonly string signature;
		public readonly uint versionMajor;
		public readonly uint versionMinor;
		public readonly uint headerSize;
		public readonly ushort width;
		public readonly ushort height;
		public readonly uint flags;
		public VTF(Stream stream)
		{
			byte[] rawData = new byte[4];
			stream.Read(rawData, 0, 4);
			signature = Encoding.UTF8.GetString(rawData);
			if (signature == "VTF\0")
			{
				stream.Read(rawData, 0, 4);
				versionMajor = BitConverter.ToUInt32(rawData);
				stream.Read(rawData, 0, 4);
				versionMinor = BitConverter.ToUInt32(rawData);
				stream.Read(rawData, 0, 4);
				headerSize = BitConverter.ToUInt32(rawData);
				stream.Read(rawData, 0, 2);
				width = BitConverter.ToUInt16(rawData);
				stream.Read(rawData, 0, 2);
				height = BitConverter.ToUInt16(rawData);
				stream.Read(rawData, 0, 4);
				flags = BitConverter.ToUInt32(rawData);
				return;
			}
			throw new("Unsupported file");
		}
	}
}