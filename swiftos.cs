using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using System.Threading;
using swiftos.lib;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using swiftos.date;
using swiftos.sysinfo;
using Cosmos.Core;
using swiftos.files;
using swiftos.text;
using swiftos.loading;
using swiftos.input;
using System.Runtime.CompilerServices;

namespace swiftos.nclearos
{
    public static class swiftos
    {
        public static string input { get; set; }
        public static bool echo { get; set; }
        public static string prompt { get; set; }
        public static void Main(string input)
        {
            switch (input)
            {
                case "help":
                    Console.WriteLine("SWift os Help --------------- GENERAL SHORTCUTS\nF1 - display help\nTAB - quit app\nESC - cancel input\nArrows Up/Down - browse command history");
                    Console.WriteLine("\nSwift os Help --------------- GENERAL  COMMANDS\nhelp - display help\nsd / shutdown - turn off computer\nrb / reboot - restart computer\ninfo - information about OS and system components\nabout - display information about system\nver / vesrion - display system version\ncls / clear - clear screen\nlib - library of apps\nerr - check Kernel error handling\necho - echo message\nsound - play beep\ndate / time - dipslay current date");
                    break;
                case "about":
                    Console.WriteLine("About NclearOS\n--------\n" + Kernel.CurrentVersion + "\nBased on CosmosOS" + "\nCreated by Harsh Dhariwal,Shashwat ahuja, parnika jain and sritama \nGithub: https://github.com/streetstylecoder \nWebsite: http://harshdhariwal.site");
                    break;
                case "shutdown" or "turnoff" or "sd":
                    Shutdown(false);
                    break;
                case "reboot" or "restart" or "rb":
                    Shutdown(true);
                    break;
                case "err":
                    Kernel.ErrorScreen("Attempted execute");
                    break;
                case "lib":
                    Lib.Main();
                    break;
                case "prompt":

                    break;
                case "cls" or "clear":
                    ClearConsole(Console.WindowHeight, false);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(Kernel.CurrentVersion);
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "echo":
                    Console.WriteLine("Usage: echo 'message'");
                    break;
                case { } when input.StartsWith("echo "):
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(input.Substring(5));
                    break;
                case "pause":
                    Console.Write("Press any key to continue...");
                    Console.ReadKey(true);
                    break;
                case "sound":
                    Console.Beep();
                    break;
                case "system/Return":
                    Kernel.KernelScreen();
                    break;
                case "ver" or "version":
                    Console.WriteLine(Kernel.CurrentVersion);
                    break;
                case "time" or "date":
                    try { Console.WriteLine(Date.Main()); }
                    catch (Exception e) { Console.WriteLine("Service 'Date' crashed\n" + e); }
                    break;
                case "services":
                    break;
                case "round robin scheduling":
                    RunRoundRobinScheduling();
                    break;
                case "fcfs scheduling":
                    RunFCFSScheduling();
                    break;
                case "info":
                    Console.WriteLine("System Info\n-----------");
                    try { Sysinfo.Main(); }
                    catch (Exception e) { Console.WriteLine("Service 'System Info' crashed\n" + e); }
                    break;
                case { } when input.StartsWith("ping "):
                    break;
                case "" or null:
                    break;
                case "load":
                    Loading.Main(50);
                    Console.WriteLine(VFSManager.GetDisks());
                    break;
                 case "color":
                    switch (Console.BackgroundColor)
                    {
                        case ConsoleColor.Gray:
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            break;
                        case ConsoleColor.DarkGray:
                            Console.BackgroundColor = ConsoleColor.Blue;
                            break;
                        case ConsoleColor.Blue:
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            break;
                        case ConsoleColor.DarkBlue:
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;
                        default:
                            Console.BackgroundColor = ConsoleColor.Gray;
                            break;
                    }
                    Console.Clear();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Unknown command '" + input + "', type help for list of commands.");
                    break;
            }
        }

        private static void RunFCFSScheduling()
        {
            throw new NotImplementedException();
        }

            
        private static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
      

       

        private static void RunRoundRobinScheduling()
        {
            Console.WriteLine("Enter the number of processes:");
            int n = int.Parse(Console.ReadLine());
            int[] processes = new int[n];
            int[] burst_time = new int[n];

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Enter burst time for process {i + 1}:");
                burst_time[i] = int.Parse(Console.ReadLine());
                processes[i] = i + 1;
            }

