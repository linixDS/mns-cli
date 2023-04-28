using Core;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace network
{
    public class HELPCLASS
    {
        public HELPCLASS()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\tNetwork");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\tNetwork tools and geo location ip address.");
            Console.WriteLine("");
            Console.WriteLine("MODULES");
            Console.WriteLine("\t network help");
            Console.WriteLine("\t network module");
            Console.WriteLine("\t network geoip");
            Console.WriteLine("\t network ip");
            Console.WriteLine("\t network tool");
            Console.WriteLine("\t network interface");
            Console.WriteLine("\t network wlan");
            Console.WriteLine("");
        }
    }

    public class GEOIPCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t network geoip ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\tGet information in address ip.");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t network geoip search <input: ip address>");
            Console.WriteLine("");
        }

        public void search(string value)
        {
            if (value == null)
            {
                Terminal.ErrorWrite("Error: Incorrect value!");
                return;
            }

            Terminal.WriteText("::Searching IP address " + value, ConsoleColor.Green, Console.BackgroundColor);
            var client = new IPClientApi();
            var result = client.SearchIP(value).Result;
            if (result == null)
            {
                Terminal.ErrorWrite(client.LastError);
                return;
            }

            if (result.Status.Contains("fail"))
            {
                Terminal.ErrorWrite("No found !");
                return;
            }

            Console.WriteLine("");
            Console.Write("Country: ");
            Terminal.WriteText(result.Country, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.Write("Region : ");
            Terminal.WriteText(result.RegionName, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.Write("City   : ");
            Terminal.WriteText(result.City, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.Write("ISP    : ");
            Terminal.WriteText(result.ISP, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.Write("AS     : ");
            Terminal.WriteText(result.AS, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.WriteLine("");
        }
    }


   public class TOOLCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t network tools ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t network tool ping <input: address>");
            Console.WriteLine("\t network tool sping <input: address>");
            Console.WriteLine("\t network tool tracert <input: address>");
            Console.WriteLine("");
        }


        public void ping(string value)
        {
            if (value == null)
            {
                Terminal.ErrorWrite("Error: Incorrect value!");
                return;
            }

            string strCmdText;
            strCmdText = value; ;

            Terminal.WriteText("::Send ping to host " + value, ConsoleColor.Green, Console.BackgroundColor);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Terminal.ExecuteProcess("ping.exe", strCmdText);
            }
            else
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Terminal.ExecuteProcess("ping", strCmdText);
            }
            else
                Terminal.ErrorWrite("Command not supported!");
        }

        public void tracert(string value)
        {
            if (value == null)
            {
                Terminal.ErrorWrite("Error: Incorrect value!");
                return;
            }

            string strCmdText;
            strCmdText =  value;

            Terminal.WriteText("::Tracert to host " + value, ConsoleColor.Green, Console.BackgroundColor);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Terminal.ExecuteProcess("tracert.exe", strCmdText);
            }
            else
            {
                Terminal.ExecuteProcess("traceroute", strCmdText);
            }
            Console.WriteLine();
        }

        public  void sping(string value)
        {
            if (value == null)
            {
                Terminal.ErrorWrite("Error: Incorrect value!");
                return;
            }

            Ping pingSender = new Ping();
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            int timeout = 5000;

            // Set options for transmission:
            // The data can go through 64 gateways or routers
            // before it is destroyed, and the data packet
            // cannot be fragmented.
            PingOptions options = new PingOptions(64, true);

            // Send the request.


            Terminal.WriteText("::Send ping to host " + value, ConsoleColor.Green, Console.BackgroundColor);
            PingReply reply = pingSender.Send(value, timeout, buffer, options);

            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine();
                Console.Write("Addres        : ");
                Terminal.WriteText(reply.Address.ToString(), ConsoleColor.Yellow, Console.BackgroundColor);

                Console.Write("RoundTrip time: ");
                Terminal.WriteText(reply.RoundtripTime.ToString(), ConsoleColor.Yellow, Console.BackgroundColor);

                Console.Write("Time to live  : ");
                Terminal.WriteText(reply.Options.Ttl.ToString(), ConsoleColor.Yellow, Console.BackgroundColor);

                Console.Write("Don't fragment: ");
                Terminal.WriteText(reply.Options.DontFragment.ToString(), ConsoleColor.Yellow, Console.BackgroundColor);

                Console.Write("Buffer size   : ");
                Terminal.WriteText(reply.Buffer.Length.ToString(), ConsoleColor.Yellow, Console.BackgroundColor);

                Console.WriteLine();
            }
            else
            {
                Terminal.ErrorWrite(reply.Status.ToString());
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
            Console.WriteLine("\t network ip ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t network ip show");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                        Console.WriteLine("\t network ip list");
            }
            Console.WriteLine("\t network ip list");
            Console.WriteLine("");
        }

        public void show()
        {
            String strHostName = string.Empty;
            try
            {
                strHostName = Dns.GetHostName();
                Terminal.WriteText("::Local Machine's Host Name: " + strHostName, ConsoleColor.Green, Console.BackgroundColor);

                IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
                IPAddress[] addr = ipEntry.AddressList;

                for (int i = 0; i < addr.Length; i++)
                {
                    Console.Write("  IP Address: ");
                    Terminal.WriteText(addr[i].ToString(), ConsoleColor.Yellow, Console.BackgroundColor);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite(error.Message);
            }

            Console.WriteLine();
        }

        public void list()
        {
            string strCmdText;
            strCmdText = "/all ";
            Terminal.ExecuteProcess("ipconfig.exe", strCmdText);
        }

    }


    public class INTERFACECLASS
    {
        public INTERFACECLASS() { }

        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t network interface ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("\t network interface help");
                Console.WriteLine("\t network interface show");
                Console.WriteLine("\t network interface enable <input: name>");
                Console.WriteLine("\t network interface disable <input: name>");
            }

            Console.WriteLine("");
        }

        public void show()
        {
            Terminal.WriteText("::Information on interfaces ", ConsoleColor.Green, Console.BackgroundColor);
            Terminal.ExecuteProcess("netsh.exe", "interface show interface");
        }

        public void enable(string value)
        {
            Terminal.WriteText("::Enable interface name  "+value, ConsoleColor.Green, Console.BackgroundColor);
            string cmd = String.Format("interface set interface name=\"{0}\" admin=ENABLED",value);
            if (Terminal.ExecuteProcess("netsh.exe", cmd) == 0)
            {
                Console.WriteLine("Enabled interface.");
            }  
                else
            Terminal.ErrorWrite("Failed enabled interface.");
        }

        public void disable(string value)
        {
            Terminal.WriteText("::Disable interface name  " + value, ConsoleColor.Green, Console.BackgroundColor);
            string cmd = String.Format("interface set interface name=\"{0}\" admin=DISABLED", value);
            if (Terminal.ExecuteProcess("netsh.exe", cmd) == 0)
            {
                Console.WriteLine("Disabled interface.");
            }
            else
                Terminal.ErrorWrite("Failed disabled interface.");
        }
    }


    public class WLANCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t Wlan controllere ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("\t network wlan help");
                Console.WriteLine("\t network wlan show");
                Console.WriteLine("\t network wlan connect <input1:profileName> <input2:interfaceName>");
                Console.WriteLine("\t network wlan disconnect <input:interfaceName>");
            }

            Console.WriteLine("");
        }

        public void show()
        {
            Terminal.WriteText("::List all wlan profiles ", ConsoleColor.Green, Console.BackgroundColor);
            Terminal.ExecuteProcess("netsh.exe", "wlan show profiles");
        }

        public void connect(string ssid, string interfaceName)
        {
            Terminal.WriteText("::Connection wlan to SSID " + ssid+" interface "+interfaceName, ConsoleColor.Green, Console.BackgroundColor);
            string cmd = String.Format("wlan connect  name=\"{0}\" interface=\"{1}\"", ssid, interfaceName);
            if (Terminal.ExecuteProcess("netsh.exe", cmd) == 0)
            {
                Console.WriteLine("Connected to WI-FI "+ssid);
            }
            else
                Terminal.ErrorWrite("Failed connect to "+ssid);
        }

        public void disconnect(string interfaceName)
        {
            Terminal.WriteText("::Disconnect interface name  " + interfaceName, ConsoleColor.Green, Console.BackgroundColor);
            string cmd = String.Format("wlan disconnect interface=\"{0}\"", interfaceName);
            if (Terminal.ExecuteProcess("netsh.exe", cmd) == 0)
            {
                Console.WriteLine("Disconnected interface.");
            }
            else
                Terminal.ErrorWrite("Failed disconnect interface.");
        }
    }


}