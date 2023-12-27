using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;  
using Core;
using Runtime;
using System.IO.Compression;
using System.Net.Mail;

namespace module
{
    public class RemoteRepository
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }   

        [JsonPropertyName("description")]
        public string Description { get; set; }          

        [JsonPropertyName("update")]
        public string LastUpdate { get; set; }    

        [JsonPropertyName("package")]   
        public List<RemotePackage> Packages{ get; set; }      

        public RemoteRepository()
        {
            Name = "";
            Description = "";
            LastUpdate = "";
            Packages = new List<RemotePackage>();
        }
    }

    public class RemotePackage
    {
        [JsonPropertyName("package")]
        public PackageInfo Package{ get; set; }

        [JsonPropertyName("url")]
        public string url{ get; set; }

        public RemotePackage()
        {
            Package = new PackageInfo();
            url = "";
        }
    }


    public class LocalRepository
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }    


        public LocalRepository()
        {
            Name = "";
            Url = "";
        }
    }

    public class RepoBaseList
    {
        List<LocalRepository> Repository;

        public RepoBaseList()
        {
            Repository = new List<LocalRepository>();
        }
    }

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

        [JsonPropertyName("type")]
        public int TypePackage { get; set; }        

        public PackageInfo()
        {
            Name = String.Empty;
            Version = String.Empty;
            Author = String.Empty;
            Licence = String.Empty;
            Description = String.Empty;
            TypePackage = 1;
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
            Console.WriteLine("\t module package");
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


    public class REPOCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t Module.Repo ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\tManagment repositories");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t module repo help");
            Console.WriteLine("\t module repo add <input: url>");
            Console.WriteLine("\t module repo remove <input: name>");
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

        public static void Clear(string path)
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error clear modules: " + error.Message);
            }
        }

        public static bool Install(string package)
        {
            Directory.SetCurrentDirectory("..");
            var path = Environment.CurrentDirectory;
            string name = Path.GetFileNameWithoutExtension(package);
            var pathPackage = path+Path.DirectorySeparatorChar+name;

            try
            {
                Terminal.WriteText("Installing package: "+name, ConsoleColor.Green, Console.BackgroundColor);

                if (!File.Exists(package)) {
                    Terminal.ErrorWrite("Error: No found package:"+package);
                    return false;
                }



                Terminal.WriteText("    -> Extracting package: "+path, ConsoleColor.White, Console.BackgroundColor);
                ZipFile.ExtractToDirectory(package, path, true);
                
                Terminal.WriteText("    -> Checking package structure "+pathPackage, ConsoleColor.White, Console.BackgroundColor);
                var info = PACKAGECLASS.Load(pathPackage);
                if (info == null){
                    Terminal.ErrorWrite("Error: This is not package (No found header)!!!");
                    
                    PACKAGECLASS.Clear(pathPackage);
                    return false;
                }

                if (info.Name != name){
                    Terminal.ErrorWrite("Error: The data structure does not match.");
                    PACKAGECLASS.Clear(pathPackage);
                    return false;   
                }

      
                Terminal.WriteText("    -> Installed package", ConsoleColor.Green, Console.BackgroundColor);

            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
                PACKAGECLASS.Clear(pathPackage);
                return false;
            }

            return true;

        }        

        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t Module.Package ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\tManagment packages");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t module package help");
            Console.WriteLine("\t module package install <input: fullname>");
            Console.WriteLine("\t module package remove <input: name>");
        }        
        
        public void install(string package)
        {
            PACKAGECLASS.Install(package);
        }

        
        public void remove(string name)
        {
            Terminal.WriteText("::Remove package "+name, ConsoleColor.Green, Console.BackgroundColor);
            Console.WriteLine();
            Terminal.WriteText("You are sure is deleting package: "+name+"?  (y/N):  ", ConsoleColor.Yellow, Console.BackgroundColor);
            
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

                var info = PACKAGECLASS.Load(path);
                if (info == null){
                    Terminal.ErrorWrite("Error: This is not package (No found header)!!!");
                    return;                    
                }

                if (info.TypePackage == 1)
                {
                    Terminal.ErrorWrite("Error: Cannot remove is system module !");
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
