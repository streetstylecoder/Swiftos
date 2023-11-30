using swiftos.input;
using System;
using System.Collections.Generic;

namespace swiftos.shell
{
    public static class Shell
    {
        public static void Main()
        {
            nclearos.swiftos.Main(Input.Main("", ConsoleColor.Green));
        }
        public static void Check()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("| OK |");
            Console.ResetColor();
            Console.Write(" SWIFT os Shell\n");
        }
    }
}