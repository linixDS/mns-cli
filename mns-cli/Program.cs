using System;

namespace nms_cli
{
    public class Application
    {
        public static string Version = "0.1.0.0";
    }


    public class NetCli
    {
        public static void Main(string[] args)
        {
            var parser = new ParserCommandLine(args);
        }
    }
}