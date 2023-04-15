using TTMC.WAD3;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] file = File.ReadAllBytes("D:\\SteamLibrary\\steamapps\\common\\Half-Life SDK\\Texture Wad Files\\halflife.wad");
            List<Texture> check = Core.LoadWAD(file);
            foreach (Texture value in check)
            {
                Console.WriteLine(value.compressed);
            }
        }
    }
}