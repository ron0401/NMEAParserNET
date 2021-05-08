using System;
using CoordinateNET;

namespace NMEAParserNET
{
    public class GGA : Sentence
    {
        public GGA(string sentence) : base(sentence)
        {

        }

        public GGA()
        {
            this._SentenceString = GetBlankCsvString(14);
            _SentenceString += "*00";
            _SentenceString = AddHeadTail(_SentenceString);
            UpdateSentenceBlock(0, "GP" + this.MessageStr);
        }
        public override TypeOfMessage Message
        {
            get
            {
                return TypeOfMessage.GGA;
            }
        }

        public override string MessageStr { get { return "GGA"; } }
        public GGATime Time 
        {
            get
            {
                string str = this.getBlockData(1).Replace(".","").PadRight(9,'0');

                return new GGATime()
                {
                    Hour = uint.Parse(str.Substring(0,2)),
                    Minute = uint.Parse(str.Substring(2,2)),
                    Second = uint.Parse(str.Substring(4,2)),
                    MilliSecond = uint.Parse(str.Substring(6,3))
                };
            }
            set 
            {
                UpdateSentenceBlock(1,
                    value.Hour.ToString("D2") +
                    value.Minute.ToString("D2") +
                    value.Second.ToString("D2") + "." +
                    (value.MilliSecond).ToString("D3")
                    );
            }
        }


        public class GGATime
        {
            public uint Hour { get; set; } = 0;
            public uint Minute { get; set; } = 0;
            public uint Second { get; set; } = 0;
            public uint MilliSecond { get; set; } = 0;
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
            set
            {
                switch (value.LatitudeType)
                {
                    case Latitude.TypeOfLatitude.North:
                        UpdateSentenceBlock(3, "N");
                        break;
                    case Latitude.TypeOfLatitude.South:
                        UpdateSentenceBlock(3, "S");
                        break;
                    default:
                        throw new Exception();
                }
                double v = DcimalTomin(value.Value) * 100;
                UpdateSentenceBlock(2, v.ToString("F4"));
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
        private double DcimalTomin(double val)
        {
            val = val / 100;
            double d = (double)((int)val);
            val = val - d;
            val = val * 0.6;
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
            set
            {
                switch (value.LongitudeType)
                {
                    case Longitude.TypeOfLongitude.East:
                        UpdateSentenceBlock(5, "E");
                        break;
                    case Longitude.TypeOfLongitude.West:
                        UpdateSentenceBlock(5, "W");
                        break;
                    default:
                        throw new Exception();
                }
                double v = DcimalTomin(value.Value) * 100;
                UpdateSentenceBlock(4, v.ToString("F4"));
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
            set
            {
                UpdateSentenceBlock(6,value.ToString());
            }
        }
        public uint NumberOfSatellites 
        { 
            get 
            {
                return uint.Parse(this.getBlockData(7));
            }
            set
            {
                UpdateSentenceBlock(7, value.ToString("D2"));
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
            set
            {
                UpdateSentenceBlock(8, value.ToString("F1"));
            }
        }
        public double AntennaHeight 
        {
            get 
            {
                return double.Parse(this.getBlockData(9));
            }
            set
            {
                UpdateSentenceBlock(9, value.ToString("F1"));
            }
        }
        public double GeoidHeight
        {
            get
            {
                return double.Parse(this.getBlockData(11));
            }
            set
            {
                UpdateSentenceBlock(11, value.ToString("F1"));
            }
        }
        public string DRPID 
        {
            get 
            {
                return this.getBlockData(14);
            }
            set 
            {
                UpdateSentenceBlock(14, value);
            }
        }
    }
}
