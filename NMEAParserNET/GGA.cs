using System;
using CoordinateNET;

namespace NMEAParserNET
{
    public class GGA : Sentence
    {
        public GGATime Time { get; set; }


        public class GGATime
        {
            public uint Hour { get; set; }
            public uint Minute { get; set; }
            public uint Second { get; set; }
        }
        public Latitude Latitude { get; set; }
        public Longitude Longitude { get; set; }

        public enum TypeOfSPS
        {
            NotSpecific = 0, SPS = 1, DGPS = 2
        }
        public uint NumberOfSatellites { get; set; }
        /// <summary>
        /// Horizontal accuracy reduction rate
        /// </summary>
        public double HARR { get; set; }
        public double AntennaHeight { get; set; }
        public double GeoidHeight { get; set; }

        public override string Message { get { return "GGA"; } }

        public override string AAA => throw new NotImplementedException();
    }
}
