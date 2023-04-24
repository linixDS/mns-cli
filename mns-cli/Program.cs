using System;
using Runtime;

namespace nms_cli
{
    public class NetCli
    {
        public static void Main(string[] args)
        {
            var parser = new ParserCommandLine(args);
        }
    }
}