using System.Data;
using LibTerminal;

namespace nms_cli
{
    public class ParserCommandLine
    {
        public ParserCommandLine(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Network Managment Servces - Client Terminal v" + Application.Version);
                Console.WriteLine("Copyrigh(c) 2023 by Dariusz Marcisz");
                Console.WriteLine("");
                return;
            }


            if (args.Length < 2)
            {
                Terminal.ErrorWrite("Invalid number of arguments entered!");
                return;
            }


            string methodName = null;
            var moduleName = args[0];
            var className = args[1];
            if (args.Length > 2)
            {
                methodName = args[2];
                methodName =  methodName.Replace('-', '_');
            }

            var route = new RouteLib(moduleName, className, methodName);
            string[] inputs = null;

            if (args.Length > 3)
            {
                inputs = new string[args.Length - 3];
                for (int idx = 0; idx < inputs.Length; idx++)
                {
                    inputs[idx] = args[3 + idx];
                }
                route.SetParams(inputs);
            }

            route.Run();
        }


    }
}