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
using System.Runtime.InteropServices;

namespace mikrotik
{
    public class HELPCLASS
    {
        public HELPCLASS()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t MikroTik Managment ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t mikrotik help");
            Console.WriteLine("\t mikrotik profile");
            Console.WriteLine("\t mikrotik winbox");
            Console.WriteLine("\t mikrotik interface");
            Console.WriteLine("\t mikrotik ip");
            Console.WriteLine("\t mikrotik usermanger");
            Console.WriteLine("\t mikrotik radius");
            Console.WriteLine("\t mikrotik caps");
            Console.WriteLine("");
        }
    }

    public class WINBOXCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t MikroTik Run Tools ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t mikrotik winbox help");
            Console.WriteLine("\t mikrotik winbox run");
            Console.WriteLine("\t mikrotik winbox connect <input: profile>");
            Console.WriteLine("\t mikrotik winbox romon <input: profile>");
            Console.WriteLine("");
        }

        private void runWinBox(CONFIGCLASS profile, bool romon)
        {
            try
            {
                string args = String.Empty;
                if (profile != null)
                {
                    if (romon)
                        args = "--romon "+profile.Address+" ";

                    args += profile.Address + " " + profile.User + " " + profile.Password;
   
                    Terminal.WriteText("::WinBox connect to: " + profile.Address, ConsoleColor.Green, Console.BackgroundColor);
                }
                    else
                    Terminal.WriteText("::WinBox running", ConsoleColor.Green, Console.BackgroundColor);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Terminal.ExecuteProcess("winbox64.exe", args);
                }
                else
                {
                    args = "winbox64.exe " + args;
                    Terminal.ExecuteProcess("wine", args);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

        public void run()
        {
            runWinBox(null, false);
        }

        public void connect(string profileName)
        {
            var profile = new PROFILECLASS();
            var config = profile.load(profileName);
            if (config == null)
            {
                Terminal.ErrorWrite("No found profile: " + profileName);
                return;
            }

            runWinBox(config, false);
        }

        public void romon(string profileName)
        {
            var profile = new PROFILECLASS();
            var config = profile.load(profileName);
            if (config == null)
            {
                Terminal.ErrorWrite("No found profile: " + profileName);
                return;
            }

            runWinBox(config, true);
        }

    }

    public class INTERFACECLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t MikroTik Interface ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t mikrotik interface help");
            Console.WriteLine("\t mikrotik interface list <input: profile>");
            Console.WriteLine("\t mikrotik interface ethernet <input: profile>");
            Console.WriteLine("");
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
                Terminal.WriteText("::MikroTik all list interfaces from: "+ config.Address, ConsoleColor.Green, Console.BackgroundColor);

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetInterfaces();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();
                string header1 = String.Format("{0,-1} {1,-35} {2,-20} {3,-18} {4,-12} {5}", " ", "Name", "Address MAC", "Type", "Link down","Comment");
                string header2 = String.Format("{0,-1} {1,-35} {2,-20} {3,-18} {4,-12} {5}", " ", "------------------------", "-----------------", "---------------", "---------", "------------------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;
                foreach (var info in result)
                {
                    if (info.Disabled == "true") R = "D";
                    Console.WriteLine("{0,-1} {1,-35} {2,-20} {3,-18} {4,-12} {5}", R, info.Name, info.MAC, info.Type, info.LinkDown, info.Comment);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }


        public void ethernet(string profileName)
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
                Terminal.WriteText("::MikroTik all list ethernet from: " + config.Address, ConsoleColor.Green, Console.BackgroundColor);

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetEthernets();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();
                string header1 = String.Format("{0,-1} {1,-35} {2,-20} {3,-8} {4,-10} {5}", " ", "Name", "Address MAC", "MTU", "Speed", "Comment");
                string header2 = String.Format("{0,-1} {1,-35} {2,-20} {3,-8} {4,-10} {5}", " ", "------------------------", "-----------------", "------", "-------", "------------------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;
                foreach (var info in result)
                {
                    if (info.Disabled == "true") R = "D";
                    Console.WriteLine("{0,-1} {1,-35} {2,-20} {3,-8} {4,-10} {5}", R, info.Name, info.MAC, info.MTU, info.Speed, info.Comment);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }
    }


    public class IPCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t MikroTik IP ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t mikrotik ip help");
            Console.WriteLine("\t mikrotik ip neighbors <input: profile>");
            Console.WriteLine("\t mikrotik ip dhcp-lease <input: profile>");
            Console.WriteLine("");
        }

        public void neighbors(string profileName)
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
                Terminal.WriteText("::MikroTik Neighbors from agent: " + config.Address, ConsoleColor.Green, Console.BackgroundColor);

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetNeighbors();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();
                string header1 = String.Format("{0,-1} {1,-22} {2,-26} {3,-18} {4,-32} {5,-10} {6,-20} {7}", " ", "Identity", "Address IP", "Address MAC","Interface","Platform","Board","Description");
                string header2 = String.Format("{0,-1} {1,-22} {2,-26} {3,-18} {4,-32} {5,-10} {6,-20} {7}", " ", 
                                                            "--------------------", "--------------------------", "-----------------", "--------------------------", "---------", "---------------", "-----------------------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;
                foreach (var info in result)
                {
                  //  if (info.Disabled == "true") R = "D";
                    Console.WriteLine("{0,-1} {1,-22} {2,-26} {3,-18} {4,-32} {5,-10} {6,-20} {7}", R, info.Identity, info.AddressIP, info.MAC, info.InterfaceName, info.Platform,info.Board,info.SystemDescription);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

        public void dhcp_lease(string profileName)
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
                Terminal.WriteText("::MikroTik DHCP Server leases from: " + config.Address, ConsoleColor.Green, Console.BackgroundColor);


                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetDhcpServerLease();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();
                string header1 = String.Format("{0,-1} {1,-20} {2,-18} {3,-18} {4,-32} {5,-18} {6,-20}", 
                                                " ", 
                                                "Server", 
                                                "Address IP", 
                                                "Address MAC", 
                                                "Host name", 
                                                "Last seen", 
                                                "Expires");
                string header2 = String.Format("{0,-1} {1,-20} {2,-18} {3,-18} {4,-32} {5,-18} {6,-20}", 
                                                " ",
                                                "-----------------",
                                                "-----------------",
                                                "-----------------",
                                                "-----------------",
                                                "-----------------",
                                                "-----------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;
                foreach (var info in result)
                {
                    if (info.Disabled == "true") R = "D";
                    if (info.DynamicAddress == "true") R = "*";
                    Console.WriteLine("{0,-1} {1,-20} {2,-18} {3,-18} {4,-32} {5,-18} {6,-20}", R, info.DHCPServer, info.Address, info.MAC, info.HostName, info.LastSeen, info.ExpiresAfter);
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
