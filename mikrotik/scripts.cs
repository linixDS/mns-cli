using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core;
using MikroTik.Types;
using MikroTik.Client;

namespace mikrotik
{
    public class SCRIPTCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t MikroTik Scripta");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t mikrotik script help");

            Console.WriteLine("\t mikrotik script list <input: profile>");

            Console.WriteLine("\t mikrotik script view <input1: id> <input2: profile>");
            Console.WriteLine("\t mikrotik script run <input1: id> <input2: profile>");
            Console.WriteLine("\t mikrotik script delete <input1: id> <input2: profile>");
        }

        public void list(string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    return;
                }

                Terminal.WriteText("::MikroTik Get List all scripts: " + config.Address, ConsoleColor.Green, Console.BackgroundColor);


                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetScripts();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();

                string header1 = String.Format(" {0,-6} {1,-25} {2,-30} {3,-12} {4,-25}",
                                                "ID",
                                                "Name",
                                                "Last started",
                                                "Count",
                                                "Policy");
                string header2 = String.Format(" {0,-6} {1,-25} {2,-30} {3,-12} {4,-25}",
                                                "----",
                                                "---------------",
                                                "---------------",
                                                "------",
                                                "-----------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                foreach (var info in result)
                {
                    if (info.Comment.Length > 0) Terminal.WriteText(" ;; " + info.Comment, ConsoleColor.Cyan, Console.BackgroundColor);
                    Console.WriteLine(" {0,-6} {1,-25} {2,-30} {3,-12} {4,-25}",
                                        info.Id,
                                        info.Name,
                                        info.LastStarted,
                                        info.RunCount,
                                        info.Policy);
                }

            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }
    }
}
