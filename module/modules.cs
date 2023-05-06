using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;  
using Core;
using Runtime;

namespace module
{

    public class PackageInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("version")]
        public string Version { get; set; }
        [JsonPropertyName("author")]
        public string Author { get; set; }
        [JsonPropertyName("licence")]
        public string Licence { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        public PackageInfo()
        {
            Name = String.Empty;
            Version = String.Empty;
            Author = String.Empty;
            Licence = String.Empty;
            Description = String.Empty;
        }
    }
    
    public class PACKAGECLASS
    {
        public static PackageInfo Load(string path)
        {
            try
            {
                var fileName = path+Path.DirectorySeparatorChar+"package.json";
                if (!File.Exists(fileName))  return null;

                string json = File.ReadAllText(fileName);
                var info = JsonSerializer.Deserialize<PackageInfo>(json);
    
                return info;
            }
            catch (Exception error)
            {
            //    Terminal.ErrorWrite("Error: " + error.Message);
                return null;
            }
        }
    }
    
    public class HELPCLASS
    {
        public HELPCLASS()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t Module Managment ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\tGet information got modules");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t module help");
            Console.WriteLine("\t module list");
            Console.WriteLine("\t module mngt");
            Console.WriteLine("\t module repo");
            Console.WriteLine("");
        }
    }


    public class GLOBALHELPCLASS
    {
        public GLOBALHELPCLASS()
        {
            string path = "." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar;
            string[] dirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);

            Console.Clear();
            Console.WriteLine("Network Managment Servces - Client Terminal v0.1");
            Console.WriteLine("Copyrigh(c) 2023 by Dariusz Marcisz");
            Console.WriteLine("");

            Console.WriteLine("NAME");
            Console.WriteLine("\t Global help ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\tList of available modules.");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            foreach (string dir in dirs)
            {
                var info = PACKAGECLASS.Load(dir);
                string version = String.Empty;
                string author = String.Empty;
                string desc = String.Empty;
                string name = dir.Replace(path, "");

                if (info != null)
                {
                    name = info.Name;
                    Console.WriteLine(string.Format("\t {0,-15} help",name,"help"));
                }


            }

            Console.WriteLine("");
        }
    }

    public class LISTCLASS
    {
        public LISTCLASS()
        {
            Terminal.WriteText("::List all modules installed", ConsoleColor.Green, Console.BackgroundColor);
            Console.WriteLine();
            try
            {
                string path = "."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar;
                string[] dirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);

                string header1 = String.Format("{0,-25} {1,-12} {2,-25} {3,-30}", "Name","Version","Author","Description");
                string header2 = String.Format("{0,-25} {1,-12} {2,-25} {3,-30}","-----------------------","----------", "-----------------------","-----------------------");
        
                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2); 
                
                foreach (string dir in dirs)
                {
                    var info = PACKAGECLASS.Load(dir);
                    string version = String.Empty;
                    string author = String.Empty;
                    string desc = String.Empty;
                    string name = dir.Replace(path,"");
                    
                    if (info != null){
                        version = info.Version;
                        author = info.Author;
                        desc = info.Description;
                        name = info.Name;
                    }
                    
                    Console.WriteLine("{0,-25} {1,-12} {2,-25} {3,-30}", name, version, author, desc);
                }
            }
            catch (Exception error)
            {
                  Terminal.ErrorWrite("Error: "+error.Message);
            }
            Console.WriteLine();
        }
    }
    
    public class MNGTCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t Module.MNGT ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\tManagment modules");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t module mngt help");
            Console.WriteLine("\t module mngt remove <input: name>");
            Console.WriteLine("");
        }        
        
        public void remove(string name)
        {
            Terminal.WriteText("::Remove module "+name, ConsoleColor.Green, Console.BackgroundColor);
            Console.WriteLine();
            Terminal.WriteText("You are sure is deleting module: "+name+"?  (y/N):  ", ConsoleColor.Yellow, Console.BackgroundColor);
            
            var key = ConsoleKey.Enter;
            while (key != ConsoleKey.Y) 
            {
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.N)
                {
                    Terminal.ErrorWrite("Cancel"); 
                    return;
                }
                
                if (key == ConsoleKey.Y) break;
            }
            Console.WriteLine();
            try
            {
                string path = "."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+name;
                if (!Directory.Exists(path))
                {
                    Terminal.ErrorWrite("Error: No found module: "+name);
                    return;
                }
                Terminal.WriteText("Removing module: "+name, ConsoleColor.Yellow, Console.BackgroundColor);
                Directory.Delete(path, true);
                Terminal.WriteText("Removed module "+name, ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                  Terminal.ErrorWrite("Error: "+error.Message);
            }
            Console.WriteLine();
        }
    }    
}
