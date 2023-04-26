using System;
using Runtime;

namespace nms_cli
{
    public class NetCli
    {
        public static void Main(string[] args)
        {
            var path = System.Reflection.Assembly.GetAssembly(typeof(NetCli)).Location;
            var workDir = Path.GetDirectoryName(path);
            Directory.SetCurrentDirectory(workDir);
            var parser = new ParserCommandLine(args);
        }
    }
}