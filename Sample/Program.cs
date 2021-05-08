using System;
using NMEAParserNET.NMEA;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var gga = new GGA("GNGGA,035817.30,3444.3418019,N,13535.1207337,E,5,12,2.59,25.572,M,34.179,M,1.3,0000*50");
            var op = new System.Text.Json.JsonSerializerOptions() { WriteIndented = true, IgnoreReadOnlyProperties = true };
            op.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(gga, op));
            gga.Talker = "GP";
            gga.AntennaHeight = 1.443;
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(gga, op));
        }
}
}
