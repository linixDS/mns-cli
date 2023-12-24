using Core;
using FortiOS.Types;
using FortiOS.Client;
using System.Text.Json;
using System;
using System.Xml.Linq;

namespace fortigate
{
    public class HELPCLASS
    {
        public HELPCLASS()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\tFortiGate");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\tFortiGate control device.");
            Console.WriteLine("");
            Console.WriteLine("MODULES");
            Console.WriteLine("\t fortigate help");
            Console.WriteLine("\t fortigate dhcp");
            Console.WriteLine("\t fortigate devices");
            Console.WriteLine("\t fortigate vpn");
            Console.WriteLine("\t fortigate profile");
            Console.WriteLine("\t fortigate log");
            Console.WriteLine("");
        }
    }

    public class CONFIGCLASS
    {
        public int Port { get; set; }
        public string Address { get; set; }
        public string Token { get; set; }

        public CONFIGCLASS() 
        { 
            this.Port= 0;
            this.Address = string.Empty;
            this.Token = string.Empty;
        }

    }

    public class PROFILECLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t Profile configuration device");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t fortigate profile help");
            Console.WriteLine("\t fortigate profile list");
            Console.WriteLine("\t fortigate profile remove <input: name>");
            Console.WriteLine("\t fortigate profile add <input: name>");
            Console.WriteLine("");
        }

        public void list()
        {
            Terminal.WriteText("::List all profiles in fortigate", ConsoleColor.Green, Console.BackgroundColor);
            string[] files =  Directory.GetFiles(Directory.GetCurrentDirectory());

            foreach (string file in files) 
            {
                string ext = Path.GetExtension(file);
                if (ext.Contains(".profile"))
                {
                    Console.Write("Profile name: ");
                    Terminal.WriteText(Path.GetFileNameWithoutExtension(file), ConsoleColor.Yellow, Console.BackgroundColor);
                }
            }
        }

        public void remove(string name)
        {
            var fileName = name + ".profile";
            if (!File.Exists(fileName))
            {
                Terminal.ErrorWrite("Error: No found profile: " + name);
                return;
            }

            try
            {
                File.Delete(fileName);
                Terminal.WriteText("Deleted profile.", ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
        }

        public void add(string name)
        {
            var config = new CONFIGCLASS();
            var fileName = name + ".profile";
            if (File.Exists(fileName))
            {
                Terminal.ErrorWrite("Error: Profile already exists: " + name) ;
                return;
            }

            try
            {
                Console.Write("ADDRESS> ");
                config.Address = Console.ReadLine();
                Console.Write("PORT> ");
                string value = Console.ReadLine();
                config.Port = Int32.Parse(value);
                Console.Write("TOKEN> ");
                config.Token = Console.ReadLine();
                if (config.Token.Length > 0)
                    config.Token = Core.Crypto.Encrypt(config.Token, "hoff01HOFF02");



                var json = JsonSerializer.Serialize(config);
                
                StreamWriter writer = new StreamWriter(fileName);
                writer.WriteLine(json);
                writer.Close();
                Terminal.WriteText("Saved new profile.", ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }


        public CONFIGCLASS load(string name)
        {
            try
            {
                var fileName = name + ".profile";
                if (!File.Exists(fileName))  return null;

                string json = File.ReadAllText(fileName);
                var config = JsonSerializer.Deserialize<CONFIGCLASS>(json);
                if (config.Token.Length > 1)
                    config.Token = Core.Crypto.Decrypt(config.Token, "hoff01HOFF02");

                return config;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
                return null;
            }
        }
    }

    public class DHCPCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t FortiGate.DHCP");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t fortigate dhcp help");
            Console.WriteLine("\t fortigate dhcp leases <input: profile>");
            Console.WriteLine("\t fortigate dhcp leases-filter <input1: value>  <input2: profile>");
            Console.WriteLine("\t fortigate dhcp servers <input1: profile>");
            Console.WriteLine("\t fortigate dhcp server-detail <input1: interfaceName> <input2: profile>");
            Console.WriteLine("\t fortigate dhcp servers-filter <input1: value> <input2: profile>");
            Console.WriteLine("");
        }

        private List<FortiDHCPLease> GetData(string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: "+profileName);
                    return null;
                }

                var client = new FortiClient(config.Address, config.Port, config.Token);
                List<FortiDHCPLease> result = client.GetDHCPLeasesFromNetwork();
                if (result == null)
                {
                    Terminal.ErrorWrite(client.GetLastError());
                    return null;
                }

                return result;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
                return null;
            }
        }

        private List<FortiDHCPServer> GetDataServers(string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: "+profileName);
                    return null;
                }

                var client = new FortiClient(config.Address, config.Port, config.Token);
                List<FortiDHCPServer> result = client.GetDHCPServers();
                if (result == null)
                {
                    Terminal.ErrorWrite(client.GetLastError());
                    return null;
                }

                return result;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
                return null;
            }
        }

        private FortiDHCPServer GetDataServerDetail(string interfaceName, string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: "+profileName);
                    return null;
                }

                var client = new FortiClient(config.Address, config.Port, config.Token);
                List<FortiDHCPServer> result = client.GetDHCPServers();
                if (result == null)
                {
                    Terminal.ErrorWrite(client.GetLastError());
                    return null;
                }

                foreach (var item in result) 
                {
                    if (item.InterfaceeName == interfaceName)
                    {
                        return item;
                    }
                }

                Terminal.ErrorWrite("Interface "+interfaceName+" no found !");
                return null;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
                return null;
            }
        }

        public void leases(string profileName)
        {
            Terminal.WriteText("::List DHCP leases", ConsoleColor.Green, Console.BackgroundColor);
            Console.WriteLine();
            try
            {
                List<FortiDHCPLease> result = GetData(profileName);
                if (result == null) return;
   
         
                string header1 = String.Format("{0,-2} {1,-18} {2,-18} {3,-20} {4}"," ","Address IP","Address MAC","Interface","Host Name");
                string header2 = String.Format("{0,-2} {1,-18} {2,-18} {3,-20} {4}"," ","---------------","-----------------","----------------","-----------------");
                                                                 
                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;

                foreach (var dhcp in result)
                {
                    if (dhcp.Reserved)
                        R = "R";
                    else
                        R = String.Empty;

                     Console.WriteLine("{0,-2} {1,-18} {2,-18} {3,-20} {4}",R,dhcp.IP, dhcp.MAC, dhcp.Interface, dhcp.HostName);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }

        public void leases_filter(string value, string profileName)
        {
            try
            {
                Terminal.WriteText("::List DHCP leases", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine();
                List<FortiDHCPLease> result = GetData(profileName);
                if (result == null) return;


                string header1 = String.Format("{0,-2} {1,-18} {2,-18} {3,-20} {4}", " ", "Address IP", "Address MAC", "Interface", "Host Name");
                string header2 = String.Format("{0,-2} {1,-18} {2,-18} {3,-20} {4}", " ", "---------------", "-----------------", "----------------", "-----------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;

                foreach (var dhcp in result)
                {
                    if ((dhcp.IP.Contains(value)) ||
                        (dhcp.MAC.Contains(value)) ||
                        (dhcp.Interface.Contains(value)) ||
                        (dhcp.HostName.Contains(value)))
                        {
                            if (dhcp.Reserved)
                                R = "R";
                            else
                                R = String.Empty;

                            Console.WriteLine("{0,-2} {1,-18} {2,-18} {3,-20} {4}", R, dhcp.IP, dhcp.MAC, dhcp.Interface, dhcp.HostName);
                        }
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }


        public void servers(string profileName)
        {
            try
            {
                Terminal.WriteText("::List DHCP Servers", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine();
                List<FortiDHCPServer> result = GetDataServers(profileName);
                if (result == null) return;

                string header1 = String.Format("{0,-2} {1,-18} {2,-16} {3,-34} {4,-16} {5,-25} {6}",        " ", "Interface", "Gateway IP", "IP Range", "NetMask","DNS Server","Domain");
                string header2 = String.Format("{0,-2} {1,-18} {2,-16} {3,-34} {4,-16} {5,-25} {6}", " ", "---------------","---------------", "------------------------------", "-------------", "-----------------","--------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;
                string dns;
                foreach (var dhcp in result)
                {
                    if (dhcp.Status.Contains("enable"))  R = "A";

                    if (dhcp.DNSService == "local")
                        dns = dhcp.Gateway;
                    else
                    {
                        dns = dhcp.DNSServer1 + " " + dhcp.DNSServer2 + " " + dhcp.DNSServer3;
                        dns = dns.Replace("0.0.0.0", "");
                    }
                    string ip_range = String.Format("{0}-{1}", dhcp.IPRange[0].StartIP, dhcp.IPRange[0].EndIP);

                    Console.WriteLine("{0,-2} {1,-18} {2,-16} {3,-34} {4,-16} {5,-25} {6}", R, dhcp.InterfaceeName, dhcp.Gateway, ip_range, dhcp.Netmask, dns, dhcp.Domain);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }

        public void server_detail(string interfaceName, string profileName)
        {
            try
            {
                Terminal.WriteText("::Detail DHCP Server: "+interfaceName, ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine();
                FortiDHCPServer result = GetDataServerDetail(interfaceName,profileName);
                if (result == null) return;

                string dns;

                if (result.DNSService == "local")
                    dns = result.Gateway;
                else
                {
                    dns = result.DNSServer1 + " " + result.DNSServer2 + " " + result.DNSServer3;
                    dns = dns.Replace("0.0.0.0", "");
                }

                var ntp = result.NTPServer1 + " " + result.NTPServer2 + " " + result.NTPServer3;
                ntp = ntp.Replace("0.0.0.0", "");
                var ip_range = String.Format("{0}-{1}", result.IPRange[0].StartIP, result.IPRange[0].EndIP);



                Console.Write("INTERFACE : ");
                Terminal.WriteText(interfaceName, ConsoleColor.Cyan, Console.BackgroundColor);
                Console.Write("GATEWAY IP: ");
                Terminal.WriteText(result.Gateway, ConsoleColor.Cyan, Console.BackgroundColor);
                Console.Write("IP RANGE  : ");
                Terminal.WriteText(ip_range, ConsoleColor.Cyan, Console.BackgroundColor);
                Console.Write("NETMASK   : ");
                Terminal.WriteText(result.Netmask, ConsoleColor.Cyan, Console.BackgroundColor);
                Console.Write("DNS       : ");
                Terminal.WriteText(dns, ConsoleColor.Cyan, Console.BackgroundColor);
                Console.Write("NTP       : ");
                Terminal.WriteText(ntp, ConsoleColor.Cyan, Console.BackgroundColor);
                Console.Write("DOMAIN    : ");
                Terminal.WriteText(result.Domain, ConsoleColor.Cyan, Console.BackgroundColor);

                Console.WriteLine();

                string header1 = String.Format("{0,-18} {1,-18} {2}", "Address IP", "Address MAC", "Comment");
                string header2 = String.Format("{0,-18} {1,-18} {2}", "---------------", "---------------", "------------------------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

     
                foreach (var dhcp in result.ReservedAddress)
                {
     
                    Console.WriteLine("{0,-18} {1,-18} {2}", dhcp.AddressIP, dhcp.AddressMAC, dhcp.Comment);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }

        public void servers_filter(string value, string profileName)
        {
            try
            {
                Terminal.WriteText("::Filter DHCP Servers", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine();
                List<FortiDHCPServer> result = GetDataServers(profileName);
                if (result == null) return;

                string header1 = String.Format("{0,-2} {1,-18} {2,-16} {3,-34} {4,-16} {5,-25} {6}", " ", "Interface", "Gateway IP", "IP Range", "NetMask", "DNS Server", "Domain");
                string header2 = String.Format("{0,-2} {1,-18} {2,-16} {3,-34} {4,-16} {5,-25} {6}", " ", "---------------", "---------------", "------------------------------", "-------------", "-----------------", "--------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;
                string dns;
                foreach (var dhcp in result)
                {
                    if ((dhcp.Status.Contains(value)) ||
                        (dhcp.Gateway.Contains(value)) ||
                        (dhcp.InterfaceeName.Contains(value)) ||
                        (dhcp.Domain.Contains(value)) ||
                        (dhcp.DNSServer1.Contains(value)) ||
                        (dhcp.DNSServer2.Contains(value)) ||
                        (dhcp.DNSServer3.Contains(value)))
                    {
                        if (dhcp.Status.Contains("enable")) R = "A";

                        if (dhcp.DNSService == "local")
                            dns = dhcp.Gateway;
                        else
                        {
                            dns = dhcp.DNSServer1 + " " + dhcp.DNSServer2 + " " + dhcp.DNSServer3;
                            dns = dns.Replace("0.0.0.0", "");
                        }
                        string ip_range = String.Format("{0}-{1}", dhcp.IPRange[0].StartIP, dhcp.IPRange[0].EndIP);

                        Console.WriteLine("{0,-2} {1,-18} {2,-16} {3,-34} {4,-16} {5,-25} {6}", R, dhcp.InterfaceeName, dhcp.Gateway, ip_range, dhcp.Netmask, dns, dhcp.Domain);
                    }
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }
    }

    public class DEVICESCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t FortiGate.DEVICES");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t fortigate devices help");
            Console.WriteLine("\t fortigate devices show <input: profile>");
            Console.WriteLine("\t fortigate devices search <input1: value> <input2: profile>");
            Console.WriteLine("");
        }

        private List<FortiRegisterDevice> GetData(string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile.");
                    return null;
                }

                var client = new FortiClient(config.Address, config.Port, config.Token);
                List<FortiRegisterDevice> result = client.GetAllDevicesFromNetwork();
                if (result == null)
                {
                    Terminal.ErrorWrite(client.GetLastError());
                    return null;
                }

                return result;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
                return null;
            }
        }

        public List<string> GetAddressIP(string value, string profileName)
        {
            List<string> result = new List<string>();
            try
            {
                List<FortiRegisterDevice> results = GetData(profileName);
                if (results == null)
                {
                  Environment.Exit(-1);
                  return result;
                }

                foreach (var data in results)
                {
                    if (data.Hostname.Contains(value))
                    {
                        result.Add(data.IP);
                    }
                }
                return result;
            }
            catch (Exception error)
            {
                Environment.Exit(-1);
            }
            return result;
        }

        public void search(string value, string profileName)
        {
            try
            {
                Terminal.WriteText("::List all registers devices", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine();

                List<FortiRegisterDevice> result = GetData(profileName);
                if (result == null) return;


                string header1 = String.Format("{0,-2} {1,-18} {2,-18} {3,-20} {4,-25} {5,-25}", " ", "Address IP", "Address MAC", "Interface", "Host Name", "Vendor");
                string header2 = String.Format("{0,-2} {1,-18} {2,-18} {3,-20} {4,-25} {5,-25}", " ", "--------------", "-----------------", "--------------", "----------------", "----------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;

                foreach (var data in result)
                {
                    if ( (data.IP.Contains(value)) ||
                         (data.MAC.Contains(value)) ||
                         (data.Interface.Contains(value)) ||
                         (data.Hostname.Contains(value)) ||
                         (data.HardwareVendor.Contains(value)) )
                    {
                        if (data.IsOnline)
                            R = "*";
                        else
                            R = String.Empty;

                        Console.WriteLine("{0,-2} {1,-18} {2,-18} {3,-20} {4,-25} {5,-25}", R, data.IP, data.MAC, data.Interface, data.Hostname, data.HardwareVendor);
                    }
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }

        public void show(string profileName)
        {
            try
            {
                Terminal.WriteText("::List all registers devices", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine();

                List<FortiRegisterDevice> result = GetData(profileName);
                if (result == null) return;


                string header1 = String.Format("{0,-2} {1,-16} {2,-17} {3,-20} {4,-25} {5,-25}", " ", "Address IP", "Address MAC", "Interface", "Host Name","Vendor");
                string header2 = String.Format("{0,-2} {1,-16} {2,-17} {3,-20} {4,-25} {5,-25}", " ", "--------------", "-----------------", "--------------", "----------------","----------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;

                foreach (var data in result)
                {
                    if (data.IsOnline)
                        R = "*";
                    else
                        R = String.Empty;
                    
                    Console.WriteLine("{0,-2} {1,-16} {2,-17} {3,-20} {4,-25} {5,-25}", R, data.IP, data.MAC, data.Interface, data.Hostname, data.HardwareVendor);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }
    }




    public class VPNCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t FortiGate.VPN");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t fortigate vpn help");
            Console.WriteLine("\t fortigate vpn users <input: profile>");
            Console.WriteLine("\t fortigate vpn users-filter <input1: value> <input2: profile>");
            Console.WriteLine("");
        }

        private List<FortiVPNUser> GetData(string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile.");
                    return null;
                }

                var client = new FortiClient(config.Address, config.Port, config.Token);
                List<FortiVPNUser> result = client.GetAllUsersFromVPN();
                if (result == null)
                {
                    Terminal.ErrorWrite(client.GetLastError());
                    return null;
                }

                return result;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
                return null;
            }
        }

        public void users_filter(string value, string profileName)
        {
            try
            {
                Terminal.WriteText("::Filter VPN users", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine();

                List<FortiVPNUser> result = GetData(profileName);
                if (result == null) return;


                string header1 = String.Format("{0,-2} {1,-30} {2,-30} {3,-20} {4,-20}", " ", "Name", "E-mal", "TwoFactor","Date");
                string header2 = String.Format("{0,-2} {1,-30} {2,-30} {3,-20} {4,-20}", " ", "----------------------", "----------------------------", "-----------", "-------------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;

                foreach (var data in result)
                {
                    if ((data.Email.Contains(value)) ||
                         (data.Name.Contains(value)) ||
                         (data.TwoFactor.Contains(value)) )
                    {
                        if (data.Status.Contains("enable"))
                            R = String.Empty;
                        else
                            R = "D";

                        Console.WriteLine("{0,-2} {1,-30} {2,-30} {3,-20} {4,-20}", R, data.Name, data.Email, data.TwoFactor, data.PasswordTime);
                    }
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }

        public void users(string profileName)
        {
            try
            {
                Terminal.WriteText("::List all VPN users", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine();

                List<FortiVPNUser> result = GetData(profileName);
                if (result == null) return;

                string header1 = String.Format("{0,-2} {1,-30} {2,-30} {3,-20} {4,-20}", " ", "Name", "E-mal", "TwoFactor", "Date");
                string header2 = String.Format("{0,-2} {1,-30} {2,-30} {3,-20} {4,-20}", " ", "----------------------", "----------------------------", "-----------", "-------------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;

                foreach (var data in result)
                {
                        if (data.Status.Contains("enable"))
                            R = String.Empty;
                        else
                            R = "D";

                    Console.WriteLine("{0,-2} {1,-30} {2,-30} {3,-20} {4,-20}", R, data.Name, data.Email, data.TwoFactor, data.PasswordTime);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }
    }



   public class LOGCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t FortiGate.LOG");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t fortigate log help");
            Console.WriteLine("\t fortigate log show <input: profile>");
            Console.WriteLine("\t fortigate log save-json <input: profile>");
            Console.WriteLine("\t fortigate log accept <input: profile>");
            Console.WriteLine("\t fortigate log deny <input: profile>");
            Console.WriteLine("\t fortigate log srcip <input1: value> <input2: profile>");
            Console.WriteLine("\t fortigate log dstip <input1: value> <input2: profile>");
            Console.WriteLine("\t fortigate log service <input1: value> <input2: profile>");
            Console.WriteLine("\t fortigate log country <input1: value> <input2: profile>");
            Console.WriteLine("\t fortigate log intf <input1: value> <input2: profile>");
            Console.WriteLine("\t fortigate log policy <input1: value> <input2: profile>");
            Console.WriteLine("\t fortigate log filter <input1: field> <input2: value> <input3: profile>");
            Console.WriteLine("");
        }

        private List<FortiLogTraffic> GetData(string profileName, string filter = "")
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile.");
                    return null;
                }

                var client = new FortiClient(config.Address, config.Port, config.Token);
                List<FortiLogTraffic> result = client.GetLogTraffic(filter);
                if (result == null)
                {
                    Terminal.ErrorWrite(client.GetLastError());
                    return null;
                }

                return result;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
                return null;
            }
        }

        private String GetJson(string profileName, string filter = "")
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile.");
                    return null;
                }

                var client = new FortiClient(config.Address, config.Port, config.Token);
                String result = client.GetLogTrafficFormatJson(filter);
                if (result == null)
                {
                    Terminal.ErrorWrite(client.GetLastError());
                    return null;
                }

                return result;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
                return null;
            }
        }        

        private void header()
        {
            string header1 = String.Format("{0,-12} {1,-9} {2,-18} {3,-18} {4,-12} {5,-20} {6,-20} {7,-35} {8,-25}",   "Date",        "Time",     "Src IP",          "Dest IP",      "Service",     "Input",            "Output",           "Policy",       "Country");
            string header2 = String.Format("{0,-12} {1,-9} {2,-18} {3,-18} {4,-12} {5,-20} {6,-20} {7,-35} {8,-25}",   "----------", "--------", "--------------", "--------------", "---------", "----------------", "----------------", "----------------", "----------------");

            Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.WriteLine(header2);
        }

        private void print(FortiLogTraffic data)
        {
            var Country = String.Empty;

            if (data.srcintf.Contains("wan"))
                Country = data.srccountry;
            else
                Country = data.dstcountry;

            if (data.action == "accept")
                Console.ForegroundColor = ConsoleColor.Green;
            else
                if (data.action == "deny")
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("{0,-12} {1,-9} {2,-18} {3,-18} {4,-12} {5,-20} {6,-20} {7,-35} {8,-25}", data.date, data.time, data.srcip, data.dstip, data.service, data.srcintf.ToUpper(), data.dstintf.ToUpper(), data.policyname, Country);
        }

        public void srcip(string value, string profileName)
        {
            try
            {
                Terminal.WriteText("::Log traffic - srcip", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine("Source IP: "+value);

                String filter = "&filter=srcip=@"+value;

                List<FortiLogTraffic> result = GetData(profileName, filter);
                if (result == null) return;


                this.header();

                foreach (var data in result)
                {
                    this.print(data);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }

        public void dstip(string value, string profileName)
        {
            try
            {
                Terminal.WriteText("::Log traffic - srcip", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine("Dest IP: "+value);

                String filter = "&filter=dstip=@"+value;

                List<FortiLogTraffic> result = GetData(profileName, filter);
                if (result == null) return;


                this.header();

                foreach (var data in result)
                {
                    this.print(data);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }

        public void service(string value, string profileName)
        {
            try
            {
                Terminal.WriteText("::Log traffic - Service", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine("Service name: "+value);

                String filter = "&filter=service=@"+value;

                List<FortiLogTraffic> result = GetData(profileName, filter);
                if (result == null) return;


                this.header();

                foreach (var data in result)
                {
                    this.print(data);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }        

        public void country(string value, string profileName)
        {
            try
            {
                Terminal.WriteText("::Log traffic - srcip", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine("Filter country: "+value);

                List<FortiLogTraffic> result = GetData(profileName);
                if (result == null) return;


                this.header();

                foreach (var data in result)
                {
                    if ((data.srccountry.Contains(value)) || 
                        (data.dstcountry.Contains(value)))
                    {
                        this.print(data);
                    }
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }

        public void intf(string value, string profileName)
        {
            try
            {
                Terminal.WriteText("::Log traffic - srcip", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine("Filter interface: "+value);

                List<FortiLogTraffic> result = GetData(profileName);
                if (result == null) return;


                this.header();

                foreach (var data in result)
                {
                    if ((data.srcintf.Contains(value)) || 
                        (data.dstintf.Contains(value)))
                    {
                        this.print(data);
                    }
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        } 

        public void policy(string value, string profileName)
        {
            try
            {
                Terminal.WriteText("::Log traffic - srcip", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine("Filter policy name: "+value);

                List<FortiLogTraffic> result = GetData(profileName);
                if (result == null) return;


                this.header();

                foreach (var data in result)
                {
                    if (data.policyname.Contains(value)) 
                    {
                        this.print(data);
                    }
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }

      public void filter(string field, string value, string profileName)
        {
            try
            {
                Terminal.WriteText("::Log traffic - srcip", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine("Filter query: "+field+"@"+value);

                String _filter = "&filter="+field+"=@"+value;

                List<FortiLogTraffic> result = GetData(profileName, _filter);
                if (result == null) return;


                this.header();

                foreach (var data in result)
                {
                    this.print(data);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }                                   


        public void accept(string profileName)
        {
            try
            {
                Terminal.WriteText("::Log traffic - Packet Accept", ConsoleColor.Green, Console.BackgroundColor);

                String filter = "&filter=action=@accept";

                List<FortiLogTraffic> result = GetData(profileName, filter);

                if (result == null) return;


                this.header();

                foreach (var data in result)
                {
                    if (data.action == "accept")
                    {
                        this.print(data);
                    }
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }

        public void deny(string profileName)
        {
            try
            {
                Terminal.WriteText("::Log traffic - Packet Deny", ConsoleColor.Green, Console.BackgroundColor);

                String filter = "&filter=action=@deny";

                List<FortiLogTraffic> result = GetData(profileName, filter);
                if (result == null) return;


                this.header();

                foreach (var data in result)
                {
                    if (data.action == "deny")
                    {
                        this.print(data);
                    }
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        } 
        


        public void save_json(string profileName)
        {
            try
            {
                Terminal.WriteText("::Log traffic - save json", ConsoleColor.Green, Console.BackgroundColor);

                String result = GetJson(profileName, "");
                if (result == null) return;

                string name = "fortigate-query-"+DateTime.Now.ToString()+".json";
                File.WriteAllText(name, result);
                Terminal.WriteText("Saved result to file: "+name, ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }
             

        public void show(string profileName)
        {
            try
            {
                Terminal.WriteText("::Log traffic", ConsoleColor.Green, Console.BackgroundColor);

                List<FortiLogTraffic> result = GetData(profileName);
                if (result == null) return;

                this.header();
                var Country = String.Empty;

                foreach (var data in result)
                {
                    this.print(data);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }
            Console.WriteLine();
        }
    }    
}