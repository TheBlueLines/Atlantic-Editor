using TTMC.VPK;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (Directory.Exists("Dump"))
            {
                Directory.Delete("Dump", true);
            }
            Directory.CreateDirectory("Dump");
            VPK.LoadVPK("D:\\SteamLibrary\\steamapps\\common\\Portal 2\\portal2\\pak01_dir.vpk");
            //VPK.LoadVPK("D:\\SteamLibrary\\steamapps\\common\\Portal 2\\update\\pak01_dir.vpk");
            Console.WriteLine("VPK loaded!");
            foreach (Entry entry in VPK.entries)
            {
                Directory.CreateDirectory("Dump\\" + entry.path);
                File.WriteAllBytes("Dump\\" + entry.fullPath, entry.data);
            }
        }
    }
}