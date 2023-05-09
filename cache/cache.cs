using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core;
using CacheManagment;

namespace cache
{

    public class HELPCLASS
    {
        public HELPCLASS()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t Cache settings ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t cache help");
            Console.WriteLine("\t cache list");
            Console.WriteLine("\t cache remove");
            Console.WriteLine("\t cache clear");
            Console.WriteLine("\t cache edit");
            Console.WriteLine();
        }
    }

    public class LISTCLASS
    {
        public LISTCLASS()
        {
            Terminal.WriteText("::List all data cache", ConsoleColor.Green, Console.BackgroundColor);
            Console.WriteLine();
            try
            {
                var db = new DataCache();
                db.LoadConfig();
                if (db.Items == null)
                {
                    Console.WriteLine("List is empty.");
                    Console.WriteLine();
                    return;
                }
                string header1 = String.Format("{0,-3} {1,-18} {2,-45} {3}",    "", "LiveTime", "File cache", "Address URL");
                string header2 = String.Format("{0,-3} {1,-18} {2,-45} {3}", "", "---------------", "------------------------------------------", "------------------------------------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                string status = "";
                foreach (var item in db.Items)
                {
                    if (!item.Enabled) 
                        status = "D";
                    else
                    {
                        if (!db.IsRenewCache(item)) status = "A";
                    }

                    Console.WriteLine("{0,-3} {1,-18} {2,-45} {3}", status, item.ConvertToMinutes(), item.FileName, item.Name);
                    status = "";
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }
    }

    public class REMOVECLASS
    {
        public REMOVECLASS()
        {
            Terminal.WriteText("::Removing cache file", ConsoleColor.Green, Console.BackgroundColor);
            Console.WriteLine();
            try
            {
                var db = new DataCache();
                db.LoadConfig();
                if (db.Items == null)
                {
                    Console.WriteLine("List is empty.");
                    Console.WriteLine();
                    return;
                }

                List<ListValue> values = new List<ListValue>();
                foreach (var item in db.Items)
                {
                    values.Add(new ListValue(item.Name, item.Name));
                }
                values.Add(new ListValue("Cancel","Cancel"));

                string choice = Input.ChoiceList(values, "Selecting cache name to delete:", false);
                if (choice == "Cancel") return;

                Terminal.WriteText2("You sure to delete ?   (Y/n):  ", ConsoleColor.Yellow, Console.BackgroundColor);

                bool confirm = Input.ChoiceYesNo();
                if (confirm)
                {
                    if (db.RemoveCacheFile(choice))
                        Terminal.SuccessWrite("Deleted file.");
                    else
                        Terminal.ErrorWrite("Failed deleted file.");
                }
                else
                    Terminal.WarnWrite("Cancled !");
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }
    }


    public class EDITCLASS
    {
        public EDITCLASS()
        {
            Terminal.WriteText("::Change setting cache", ConsoleColor.Green, Console.BackgroundColor);
            Console.WriteLine();
            try
            {
                var db = new DataCache();
                db.LoadConfig();
                if (db.Items == null)
                {
                    Console.WriteLine("List is empty.");
                    Console.WriteLine();
                    return;
                }

                List<ListValue> values = new List<ListValue>();
                foreach (var item in db.Items)
                {
                    values.Add(new ListValue(item.Name, item.Name));
                }
                values.Add(new ListValue("Cancel", "Cancel"));

                string choice = Input.ChoiceList(values, "Selecting cache name to edit:", false);
                if (choice == "Cancel") return;

                var cache = db.Find(choice);
                if (cache == null)
                {
                    Terminal.ErrorWrite("No found record.");
                    Console.WriteLine();
                    return;
                }

                Terminal.WriteText(string.Format("Current live time (sec): {0}", cache.LiveTimeSec), ConsoleColor.Cyan, Console.BackgroundColor);
                var newValue = Input.Number("Set new value", 1, 100000000);


                Terminal.WriteText2("You sure to saved?   (Y/n):  ", ConsoleColor.Yellow, Console.BackgroundColor);

                bool confirm = Input.ChoiceYesNo();
                if (confirm)
                {
                    cache.LiveTimeSec = newValue;
                    db.SaveConfig();
                    Terminal.SuccessWrite("Saved cache database!");
                }
                else
                    Terminal.WarnWrite("Cancled !");
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }
    }




    public class CLEARCLASS
    {
        public CLEARCLASS()
        {
            Terminal.WriteText("::Clearing all files cache", ConsoleColor.Green, Console.BackgroundColor);
            Console.WriteLine();
            try
            {
                var db = new DataCache();
                db.LoadConfig();
                if (db.Items == null)
                {
                    Console.WriteLine("List is empty.");
                    Console.WriteLine();
                    return;
                }

                Terminal.WriteText2("You sure to clear all files cache?   (Y/n):  ", ConsoleColor.Yellow, Console.BackgroundColor);

                bool confirm = Input.ChoiceYesNo();
                if (confirm)
                {
                    if (db.Clear())
                        Terminal.SuccessWrite("Deleted file.");
                    else
                        Terminal.ErrorWrite("Failed deleted file.");
                }
                else
                    Terminal.WarnWrite("Cancled !");
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }
    }

}
