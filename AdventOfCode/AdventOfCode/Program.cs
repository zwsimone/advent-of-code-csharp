using System.Reflection;
using Utilities;

namespace AdventOfCode
{
    class Program
    {
        private static readonly string SolutionPath = FileUtilities.GetProjectSolutionPath();
        private static readonly string[] AocFolderPaths = Directory.GetDirectories(SolutionPath, "AOC*");

        private static void RunMainMenu()
        {
            string prompt = @"
 █████╗ ██████╗ ██╗   ██╗███████╗███╗   ██╗████████╗     ██████╗ ███████╗     ██████╗ ██████╗ ██████╗ ███████╗    ██████╗ ██╗   ██╗███╗   ██╗███╗   ██╗███████╗██████╗ 
██╔══██╗██╔══██╗██║   ██║██╔════╝████╗  ██║╚══██╔══╝    ██╔═══██╗██╔════╝    ██╔════╝██╔═══██╗██╔══██╗██╔════╝    ██╔══██╗██║   ██║████╗  ██║████╗  ██║██╔════╝██╔══██╗
███████║██║  ██║██║   ██║█████╗  ██╔██╗ ██║   ██║       ██║   ██║█████╗      ██║     ██║   ██║██║  ██║█████╗      ██████╔╝██║   ██║██╔██╗ ██║██╔██╗ ██║█████╗  ██████╔╝
██╔══██║██║  ██║╚██╗ ██╔╝██╔══╝  ██║╚██╗██║   ██║       ██║   ██║██╔══╝      ██║     ██║   ██║██║  ██║██╔══╝      ██╔══██╗██║   ██║██║╚██╗██║██║╚██╗██║██╔══╝  ██╔══██╗
██║  ██║██████╔╝ ╚████╔╝ ███████╗██║ ╚████║   ██║       ╚██████╔╝██║         ╚██████╗╚██████╔╝██████╔╝███████╗    ██║  ██║╚██████╔╝██║ ╚████║██║ ╚████║███████╗██║  ██║
╚═╝  ╚═╝╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═══╝   ╚═╝        ╚═════╝ ╚═╝          ╚═════╝ ╚═════╝ ╚═════╝ ╚══════╝    ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝

Welcome to the Advent of Code Runner.
Please select one of the following options.
(Use arrow keys to navigate, press enter to select)";
            string[] options = { "Latest", "Custom", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    string yearFolderName = FileUtilities.GetFolderNameFromPath(AocFolderPaths[^1]);
                    string dayFolderName = FileUtilities.GetFolderNameFromPath(Directory.GetDirectories(AocFolderPaths[^1], "Day*")[^1]);
                    RunDay(yearFolderName, dayFolderName);
                    break;
                case 1:
                    RunCustomMenu();
                    break;
                case 2:
                    ExitGame();
                    break;
            }
        }

        private static void RunCustomMenu()
        {
            string prompt = @"
Please select the year.
(Use arrow keys to navigate, press enter to select)";
            List<string> options = new List<string>();
            foreach (string s in AocFolderPaths.ToList())
            {
                options.Add(FileUtilities.GetFolderNameFromPath(s));
            }
            options.Add("Back");
            Menu mainMenu = new Menu(prompt, options.ToArray());
            int selectedIndex = mainMenu.Run();
            
            if (selectedIndex == options.Count - 1)
            {
                RunMainMenu();
            }
            else
            {
                string yearFolderName = options[selectedIndex];
                RunDayMenu(yearFolderName, selectedIndex);
            }
        }

        private static void RunDayMenu(string yearFolderName, int yearIndex)
        {
            string prompt = @"
Please select the day.
(Use arrow keys to navigate, press enter to select)";
            List<string> options = new List<string>();
            string[] dayFolders = Directory.GetDirectories(AocFolderPaths[yearIndex], "Day*");
            foreach (string s in dayFolders.ToList())
            {
                options.Add(FileUtilities.GetFolderNameFromPath(s));
            }

            options.Add("Back");
            Menu dayMenu = new Menu(prompt, options.ToArray());
            int selectedIndex = dayMenu.Run();

            if (selectedIndex == options.Count - 1)
            {
                RunMainMenu();
            }
            else
            {
                string dayFolderName = options[selectedIndex];
                RunDay(yearFolderName, dayFolderName);
            }
        }

        private static void RunDay(string yearFolderName, string dayFolderName)
        {
            Console.WriteLine("Running {0}, {1}...", yearFolderName, dayFolderName);
            Type? type = Type.GetType($"{yearFolderName}.{dayFolderName}.{dayFolderName}, {yearFolderName}");
            if (type != null)
            {
                object instance = Activator.CreateInstance(type);
                MethodInfo methodInfo = type.GetMethod("Run");
                if (methodInfo != null)
                {
                    methodInfo.Invoke(instance, null);
                }
                else
                {
                    Console.WriteLine("Method 'Run' not found: {0}.{1}", yearFolderName, dayFolderName);
                }
            }
            else
            {
                Console.WriteLine("Class '{0}' not found in namespace '{1}.{0}'", dayFolderName, yearFolderName);
            }
        }

        private static void ExitGame()
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            RunMainMenu();
        }
    }
}