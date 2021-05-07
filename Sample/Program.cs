using System;
using NMEAParserNET;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var gga = new GGA("GNGGA,035817.30,3444.3418019,N,13535.1207337,E,5,12,2.59,25.572,M,34.179,M,1.3,0000*50");
            Console.WriteLine("Time {0}:{1}:{2}", gga.Time.Hour.ToString(), gga.Time.Minute.ToString(), gga.Time.Second.ToString());
            Console.WriteLine("Latitude {0}", gga.Latitude.Value.ToString());
            Console.WriteLine("Longitude {0}", gga.Longitude.Value.ToString());
        }
    }
}
