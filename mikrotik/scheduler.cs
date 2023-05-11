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
using CacheManagment;

namespace mikrotik
{
    public class SCHEDULERCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t MikroTik Scheduler");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t mikrotik scheduler help");

            Console.WriteLine("\t mikrotik scheduler list <input: profile>");
            Console.WriteLine("\t mikrotik scheduler enable <input1: id> <input2: profile>");
            Console.WriteLine("\t mikrotik scheduler disable <input1: id> <input2: profile>");

            Console.WriteLine("\t mikrotik scheduler delete <input1: id> <input2: profile>");
        }


        private CONFIGCLASS GetProfile(string profileName)
        {
            var profile = new PROFILECLASS();
            var config = profile.load(profileName);
            if (config == null)
            {
                Terminal.ErrorWrite("No found profile: " + profileName);
                return null;
            }
            return config;
        }

        private List<mtSchedulerInfo> GetList(string profileName)
        {
            try
            {
                var config = GetProfile(profileName);
                if (config == null) return null;

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetSchedulers();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return null;
                }

                return result;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
                return null;
            }
        }

        private List<mtSchedulerInfo> GetList(CONFIGCLASS config)
        {
            try
            {
                if (config == null) return null;

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetSchedulers();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return null;
                }

                return result;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
                return null;
            }
        }

        private mtSchedulerInfo Find(List<mtSchedulerInfo> list, string id)
        {
            var result = list.Find(x => x.Id == id);
            return result;
        }

        public void list(string profileName)
        {
            try
            {
                var config = GetProfile(profileName);
                var result = GetList(config);
                if (result == null) return;

                Terminal.WriteText("::MikroTik List schedulers : ", ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine();

                string header1 = String.Format("{0,-3} {1,-5} {2,-30} {3,-15} {4,-15} {5,-15} {6,-20} {7,-10} {8,-18} {9,-18}",
                                                "",
                                                "ID",
                                                "Name",
                                                "Start Date",
                                                "Start Time",
                                                "Interval",
                                                "Next Run",
                                                "Counter",
                                                "Owner",
                                                "On Event");
                string header2 = String.Format("{0,-3} {1,-5} {2,-30} {3,-15} {4,-15} {5,-15} {6,-20} {7,-10} {8,-18} {9,-18}",
                                                "",
                                                "-----",
                                                "------------",
                                                "-----------",
                                                "----------",
                                                "--------",
                                                "----------------",
                                                "-------",
                                                "-------------",
                                                "----------------------");

                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var status = "";
                foreach (var info in result)
                {
                    if (info.Comment.Length > 2)
                        Terminal.WriteText(";;"+info.Comment, ConsoleColor.Cyan, Console.BackgroundColor);

                    if (info.Disabled == "true") status = "D";

                    if (info.Comment.Length > 0) Terminal.WriteText(" ;; " + info.Comment, ConsoleColor.Cyan, Console.BackgroundColor);
                    Console.WriteLine("{0,-3} {1,-5} {2,-30} {3,-15} {4,-15} {5,-15} {6,-20} {7,-10} {8,-18} {9,-35}",
                                        status,
                                        info.Id,
                                        info.Name,
                                        info.StartDate,
                                        info.StartTime,
                                        info.Intervaal,
                                        info.NextRun,
                                        info.RunCount,
                                        info.Owner,
                                        info.OnEvent );
                }

            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

   

        public void delete(string id, string profileName)
        {
            try
            {
                var config = GetProfile(profileName);
                if (config == null) return;

                Terminal.WriteText("::MikroTik Delete scheduler ID: " + id, ConsoleColor.Green, Console.BackgroundColor);
                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.DeleteScheduler(id);
                if (result == false)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    Console.WriteLine();
                    return;
                }
                Terminal.WriteText("Deleted!", ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }


        private bool UpdateState(string id, bool enabled, string profileName)
        {
            try
            {
                var config = GetProfile(profileName);
                var result = GetList(config);
                if (result == null) return false;

                if (enabled)
                    Terminal.WriteText("::MikroTik Enable scheduler ID: "+id, ConsoleColor.Green, Console.BackgroundColor);
                else
                    Terminal.WriteText("::MikroTik Disble scheduler ID: " + id, ConsoleColor.Green, Console.BackgroundColor);
                
                Console.WriteLine();

                var data = Find(result, id);
                if (data == null)
                {
                    Terminal.ErrorWrite("No found scheduler id: " + id);
                    return false;
                }

                mtNewScheduler mt = data.CopyToNewScheduler();
                if (enabled)
                    mt.Disabled = "false";
                else
                    mt.Disabled = "true";

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var res = client.UpdateScheduler(id, mt);

                if (res == false)
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());

                return res;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            
            return false;
        }

        public void enable(string id, string profileName)
        {
            var result = UpdateState(id, true, profileName);
            if (result)
                Terminal.SuccessWrite("Done!");
            else
                Terminal.ErrorWrite("Failed!");
            Console.WriteLine();
        }

        public void disable(string id, string profileName)
        {
            var result = UpdateState(id, false, profileName);
            if (result)
                Terminal.SuccessWrite("Done!");
            else
                Terminal.ErrorWrite("Failed!");
            Console.WriteLine();
        }


    }
}
