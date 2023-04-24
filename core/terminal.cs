namespace Core
{
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
