using System;

namespace binmea
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 1 )
            {
                return 1;
            }
            if (!System.IO.File.Exists(args[0]))
            {
                return 1;
            }
            var str = NMEAParserNET.Parser.PurgeWithinNMEA(args[0]);
            Console.Write(str);
            return 0;
        }
    }
}
