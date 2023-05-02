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
    public class CAPSCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t MikroTik CAPsMAN");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t mikrotik caps help");

            Console.WriteLine("\t mikrotik caps config <input: profile>");

            Console.WriteLine("\t mikrotik caps wifi-clients <input: profile>");
            Console.WriteLine("\t mikrotik caps search-client <input1: value> <input2: profile>");

            Console.WriteLine("\t mikrotik caps interfaces <input: profile>");
            Console.WriteLine("\t mikrotik caps search-interfaces <input1: value> <input2: profile>");

            Console.WriteLine("\t mikrotik caps remote-cap <input: profile>");
            Console.WriteLine("\t mikrotik caps search-cap <input1: value> <input2: profile>");
        }



        private string SizeSuffix(Int64 value, int decimalPlaces = 0)
        {
            string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }

            int i = 0;
            decimal dValue = (decimal)value;

            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }

        private void DisplayRowWifiClient(mtCAPsMANRegistrationTableInfo info)
        {
            string[] bytes = info.Bytes.Split(',');

            Int64 txBytes = Int64.Parse(bytes[0]);
            Int64 rxBytes = Int64.Parse(bytes[1]);


            string tx = SizeSuffix(txBytes);
            string rx = SizeSuffix(rxBytes);
            string bytesStr = tx + "/" + rx;

            if (info.Comment.Length > 0)
                Terminal.WriteText(" ;; " + info.Comment, ConsoleColor.Cyan, Console.BackgroundColor);
            Console.WriteLine(" {0,-30} {1,-20} {2,-18} {3,-5} {4,-25} {5,-25} {6,-20} {7,-20}",
                                info.Interface,
                                info.SSID,
                                info.MAC,
                                info.RxSignal,
                                info.RxRate,
                                info.TxRate,
                                info.Uptime,
                                bytesStr);
        }

        private void GetRegistrionTable(string profileName, bool searching, string value)
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
                if (searching)
                    Terminal.WriteText("::MikroTik Searching WiFi-Client from CAPsMAN: " + value, ConsoleColor.Green, Console.BackgroundColor);
                else
                    Terminal.WriteText("::MikroTik CAPsMAN Registrion Table Clients: " + config.Address, ConsoleColor.Green, Console.BackgroundColor);


                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetRegistrionTableFromCAPsMAN();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();

                string header1 = String.Format(" {0,-30} {1,-20} {2,-18} {3,-5} {4,-25} {5,-25} {6,-20} {7,-20}",
                                                "Interface",
                                                "SSID",
                                                "MAC Address",
                                                "Rx",
                                                "Rx Rate",
                                                "Tx Rate",
                                                "Uptime",
                                                "Tx/Rx bytes");
                string header2 = String.Format(" {0,-30} {1,-20} {2,-18} {3,-5} {4,-25} {5,-25} {6,-20} {7,-20}",
                                                "--------------------",
                                                "---------------",
                                                "---------------",
                                                "-----",
                                                "---------------",
                                                "---------------",
                                                "---------------",
                                                "---------------");
                                                
                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                foreach (var info in result)
                {
                    if (searching)
                    {
                        if (info.Interface.ToUpper().Contains(value.ToUpper()) ||
                             info.SSID.ToUpper().Contains(value.ToUpper()) ||
                             info.Comment.ToUpper().Contains(value.ToUpper()) ||
                             info.MAC.ToUpper().Contains(value.ToUpper()))
                        {
                            DisplayRowWifiClient(info);
                        }

                    }
                        else{
                        DisplayRowWifiClient(info);
                    }

                }

            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }


        public void wifi_clients(string profileName)
        {
            GetRegistrionTable(profileName, false, String.Empty);
        }

        public void search_client(string value, string profileName)
        {
            GetRegistrionTable(profileName, true, value);
        }

        private void DisplayRowInterface(mtCAPsMANInterfaceInfo info)
        {
            if (info.Comment.Length > 0)
                Terminal.WriteText(" ;; " + info.Comment, ConsoleColor.Cyan, Console.BackgroundColor);
            var R = "";
            if (info.Disabled == "true") R = "D";

            Console.WriteLine("{0,-1} {1,-30} {2,-15} {3,-18} {4,-20} {5,-10} {6,-20} {7,-20}",
                                R,
                                info.Name,
                                info.CurrentState,
                                info.RadioMAC,
                                info.CurrentChannel,
                                info.CurrentAuthClients,
                                info.Configuration,
                                info.MasterInterface);
        }


        private void GetInterfaces(string profileName, bool searching, string value)
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
                if (searching)
                    Terminal.WriteText("::MikroTik Searching interface from CAPsMAN: " + value, ConsoleColor.Green, Console.BackgroundColor);
                else
                    Terminal.WriteText("::MikroTik Get List interfaces from CAPsMAN: " + config.Address, ConsoleColor.Green, Console.BackgroundColor);


                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetInterfacesFromCAPsMAN();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();

                string header1 = String.Format("{0,-1} {1,-30} {2,-15} {3,-18} {4,-20} {5,-10} {6,-20} {7,-20}",
                                                " ",
                                                "Name",
                                                "State",
                                                "RadioMAC",
                                                "Channel",
                                                "Clients",
                                                "Config",
                                                "Master");
                string header2 = String.Format("{0,-1} {1,-30} {2,-15} {3,-18} {4,-20} {5,-10} {6,-20} {7,-20}",
                                                " ",
                                                "--------------------",
                                                "----------",
                                                "---------------",
                                                "------------------",
                                                "-------",
                                                "------------------",
                                                "------------------");


                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                foreach (var info in result)
                {
                    if (searching)
                    {
                        if (info.Name.ToUpper().Contains(value.ToUpper()) ||
                            info.Comment.ToUpper().Contains(value.ToUpper()))
                        {
                            DisplayRowInterface(info);
                        }

                    }
                    else
                    {
                        DisplayRowInterface(info);
                    }

                }

            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

        public void interfaces(string profileName)
        {
            GetInterfaces(profileName, false, String.Empty);
        }

        public void search_interface(string value, string profileName)
        {
            GetInterfaces(profileName, true, value);
        }


        private void DisplayRowCAPRemote(mtCAPsMANRemoteCAPInfo info)
        {
            int idx = info.Address.IndexOf('/');
            string Address = info.Address.Substring(0,idx);


            Console.WriteLine(" {0,-8} {1,-25} {2,-18} {3,-18} {4,-20} {5,-18} {6,-10} {7,-6}",
                                info.State,
                                info.Identity,
                                Address,
                                info.MAC,
                                info.Board,
                                info.Serial,
                                info.Version,
                                info.Radios);
        }

        private void GetCAPRemote(string profileName, bool searching, string value)
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
                if (searching)
                    Terminal.WriteText("::MikroTik Searching remote-cap from CAPsMAN: " + value, ConsoleColor.Green, Console.BackgroundColor);
                else
                    Terminal.WriteText("::MikroTik Get List Remote-CAP from CAPsMAN: " + config.Address, ConsoleColor.Green, Console.BackgroundColor);


                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetRemoteCAPFromCAPsMAN();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();




                string header1 = String.Format(" {0,-8} {1,-25} {2,-18} {3,-18} {4,-20} {5,-18} {6,-10} {7,-6}",
                                                "State",
                                                "Identity",
                                                "Address",
                                                "MAC",
                                                "Board",
                                                "Serial",
                                                "Version",
                                                "Radios");
                string header2 = String.Format(" {0,-8} {1,-25} {2,-18} {3,-18} {4,-20} {5,-18} {6,-10} {7,-6}",
                                                "------",
                                                "--------------------",
                                                "---------------",
                                                "-----------------",
                                                "-----------------",
                                                "------------",
                                                "-------",
                                                "------");


                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                foreach (var info in result)
                {
                    if (searching)
                    {
                        if (info.Identity.ToUpper().Contains(value.ToUpper()) ||
                            info.Board.ToUpper().Contains(value.ToUpper()) ||
                            info.Address.ToUpper().Contains(value.ToUpper()) ||
                            info.Serial.ToUpper().Contains(value.ToUpper()) ||
                            info.Version.ToUpper().Contains(value.ToUpper()) ||
                            info.State.ToUpper().Contains(value.ToUpper()) )
                        {
                            DisplayRowCAPRemote(info);
                        }

                    }
                    else
                    {
                        DisplayRowCAPRemote(info);
                    }

                }

            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

        public void remote_cap(string profileName)
        {
            GetCAPRemote(profileName, false, String.Empty);
        }

        public void search_cap(string value, string profileName)
        {
            GetCAPRemote(profileName, true, value);
        }

        public void config(string profileName)
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

                Terminal.WriteText("::MikroTik Get List Configuration from CAPsMAN: " + config.Address, ConsoleColor.Green, Console.BackgroundColor);


                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetConfigurationFromCAPsMAN();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();




                string header1 = String.Format(" {0,-6} {1,-25} {2,-25} {3,-20} {4,-25} {5,-10} {6,-25} {7,-25}",
                                                "ID",
                                                "Name",
                                                "SSID",
                                                "Country",
                                                "Channel",
                                                "TxPower",
                                                "Security",
                                                "DataPath");
                string header2 = String.Format(" {0,-6} {1,-25} {2,-25} {3,-20} {4,-25} {5,-10} {6,-25} {7,-25}",
                                                "----",
                                                "---------------",
                                                "---------------",
                                                "-----------------",
                                                "-----------------",
                                                "-------",
                                                "-----------------",
                                                "-----------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                foreach (var info in result)
                {
                    if (info.Comment.Length > 0)Terminal.WriteText(" ;; " + info.Comment, ConsoleColor.Cyan, Console.BackgroundColor);
                    Console.WriteLine(" {0,-6} {1,-25} {2,-25} {3,-20} {4,-25} {5,-10} {6,-25} {7,-25}",
                                        info.Id,
                                        info.Name,
                                        info.SSID,
                                        info.Country,
                                        info.Channel,
                                        info.TxPower,
                                        info.Security,
                                        info.DataPath );
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
