using System;
using Sec;

namespace SecC
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please specify a file to read.");
                return;
            }

            var sec = SecFile.Open(args[0]);
            Console.WriteLine(sec);
        }
    }
}
