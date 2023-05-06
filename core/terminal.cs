namespace Core
{

    public class Input
    {
        public static bool ChoiceYesNo()
        {
            LabelChoiceYesNo:
            var cki = Console.ReadKey();

            if (cki.KeyChar != 'Y' &&
                cki.KeyChar != 'y' &&
                cki.KeyChar != 'n' &&
                cki.KeyChar != 'N')
            {
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                goto LabelChoiceYesNo;
            }

            if (cki.KeyChar == 'Y' || cki.KeyChar == 'y')
                return true;
            else
                return false;
        }

        public static string ChoiceList(List<string> list, string title)
        {
            Console.WriteLine(title);
            var idx = 1;
            foreach (var txt in list)
            {
                Console.WriteLine("   {0,-3} {1}", idx.ToString() + ".", list[idx - 1]);
                idx++;
            }
            Console.WriteLine();


            LabelChoiceSelect:

            Console.Write("Choice [1-{0}]:\t", list.Count);
            var posX = Console.CursorLeft;
            Console.Write("                  ");
            Console.SetCursorPosition(posX, Console.CursorTop);
            var input = Console.ReadLine();


            int choice;
            if (!int.TryParse(input, out choice))
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                goto LabelChoiceSelect;
            }

            if (choice < 1 || choice > list.Count)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                goto LabelChoiceSelect;
            }


            return list[choice - 1];
        }
    }


    public class Terminal
    {
        public static int ExecuteProcess(string cmd, string args, bool wait = true)
        {
            try
            {
                var process = System.Diagnostics.Process.Start(cmd, args);
                if (wait) 
                {
                    process.WaitForExit();
                    return process.ExitCode;
                }
                    else
                return 0;
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error: "+error.Message);
                return -1;
            }
        }

        public static void WriteText(string message, ConsoleColor fg, ConsoleColor bg, bool isError = false)
        {
            ConsoleColor currentBackground = Console.BackgroundColor;
            ConsoleColor currentForeground = Console.ForegroundColor;

            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;

            if (isError)
                Console.Error.WriteLine(message);
            else
            {
                 Console.WriteLine(message);
            }

            Console.ForegroundColor = currentForeground;
            Console.BackgroundColor = currentBackground;
        }
        

    

        public static void ErrorWrite(string message)
        {
            Terminal.WriteText(message, ConsoleColor.Red, Console.BackgroundColor, true);
        }

        public static void ColorWrite(string text, ConsoleColor color)
        {
            Terminal.WriteText(text, color, Console.BackgroundColor, true);
        }

    }
}
