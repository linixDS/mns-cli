using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core;

namespace module
{

    public class RepositoryInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("servers")]
        public List<string> ServerUrl { get; set; }
        [JsonPropertyName("priority")]
        public int Priority { get; set; }

        public RepositoryInfo()
        {
            Name = String.Empty;
            ServerUrl = new List<String>();
            Priority = 1;
        }
    }

    public class REPOCLASS
    {

        private static RepositoryInfo LoadRepo(string name)
        {
            try
            {
                var fileName = name = ".repo";
                if (!File.Exists(fileName))
                {
                    Terminal.ErrorWrite("Error: No found repository: " + name);

                }

                string json = File.ReadAllText(fileName);
                var info = JsonSerializer.Deserialize<RepositoryInfo>(json);

                return info;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
                return null;
            }
        }

        private void SaveRepo(RepositoryInfo repo, string name)
        {
            try
            {
                var fileName = name + ".repo";
                if (File.Exists(fileName))
                {
                    Terminal.ErrorWrite("Error: Repository already exists: " + name);
                    return;
                }

                var json = JsonSerializer.Serialize(repo);

                StreamWriter writer = new StreamWriter(fileName);
                writer.WriteLine(json);
                writer.Close();
                Terminal.WriteText("Saved new repository.", ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
                return;
            }
        }


        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t Module.REPO ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\tManagment module repesitory");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t module repo help");
            Console.WriteLine("\t module repo list");
            Console.WriteLine("\t module repo add <input: name>");
            Console.WriteLine("\t module repo remove <input: name>");
            Console.WriteLine("");
        }

        public void list()
        {
            Terminal.WriteText("::List all repository installed", ConsoleColor.Green, Console.BackgroundColor);
            Console.WriteLine();
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());

            string header1 = string.Format(" {0,-20}", "Repo name");
            string header2 = string.Format(" {0,-20}", "--------------------------------");
            Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.WriteLine(header2);

            foreach (string file in files)
            {
                string ext = Path.GetExtension(file);
                if (ext.Contains(".repo"))
                {
                    Console.WriteLine(" {0,-20}", Path.GetFileNameWithoutExtension(file));
                }
            }
            Console.WriteLine();
        }

        public void remove(string name)
        {
            var fileName = name + ".repo";
            if (!File.Exists(fileName))
            {
                Terminal.ErrorWrite("Error: No found repository: " + name);
                return;
            }

            try
            {
                File.Delete(fileName);
                Terminal.WriteText("Deleted repository.", ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
        }

        public void add(string name)
        {
            if (name.Length < 2)
            {
                Terminal.ErrorWrite("Error: Incorrect name repo! ");
                return;
            }

            var repo = new RepositoryInfo();
            repo.Name = name;

            Terminal.WriteText("::Add new repository", ConsoleColor.Green, Console.BackgroundColor);
            Console.WriteLine();


            try
            {
                Console.Write("SERVER URL> ");
                var url = Console.ReadLine();

                if (url.Length < 4)
                {
                    Terminal.ErrorWrite("Error: Incorrect argument! ");
                    return;
                }
                repo.ServerUrl.Add(url);

                SaveRepo(repo, name);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }
    }
}
