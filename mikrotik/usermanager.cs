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
    public class USERMANAGERCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t MikroTik UserManger ");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t mikrotik usermanger help");
            Console.WriteLine("\t mikrotik usermanger router <input: profile>");
            Console.WriteLine("\t mikrotik usermanger users <input: profile>");
            Console.WriteLine("\t mikrotik usermanger user <input1: id> <input2: profile>");
            Console.WriteLine("\t mikrotik usermanger user-search <input1: value> <input2: profile>");
            Console.WriteLine("\t mikrotik usermanger user-new <input1: name> <input2: password> <input3: group> <input4: profile>");
            Console.WriteLine("\t mikrotik usermanger user-change <input1: id> <input2: name> <input3: password> <input4: group> <input5: profile>");
            Console.WriteLine("\t mikrotik usermanger user-remove <input1: id> <input2: profile>");

            Console.WriteLine("\t mikrotik usermanger groups <input: profile>");
            Console.WriteLine("\t mikrotik usermanger group <input1: id> <input2: profile>");
            Console.WriteLine("");
        }


        public void router(string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    return;
                }
                Terminal.WriteText("::MikroTik Get User-Manager route from: " + config.Address, ConsoleColor.Green, Console.BackgroundColor);

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetRadiusRouter();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();
                string header1 = String.Format("{0,-1} {1,-6} {2,-25} {3,-18} {4,-6} {5,-25}",
                                                " ",
                                                "ID",
                                                "Name",
                                                "Address",
                                                "Port",
                                                "Secret");
                string header2 = String.Format("{0,-1} {1,-6} {2,-25} {3,-18} {4,-6} {5,-25}",
                                                " ",
                                                "----",
                                                "-----------------",
                                                "-----------------",
                                                "----",
                                                "-----------------");
                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;
                foreach (var info in result)
                {
                    if (info.Disabled == "false")
                       R = "";
                    else
                        R = "D";

                    Console.WriteLine("{0,-1} {1,-6} {2,-25} {3,-18} {4,-6} {5,-25}", 
                            R, 
                            info.Id, 
                            info.Name, 
                            info.Address, 
                            info.COAPort, 
                            info.SharedSecret);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }



        public void users(string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    return;
                }
                Terminal.WriteText("::MikroTik Get all list users from: " + config.Address, ConsoleColor.Green, Console.BackgroundColor);

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetUsersFromUserManager();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();
                string header1 = String.Format("{0,-1} {1,-6} {2,-18} {3,-25} {4,-10}  {5,-40}",
                                                " ",
                                                "ID",
                                                "Name",
                                                "Group",
                                                "Attributes",
                                                "Comment");
                string header2 = String.Format("{0,-1} {1,-6} {2,-18} {3,-25} {4,-10}  {5,-40}",
                                                " ",
                                                "------",
                                                "-----------------",
                                                "-----------------",
                                                "----------",
                                                "-----------------");
                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;
                foreach (var info in result)
                {
                    if (info.Disabled == "false")
                       R = "";
                    else
                        R = "D";
                    string[] attr = info.Attributes.Split(',');

                    Console.WriteLine("{0,-1} {1,-6} {2,-18} {3,-25} {4,-10}  {5,-40}", R, info.Id, info.Name, info.Group, attr.Length, info.Comment);
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }



        public void user(string id, string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    return;
                }
                Terminal.WriteText("::MikroTik Get detail information for user id: " + id, ConsoleColor.Green, Console.BackgroundColor);

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetUserFromUserManager(id);
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.Write("{0,-15} :  ", "ID"); Terminal.WriteText(result.Id, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.Write("{0,-15} :  ", "DISABLED"); Terminal.WriteText(result.Disabled, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.Write("{0,-15} :  ", "NAME"); Terminal.WriteText(result.Name, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.Write("{0,-15} :  ", "GROUP"); Terminal.WriteText(result.Group, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.Write("{0,-15} :  ", "PASSWORD"); Terminal.WriteText(result.Password, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.Write("{0,-15} :  ", "OTP SECRET"); Terminal.WriteText(result.OTPSecret, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.Write("{0,-15} :  ", "ATTRIBUTES");

                string[] attr = result.Attributes.Split(',');
                if (attr.Length > 0)
                {
                    Terminal.WriteText(attr[0], ConsoleColor.Yellow, Console.BackgroundColor);

                    for (var idx = 1; idx < attr.Length; idx++)
                    {
                        Terminal.WriteText(attr[idx], ConsoleColor.Yellow, Console.BackgroundColor);
                        Console.SetCursorPosition(19, Console.CursorTop);
                    }
                }
                else
                    Console.WriteLine();

                Console.Write("{0,-15} :  ", "SHARED USERS"); Terminal.WriteText(result.SharedUsers, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.Write("{0,-15} :  ", "COMMENT"); Terminal.WriteText(result.Comment, ConsoleColor.Yellow, Console.BackgroundColor);


            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

        public void user_search(string value, string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    return;
                }
                Terminal.WriteText("::Seearching user: " + value, ConsoleColor.Green, Console.BackgroundColor);

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetUsersFromUserManager();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();
                string header1 = String.Format("{0,-1} {1,-6} {2,-18} {3,-25} {4,-10}  {5,-40}",
                                                " ",
                                                "ID",
                                                "Name",
                                                "Group",
                                                "Attributes",
                                                "Comment");
                string header2 = String.Format("{0,-1} {1,-6} {2,-18} {3,-25} {4,-10}  {5,-40}",
                                                " ",
                                                "------",
                                                "-----------------",
                                                "-----------------",
                                                "----------",
                                                "-----------------");
                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                var R = String.Empty;
                foreach (var info in result)
                {
                    if (info.Name.Contains(value) ||
                        info.Group.Contains(value) ||
                        info.Comment.Contains(value))
                    {
                        if (info.Disabled == "true") R = "D";
                        string[] attr = info.Attributes.Split(',');

                        Console.WriteLine("{0,-1} {1,-6} {2,-18} {3,-25} {4,-10}  {5,-40}", R, info.Id, info.Name, info.Group, attr.Length, info.Comment);
                    }
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }


        public void user_new(string name, string password, string group, string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    return;
                }
                Terminal.WriteText("::MikroTik Add new user to User-Manger: " + config.Address, ConsoleColor.Green, Console.BackgroundColor);


                var user = new mtRequestNewUserData();
                user.Name = name;
                user.Password = password;
                user.Group = group;

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.CreateUserToUserManager(user);
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Terminal.WriteText("Created new user id: " + result.Id, ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

        public void user_change(string id, string name, string password, string group, string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    return;
                }
                Terminal.WriteText("::MikroTik Changing user data id: " + id, ConsoleColor.Green, Console.BackgroundColor);


                var user = new mtRequestNewUserData();
                user.Name = name;
                user.Password = password;
                user.Group = group;

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.UpdateeUserToUserManagerAsync(id, user);
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Terminal.WriteText("Changed user data id: " + result.Id, ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }


        public void user_remove(string id, string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    return;
                }
                Terminal.WriteText("::MikroTik Removing user data id: " + id, ConsoleColor.Green, Console.BackgroundColor);



                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.RemoveUserFromUserManager(id);
                if (!result)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Terminal.WriteText("Removed user data id: " + id, ConsoleColor.Green, Console.BackgroundColor);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }


        public void groups(string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    return;
                }
                Terminal.WriteText("::MikroTik Get all list groups from: " + config.Address, ConsoleColor.Green, Console.BackgroundColor);

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetGroupsFromUserManager();
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();
                string header1 = String.Format(" {0,-6} {1,-25} {2,-65}",
                                                "ID",
                                                "Name",
                                                "Attributes");
                string header2 = String.Format(" {0,-6} {1,-25} {2,-65}",
                                                "------",
                                                "-----------------",
                                                "-------------------------------------------------");
                Terminal.WriteText(header1, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.WriteLine(header2);

                foreach (var info in result)
                {
                    string[] attr = info.Attributes.Split(',');
                    if (attr.Length > 0)
                    {
                        Console.WriteLine(" {0,-6} {1,-25} {2,-65}", info.Id, info.Name, attr[0]);
                        for (var idx=1; idx < attr.Length; idx++)
                        {
                            Console.SetCursorPosition(34, Console.CursorTop);
                            Console.WriteLine(attr[idx]);
                        }
                    }
                    else
                    Console.WriteLine(" {0,-6} {1,-25} {2,-65}", info.Id, info.Name, "");
                }
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

        public void group(string id, string profileName)
        {
            try
            {
                var profile = new PROFILECLASS();
                var config = profile.load(profileName);
                if (config == null)
                {
                    Terminal.ErrorWrite("No found profile: " + profileName);
                    return;
                }
                Terminal.WriteText("::MikroTik Get detail group from id: " + id, ConsoleColor.Green, Console.BackgroundColor);

                var client = new MikroTikClientRestApi(config.Address, config.User, config.Password);
                var result = client.GetGroupFromUserManager(id);
                if (result == null)
                {
                    Terminal.ErrorWrite("Error: " + client.GetLastErrorMessage());
                    return;
                }

                Console.WriteLine();

                Console.Write("{0,-15} :  ", "ID"); Terminal.WriteText(result.Id, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.Write("{0,-15} :  ", "NAME"); Terminal.WriteText(result.Name, ConsoleColor.Yellow, Console.BackgroundColor);
                Console.Write("{0,-15} :  ", "ATTRIBUTES");

                string[] attr = result.Attributes.Split(',');
                if (attr.Length > 0)
                {
                    Terminal.WriteText(attr[0], ConsoleColor.Yellow, Console.BackgroundColor);

                    Console.SetCursorPosition(19, Console.CursorTop);
                    for (var idx = 1; idx < attr.Length; idx++)
                        Terminal.WriteText(attr[idx], ConsoleColor.Yellow, Console.BackgroundColor);
                }
                else
                    Console.WriteLine();

                Console.WriteLine();
                Console.Write("{0,-15} :  ", "INNER-AUTHS");
                attr = result.InnerAuths.Split(',');
                if (attr.Length > 0)
                {
                    Terminal.WriteText(attr[0], ConsoleColor.Yellow, Console.BackgroundColor);

                    for (var idx = 1; idx < attr.Length; idx++)
                    {
                        Console.SetCursorPosition(19, Console.CursorTop);
                        Terminal.WriteText(attr[idx], ConsoleColor.Yellow, Console.BackgroundColor);
                    }

                }
                else
                    Console.WriteLine();

                Console.WriteLine();
                Console.Write("{0,-15} :  ", "OUTER-AUTHS");
                attr = result.OuterAuths.Split(',');
                if (attr.Length > 0)
                {
                    Terminal.WriteText(attr[0], ConsoleColor.Yellow, Console.BackgroundColor);
                    for (var idx = 1; idx < attr.Length; idx++)
                    {
                        Console.SetCursorPosition(19, Console.CursorTop);
                        Terminal.WriteText(attr[idx], ConsoleColor.Yellow, Console.BackgroundColor);
                    }
                }
                else
                    Console.WriteLine();

            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: " + error.Message);
            }
            Console.WriteLine();
        }

    }
}
