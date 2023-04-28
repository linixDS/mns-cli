using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core;
using System.Runtime.InteropServices;


namespace remote
{
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
            Console.WriteLine("\t remote vnc");
            Console.WriteLine("\t remote rdp");
            Console.WriteLine("\t remote pssession");
            Console.WriteLine("");
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
