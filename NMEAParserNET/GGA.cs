using System;
using CoordinateNET;

namespace NMEAParserNET
{
    public class GGA : Sentence
    {
        public GGA(string sentence) : base(sentence)
        {

        }

        public GGATime Time 
        {
            get
            {
                return new GGATime()
                {
                    Hour = uint.Parse(this.getBlockData(1).Substring(0,2)),
                    Minute = uint.Parse(this.getBlockData(1).Substring(2,2)),
                    Second = uint.Parse(this.getBlockData(1).Substring(4,2)),
                    MilliSecond = uint.Parse(this.getBlockData(1).Substring(7,2)) * 10
                };
            }    
        }
        internal override bool checkSentence() 
        {
            return (this.Message == "GGA");
        }


        public class GGATime
        {
            public uint Hour { get; set; }
            public uint Minute { get; set; }
            public uint Second { get; set; }
            public uint MilliSecond { get; set; }
        }
        public Latitude Latitude
        {
            get
            {
                var lat = new Latitude();
                if (this.getBlockData(3) == "N")
                {
                    lat.LatitudeType = Latitude.TypeOfLatitude.North;
                }
                else if (this.getBlockData(3) == "S")
                {
                    lat.LatitudeType = Latitude.TypeOfLatitude.South;
                }
                else
                {
                    return null;
                }
                try
                {
                    lat.Value = minToDecimal(double.Parse(this.getBlockData(2)));
                }
                catch (Exception)
                {
                    return null;
                }
                return lat;
            }

        }
        private double minToDecimal(double val) 
        {
            val = val / 100;
            double d = (double)((int)val);
            val = val - d;
            val = val / 0.6;
            return d + val;
        }
        public Longitude Longitude {
            get 
            {
                var lon = new Longitude();
                switch (this.getBlockData(5))
                {
                    case "E":
                        lon.LongitudeType = Longitude.TypeOfLongitude.East;
                        break;
                    case "W":
                        lon.LongitudeType = Longitude.TypeOfLongitude.West;
                        break;
                    default:
                        return null;
                }
                try
                {
                    lon.Value = minToDecimal(double.Parse(this.getBlockData(4)));
                }
                catch (Exception)
                {
                    return null;
                }
                return lon;
            }
        }

        public enum TypeOfQuality
        {
            NotSpecific = 0, GPS = 1, DGPS = 2, GPSPPS = 3, RTKFix = 4, FloatRTK = 5, DR = 6 
        }
        public TypeOfQuality Quality
        {
            get
            {
                return (TypeOfQuality)int.Parse(this.getBlockData(6));
            }
        }
        public uint NumberOfSatellites 
        { 
            get 
            {
                return uint.Parse(this.getBlockData(7));
            }
        }
        /// <summary>
        /// Horizontal accuracy reduction rate
        /// </summary>
        public double HARR 
        {
            get
            {
                return double.Parse(this.getBlockData(8));
            }
            
        }
        public double AntennaHeight 
        {
            get 
            {
                return double.Parse(this.getBlockData(9));
            }     
        }
        public double GeoidHeight
        {
            get
            {
                return double.Parse(this.getBlockData(11));
            }
        }
    }
}
