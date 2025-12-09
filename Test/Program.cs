using TTMC.BPList;

namespace Test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			FileStream fileStream = File.OpenRead("C:\\Users\\David\\Downloads\\Rule The Kingdom\\Payload\\Kingdom Unibuild.app\\Info.plist");
			BPList bpList = new BPList(fileStream);
		}
	}
}