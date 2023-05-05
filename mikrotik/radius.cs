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
using System.Runtime.InteropServices;


namespace mikrotik
{



    public class RADIUSCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t MikroTik Alias to UserManger Radius Master-Backup Server");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t mikrotik radius help");
            Console.WriteLine("\t mikrotik radius services");
            Console.WriteLine("\t mikrotik radius router");
            Console.WriteLine("\t mikrotik radius users");

            Console.WriteLine("\t mikrotik radius groups");
            Console.WriteLine("\t mikrotik radius search <input: value>");

            Console.WriteLine("\t mikrotik radius new-user <input1: name> <input2: group> <input3: comment>");
            Console.WriteLine("\t mikrotik radius remove-user: <input: name>");

            Console.WriteLine("\t mikrotik radius change-user <input1: name> <input2: newName|null> <input3: newgroup|null> <input4: newComment|null>");
            Console.WriteLine("\t mikrotik radius change-user-name <input1: name> <input2: newName>");
            Console.WriteLine("\t mikrotik radius change-user-group <input1: name> <input2: group> <input3: comment>");


            Console.WriteLine("\t mikrotik radius user-enable <input1: name>");
            Console.WriteLine("\t mikrotik radius user-disable <input1: name>");

            Console.WriteLine("");
        }

        private mtUserManagerInfo GetUser(string name, CONFIGCLASS config)
        {
            try
            {
                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetUsersFromUserManager();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    Console.WriteLine();
                    return null;
                }


                foreach (var item in result)
                {
                    var search = item.Name.ToUpper();
                    if (search == name.ToUpper())
                    {
                        return item;
                    }
                }

                return null;
            }
            catch (Exception error)
            {
                return null;
            }
        }

        private void PrintReportMaster(mtUserManagerInfo info)
        {
            if (info == null) return;
            Terminal.WriteText("Report to save master server:", ConsoleColor.Cyan, Console.BackgroundColor);
            Console.WriteLine("----------------------------------------------------");
            Console.Write("{0,-15} :  ", "ID"); Terminal.WriteText(info.Id, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.Write("{0,-15} :  ", "NAME"); Terminal.WriteText(info.Name, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.Write("{0,-15} :  ", "GROUP"); Terminal.WriteText(info.Group, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.Write("{0,-15} :  ", "DISABLED"); Terminal.WriteText(info.Disabled, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.Write("{0,-15} :  ", "COMMENT"); Terminal.WriteText(info.Comment, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.WriteLine();
        }

        private void PrintReportSlave(mtUserManagerInfo info)
        {
            if (info == null) return;

            Terminal.WriteText("Report to save backup server:", ConsoleColor.Blue, Console.BackgroundColor);
            Console.WriteLine("----------------------------------------------------");
            Console.Write("{0,-15} :  ", "ID"); Terminal.WriteText(info.Id, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.Write("{0,-15} :  ", "NAME"); Terminal.WriteText(info.Name, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.Write("{0,-15} :  ", "GROUP"); Terminal.WriteText(info.Group, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.Write("{0,-15} :  ", "DISABLED"); Terminal.WriteText(info.Disabled, ConsoleColor.Yellow, Console.BackgroundColor);
            Console.Write("{0,-15} :  ", "COMMENT"); Terminal.WriteText(info.Comment, ConsoleColor.Yellow, Console.BackgroundColor);
        }

        public void services()
        {
            try
            {
                var profile = new PROFILECLASS();
                var profileName = "master";
                var config1 = profile.load(profileName);
                if (config1 == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    Console.WriteLine();
                    return;
                }

     
                Terminal.WriteText("::MikroTik List radius services from : "+config1.Address, ConsoleColor.Green, Console.BackgroundColor);
                Console.WriteLine();


                var client = new MikroTikClientRestApi(config1.Address, config1.User, config1.Password);
                var result = client.GetRadiusServices();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    Console.WriteLine();
                    return;
                }

                string header1 = String.Format("{0,-1} {1,-4} {2,-15} {3,-18} {4,-10} {5,-6} {6,-15} {7,-12} {8,-35}",
                                                " ",
                                                "ID",
                                                "Service",
                                                "Address",
                                                "Protocol",
                                                "Port",
                                                "Secret",
                                                "Timeout",
                                                "Comment");
                string header2 = String.Format("{0,-1} {1,-4} {2,-15} {3,-18} {4,-10} {5,-6} {6,-15} {7,-12} {8,-35}",
                                                " ",
                                                "----",
                                                "----------",
                                                "---------------",
                                                "--------", 
                                                "----",
                                                "-----------",
                                                "-------",
                                                "-----------------");
                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;
                foreach (var info in result)
                {
                    if (info.Disabled == "true") R = "D";

                    Console.WriteLine("{0,-1} {1,-4} {2,-15} {3,-18} {4,-10} {5,-6} {6,-15} {7,-12} {8,-35}", 
                                        R, 
                                        info.Id, 
                                        info.ServiceName, 
                                        info.Address, 
                                        info.Protocol,
                                        info.AuthenticationPort,
                                        info.Secret,
                                        info.Timeout,
                                        info.Comment);
                }


            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }



        public void router()
        {
            var um = new USERMANAGERCLASS();
            um.router("master");
        }


        public void users()
        {
            var um = new USERMANAGERCLASS();
            um.users("master");
        }

        public void groups()
        {
            var um = new USERMANAGERCLASS();
            um.users("master");
        }

        public void search(string value)
        {
            var um = new USERMANAGERCLASS();

            um.user_search(value, "master");
        }

        public void new_user(string name, string group, string comment)
        {
            try
            {
                var profile = new PROFILECLASS();
                var profileName = "master";
                var config1 = profile.load(profileName);
                if (config1 == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    Console.WriteLine();
                    return;
                }

                profileName = "slave";
                var config2 = profile.load(profileName);
                if (config2 == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    Console.WriteLine();
                    return;
                }
                Terminal.WriteText("::MikroTik Create new user: ", ConsoleColor.Green, Console.BackgroundColor);

                var user = new mtRequestNewUserData();
                user.Name = name.ToUpper();
                user.Group = group.ToUpper();

                string[] head = group.Split("_");
                if (head.Length > 0)
                    user.Comment = head[0] + "|" + comment;
                else
                    user.Comment = comment;
                user.Comment = user.Comment.ToUpper();

                var client = new MikroTikClientRestApi(config1.Address, config1.User, config1.Password);
                var result = client.CreateUserToUserManager(user);
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    Console.WriteLine();
                    return;
                }
                Console.WriteLine();
                PrintReportMaster(result);

                client = new MikroTikClientRestApi(config2.Address, config2.User, config2.Password);
                result = client.CreateUserToUserManager(user);
                if (result == null)
                {
                    Terminal.ErrorWrite("Error save backup: " + client.GetLastErrorMessage());
                    Console.WriteLine();
                    return;
                }
                PrintReportSlave(result);

            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();  
        }


        private void changeUser(string name, string newName, string group, string comment, bool disabled)
        {
            try
            {
                var profile = new PROFILECLASS();
                var profileName = "master";
                var config1 = profile.load(profileName);
                if (config1 == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    Console.WriteLine();
                    return;
                }

                profileName = "slave";

                var config2 = profile.load(profileName);
                if (config2 == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    Console.WriteLine();
                    return;
                }
                Terminal.WriteText("::MikroTik Change user: "+name, ConsoleColor.Green, Console.BackgroundColor);

                Terminal.WriteText("::Checking user from server master: " + config1.Address, ConsoleColor.Yellow, Console.BackgroundColor);
                var currentUser1 = GetUser(name, config1);
                if (currentUser1 == null)
                {
                    Terminal.ErrorWrite("Error: No found user (master): " + name);
                    Console.WriteLine();
                    return;
                }

                Terminal.WriteText("::Checking user from server slave: " + config2.Address, ConsoleColor.Yellow, Console.BackgroundColor);
                var currentUser2 = GetUser(name, config2);
                if (currentUser2 == null)
                {
                    Terminal.ErrorWrite("Error: No found user (slave): " + name);
                    Console.WriteLine();
                    return;
                }


                var changeUser = new mtRequestNewUserData(currentUser1);

                if (disabled)
                    changeUser.Disabled = "true";
                else
                    changeUser.Disabled = "false";

                if (newName != "null")
                    changeUser.Name = newName;

                if (group != "null")
                {
                    changeUser.Group = group;
                }


                if (comment != "null" || group != changeUser.Group)
                {
                    string[] head = group.Split("_");
                    if (head.Length > 0)
                        changeUser.Comment = head[0] + "|" + comment;
                    else
                        changeUser.Comment = comment;
                }


                Terminal.WriteText("::Change user to master server: " + config1.Address, ConsoleColor.Magenta, Console.BackgroundColor);
                var client = new MikroTikClientRestApi(config1.Address, config1.User, config1.Password);
                var result1 = client.UpdateUserToUserManager(currentUser1.Id, changeUser);
                if (result1 == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    Console.WriteLine();
                    return;
                }
                Console.WriteLine();

                Terminal.WriteText("::Change user to slave server: " + config2.Address, ConsoleColor.Magenta, Console.BackgroundColor);
                client = new MikroTikClientRestApi(config2.Address, config2.User, config2.Password);

                var result2 = client.UpdateUserToUserManager(currentUser2.Id, changeUser);
                if (result2 == null)
                {
                    Terminal.ErrorWrite("Error save backup: " + client.GetLastErrorMessage());
                    Console.WriteLine();
                    return;
                }

                PrintReportMaster(result1);
                PrintReportSlave(result2);
                Terminal.WriteText("Saved data." , ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }


        public void change_user(string name, string newName, string group, string comment)
        {
            changeUser(name, newName, group, comment, false);
        }

        public void change_user_name(string name, string newName)
        {
            change_user(name, newName, "null", "null");
        }

        public void change_user_group(string name, string group, string comment)
        {
            change_user(name, "null", group, comment);
        }

        public void user_enable(string name)
        {
            changeUser(name, "null", "null", "null", false);
        }

        public void user_disable(string name)
        {
            changeUser(name, "null", "null", "null", true);
        }

        public void remove_user(string name)
        {
            try
            {
                var profile = new PROFILECLASS();
                var profileName = "master";
                var config1 = profile.load(profileName);
                if (config1 == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    Console.WriteLine();
                    return;
                }

                profileName = "slave";

                var config2 = profile.load(profileName);
                if (config2 == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    Console.WriteLine();
                    return;
                }
                Terminal.WriteText("::MikroTik Removing user: "+name, ConsoleColor.Green, Console.BackgroundColor);

                Terminal.WriteText("::Checking user from server master: " + config1.Address, ConsoleColor.Yellow, Console.BackgroundColor);
                var currentUser1 = GetUser(name, config1);
                if (currentUser1 == null)
                {
                    Terminal.ErrorWrite("Error: No found user (master): " + name);
                    Console.WriteLine();
                    return;
                }

                Terminal.WriteText("::Checking user from server slave: " + config2.Address, ConsoleColor.Yellow, Console.BackgroundColor);
                var currentUser2 = GetUser(name, config2);
                if (currentUser2 == null)
                {
                    Terminal.ErrorWrite("Error: No found user (slave): " + name);
                    Console.WriteLine();
                    return;
                }


                Terminal.WriteText("::Removing user to master server: " + config1.Address, ConsoleColor.Magenta, Console.BackgroundColor);
                var client = new MikroTikClientRestApi(config1.Address, config1.User, config1.Password);
                var result1 = client.RemoveUserFromUserManager(currentUser1.Id);
                if (!result1)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    Console.WriteLine();
                    return;
                }
                Console.WriteLine();

                Terminal.WriteText("::Removing user to slave server: " + config2.Address, ConsoleColor.Magenta, Console.BackgroundColor);
                client = new MikroTikClientRestApi(config2.Address, config2.User, config2.Password);

                var result2 = client.RemoveUserFromUserManager(currentUser2.Id);
                if (!result2)
                {
                    Terminal.ErrorWrite("Error save backup: " + client.GetLastErrorMessage());
                    Console.WriteLine();
                    return;
                }

                 Terminal.WriteText("Done." , ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }
    }
}