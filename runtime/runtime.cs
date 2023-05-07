using Core;
using System.Reflection;

namespace Runtime
{

    public class ParserCommandLine
    {
        public ParserCommandLine(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {

                    Console.WriteLine("Network Managment Servces - Client Terminal v"+Utils.GetVersion());
                    Console.WriteLine("Copyrigh(c) 2023 by Dariusz Marcisz");
                    Console.WriteLine("");
                    return;
                }

                if (args.Length < 1)
                {
                    Terminal.ErrorWrite("Invalid number of arguments entered!");
                    return;
                }


                string methodName = null;
                string moduleName = null;
                string className = null;

                if (args[0] == "help")
                {
                    moduleName = "module";
                    className = "GLOBALHELP";
                }
                else
                    moduleName = args[0];





                if (args.Length > 1)
                    className = args[1];

                if (args.Length > 2)
                {
                    methodName = args[2];
                    methodName =  methodName.Replace('-', '_');
                }  


                var route = new Route(moduleName, className, methodName);

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
            catch (System.Exception error)
            {
                Terminal.ErrorWrite("Error (Parser): "+error.Message);
            }
                          
        }
    }

}