            Console.WriteLine("Enter the quantum time:");
            int quantum = int.Parse(Console.ReadLine());

            FindAvgTime(processes, n, burst_time, quantum);
        }

        private static void FindAvgTime(int[] processes, int n, int[] bt, int quantum)
        {
            int[] wt = new int[n];
            int[] tat = new int[n];
            int total_wt = 0, total_tat = 0;

            FindWaitingTime(processes, n, bt, wt, quantum);
            FindTurnAroundTime(processes, n, bt, wt, tat);

            Console.WriteLine("Processes " + " Burst time " + " Waiting time " + " Turn around time");

            for (int i = 0; i < n; i++)
            {
                total_wt += wt[i];
                total_tat += tat[i];
                Console.WriteLine(" " + processes[i] + "\t\t" + bt[i] + "\t " + wt[i] + "\t\t " + tat[i]);
            }

            Console.WriteLine("Average waiting time = " + (float)total_wt / n);
            Console.WriteLine("Average turn around time = " + (float)total_tat / n);
        }

        private static void FindWaitingTime(int[] processes, int n, int[] bt, int[] wt, int quantum)
        {
            int[] rem_bt = new int[n];
            for (int i = 0; i < n; i++)
                rem_bt[i] = bt[i];

            int t = 0;
            while (true)
            {
                bool done = true;
                for (int i = 0; i < n; i++)
                {
                    if (rem_bt[i] > 0)
                    {
                        done = false;
                        if (rem_bt[i] > quantum)
                        {
                            t += quantum;
                            rem_bt[i] -= quantum;
                        }
                        else
                        {
                            t += rem_bt[i];
                            wt[i] = t - bt[i];
                            rem_bt[i] = 0;
                        }
                    }
                }
                if (done == true)
                    break;
            }
        }

        private static void FindTurnAroundTime(int[] processes, int n, int[] bt, int[] wt, int[] tat)
        {
            for (int i = 0; i < n; i++)
                tat[i] = bt[i] + wt[i];
        }
        public static void Boot()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("| OK |");
            Console.ResetColor();
            Console.Write(" SWift os Core\n");
        }
        public static void Shutdown(bool reboot)
        {
            if (Files.content != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("There are unsaved work in app 'Text', do you want to shutdown anyway?\nEnter - continue shutdown and delete unsaved data\nESC - cancel shutdown\nTAB - save data");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Escape:
                        goto exit;
                    case ConsoleKey.Tab:
                        Files.Save();
                        Loading.Main(5);
                        Shutdown(reboot);
                        break;
                    case ConsoleKey.Enter:
                        break;
                    default:
                        return;
                }
            }
            Console.CursorVisible = false;
            ClearConsole(Console.WindowHeight, true);
            Console.ResetColor();
            Console.Clear();
            Loading.Main(10);
            if (reboot == true) { Sys.Power.Reboot(); ACPI.Shutdown(); } else { Sys.Power.Shutdown(); ACPI.Reboot(); }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(18, Console.CursorTop = Console.WindowHeight / 2 - 1);
            Console.Write("It is now safe to turn off computer.");
            while (true) ;
            exit:
            Console.WriteLine("\nShutdown cancelled");
        }
        public static void ClearConsole(int height, bool sound)
        {
            if (sound)
            {
                Console.Beep(700, 100);
                Console.WriteLine();
                Console.Beep(600, 100);
                Console.WriteLine();
                Console.Beep(500, 100);
                Console.WriteLine();
                Console.Beep(400, 100);
                Console.WriteLine();
                Console.Beep(300, 100);
                Console.WriteLine();
                Console.Beep(300, 100);
                height -= 5;
                for (int i = 0; i <= height; i++)
                {
                    Thread.Sleep(50);
                    Console.WriteLine();
                }

            }
            else
            {
                for (int i = 0; i <= height; i++)
                {
                    Thread.Sleep(50);
                    Console.WriteLine();
                }
            }
        }
        public static void RefreshScreen(int height)
        {
            for (int i = 0; i <= height; i++)
            {
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(Kernel.CurrentVersion);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nSuccesfully switched color");
        }
    }
}