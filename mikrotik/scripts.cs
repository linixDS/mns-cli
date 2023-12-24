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
using CacheManagment;

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


        private List<mtScriptInfo> GetListScripts(string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    return null;
                }

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetScripts();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return null;
                }

                return result;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " +error.Message);
                return null;
            }
        }

        private mtScriptInfo Find(List<mtScriptInfo> list, string id)
        {
            var result = list.Find(x => x.Id == id);
            return result;
        }

        public void list(string profileName)
        {
            try
            {
                var result = GetListScripts(profileName);
                if (result == null) return;

                Terminal.WriteText("::MikroTik List scripts : ", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine();

                string header1 = String.Format(" {0,-6} {1,-45} {2,-30} {3,-12} {4,-25}",
                                                "ID",
                                                "Name",
                                                "Last started",
                                                "Count",
                                                "Policy");
                string header2 = String.Format(" {0,-6} {1,-45} {2,-30} {3,-12} {4,-25}",
                                                "----",
                                                "------------------------------------",
                                                "---------------------",
                                                "------",
                                                "------------------------------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                foreach (var info in result)
                {
                    if (info.Comment.Length > 0) Terminal.WriteText(" ;; " + info.Comment, ConsoleColor.Cyan, Console.BackgroundColor);
                    Console.WriteLine(" {0,-6} {1,-45} {2,-30} {3,-12} {4,-25}",
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

        public void view(string id, string profileName)
        {
            try
            {
                var result = GetListScripts(profileName);
                if (result == null) return;

                var data = Find(result, id);
                if (data == null)
                {
                    Terminal.ErrorWrite("No found script id:" + id);
                    return;
                }

                Terminal.WriteText("::MikroTik View script : " + id, ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine();

                var text = "ID";

                Console.Write("{0,-15}", text); Terminal.WarnWrite(data.Id);
                text = "NAME:";
                Console.Write("{0,-15}", text); Terminal.WarnWrite(data.Name);
                text = "LAST STARTED:";
                Console.Write("{0,-15}", text); Terminal.WarnWrite(data.LastStarted);
                text = "POLICY:";
                Console.Write("{0,-15}", text); Terminal.WarnWrite(data.Policy);
                text = "OWNER:";
                Console.Write("{0,-15}", text); Terminal.WarnWrite(data.Owner);
                text = "COMMENT:";
                Console.Write("{0,-15}", text); Terminal.WarnWrite(data.Comment);
                text = "RUN COUNT:";
                Console.Write("{0,-15}", text); Terminal.WarnWrite(data.RunCount);
                text = "-----[ SOURCE ]----------------------------------------------------------------------------------";
                Terminal.WriteText(text, ConsoleColor.Blue, Console.BackgroundColor);
                Terminal.WriteText(data.Source, ConsoleColor.Cyan, Console.BackgroundColor);
                text = "--------------------------------------------------------------------------------------------------";
                Terminal.WriteText(text, ConsoleColor.Blue, Console.BackgroundColor);

            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

        public void run(string id, string profileName)
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


                Terminal.WriteText("::MikroTik Execute script ID: " + id, ConsoleColor.Green, Console.BackgroundColor);
                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.ExecuteScript(id);
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }
                Terminal.WriteText("Done!", ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

        public void delete(string id, string profileName)
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


                Terminal.WriteText("::MikroTik Delete script ID: " + id, ConsoleColor.Green, Console.BackgroundColor);
                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.DeleteScript(id);
                if (result == false)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    Console.WriteLine();
                    return;
                }
                Terminal.WriteText("Deleted!", ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

    }
}
