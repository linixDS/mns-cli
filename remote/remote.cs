using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core;
using FortiOS.Types;
using FortiOS.Client;
using System.Runtime.InteropServices;


namespace remote
{

    internal class FortiGateConfig
    {
        public int Port { get; set; }
        public string Address { get; set; }
        public string Token { get; set; }

        public FortiGateConfig()
        {
            this.Port = 0;
            this.Address = string.Empty;
            this.Token = string.Empty;
        }

    }

    internal class FortiGateProfiles
    {

        public FortiGateConfig load(string name)
        {
            try
            {
                var fileName = name + ".profile";
                if (!File.Exists(fileName)) return null;

                string json = File.ReadAllText(fileName);
                var config = JsonSerializer.Deserialize<FortiGateConfig>(json);
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

    internal class FortiGateClass
    {
        private List<FortiRegisterDevice> GetData()
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "fortigate" + Path.DirectorySeparatorChar + "default";
                var profile = new FortiGateProfiles();
                var config = profile.load(path);
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

        public List<string> GetAddressIP(string value)
        {
            List<string> result = new List<string>();
            try
            {
                List<FortiRegisterDevice> results = GetData();
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
    }


    public class HELPCLASS
    {
        public HELPCLASS()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t Remote ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t remote help");
            Console.WriteLine("\t remote profile");
            Console.WriteLine("\t remote vnc");
            Console.WriteLine("\t remote rdp");
            Console.WriteLine("\t remote pssession");
            Console.WriteLine("");
        }
    }

    public class SSHCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t Remote SSH Client");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t remote ssh help");
            Console.WriteLine("\t remote ssh connect <input1: ip address> <input2: port> <input3: user>");
            Console.WriteLine("\t remote ssh connect-name <input1: hostname> <input2: port> <input3: user>");
            Console.WriteLine("\t remote ssh load <input: profile>");
            Console.WriteLine("");
        }

        public void connect(string address, string port, string user)
        {
            Terminal.WriteText("::Connecting to SSH server: " + address+":"+port, ConsoleColor.Green, Console.BackgroundColor);
            try
            {
                string strCmdText;
                string command;
                strCmdText = String.Format("{0}@{1} -p {2}",user,address,port);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    command = "ssh.exe";
                }
                else
                {
                    command = "/usr/bin/ssh";
                }

                Terminal.ExecuteProcess(command, strCmdText);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

        public void connect_name(string hostname, string port, string user)
        {


            try
            {
                Terminal.WriteText("::Searching hostname from database FortiGate: " + hostname + ":" + port, ConsoleColor.Yellow, Console.BackgroundColor);
                FortiGateClass client = new FortiGateClass();
                var list = client.GetAddressIP(hostname);
                if (list == null) return;
                Terminal.WriteText("::Connecting to SSH server: " + hostname + ":" + port, ConsoleColor.Green, Console.BackgroundColor);
                var address = "";
                string strCmdText;
                string command;
                strCmdText = String.Format("{0}@{1} -p {2}", user, address, port);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    command = "ssh.exe";
                }
                else
                {
                    command = "/usr/bin/ssh";
                }

                Terminal.ExecuteProcess(command, strCmdText);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

        public void load(string profileName)
        {
            var profile = new PROFILECLASS();
            var config = profile.load(profileName);
            if (config == null)
            {
                Terminal.ErrorWrite("No found profile: " + profileName);
                return;
            }

            connect(config.Address, config.Port.ToString(), config.User);
        }
    }


    public class VNCCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t Remote VNC");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t remote vnc help");
            Console.WriteLine("\t remote vnc connect <input: ip address>");
            Console.WriteLine("");
        }

        public void connect(string address)
        {
            Terminal.WriteText("::Connecting to VNC server: " + address, ConsoleColor.Green, Console.BackgroundColor);
            try
            {
                string strCmdText;
                string command;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    strCmdText = address;
                    command = "vnc.exe";
                }
                else
                {
                    strCmdText = address;
                    command = "./vnc.sh";
                }

                Terminal.ExecuteProcess(command, strCmdText);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();

        }

    }

    public class PSSESSIONCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t PowerShell Session");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t remote pssession help");
            Console.WriteLine("\t remote psession  connect <input: ip address>");
            Console.WriteLine("");
        }

        public void connect(string address)
        {
            Terminal.WriteText("::Connecting to ComputerNamer: " + address, ConsoleColor.Green, Console.BackgroundColor);
            try
            {
                string strCmdText;
                string command;
                strCmdText = "-NoExit -Command " + "\"& {Enter-PSSession -Credential (Get-Credential) -ComputerName " + address + "}\"";

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    command = "powershell.exe";
                }
                else
                {
                    command = "pwsh";
                }

                Terminal.ExecuteProcess(command, strCmdText);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();

        }
    }


    public class RDPCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t Remote RDP Desktop");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t remote rdp help");
            Console.WriteLine("\t remote rdp connect <input: ip address>");
            Console.WriteLine("");
        }

        public void connect(string address)
        {
            Terminal.WriteText("::Connecting to RDP Desktop: " + address, ConsoleColor.Green, Console.BackgroundColor);
            try
            {
                string strCmdText;
                string command;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    strCmdText = "/v:"+address;
                    command = "mstsc.exe";
                }
                else
                {
                    strCmdText = address;
                    command = "./rdp.sh";
                }

                Terminal.ExecuteProcess(command, strCmdText);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();

        }
    }


}
