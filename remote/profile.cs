using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace remote
{
    public class CONFIGCLASS
    {
        public int Port { get; set; }
        public string Address { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public CONFIGCLASS()
        {
            this.Port= 22;
            this.Address = string.Empty;
            this.User = "admin";
            this.Password = string.Empty;
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
            Console.WriteLine("\t remote profile help");
            Console.WriteLine("\t remote profile list");
            Console.WriteLine("\t remote profile remove <input: name>");
            Console.WriteLine("\t remote profile add <input: name>");
            Console.WriteLine("");
        }

        public void list()
        {
            Terminal.WriteText("::List all profiles in remote module", ConsoleColor.Green, Console.BackgroundColor);
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
                Console.Write("USER> ");
                config.User = Console.ReadLine();

                Console.Write("PASSWORD> ");
                config.Password = Console.ReadLine();
                if (config.Password.Length > 0)
                    config.Password = Core.Crypto.Encrypt(config.Password, "hoff01HOFF02");



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
                if (config.Password.Length > 1)
                    config.Password = Core.Crypto.Decrypt(config.Password, "hoff01HOFF02");

                return config;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
                return null;
            }
        }
    }
}
