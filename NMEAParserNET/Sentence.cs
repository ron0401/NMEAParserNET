using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEAParserNET
{

    /// <summary>
    /// NMEA Sentence Super Class.
    /// </summary>
    public abstract class Sentence
    {
        //public Sentence(string sentense) 
        //{

        //}
        public abstract string Message { get; }
        private string _SentenceString;
        public string SentenceString
        {
            get
            {
                return _SentenceString;
            }
            set
            {
                if (value.Length < 6)
                {
                    throw new Exception();
                }
                if (value.Substring(value.Length - 2) != "\r\n" || value.Substring(0, 1) != "$" || value.Substring(value.Length - 5).Substring(0,2) != "*")
                {
                    throw new Exception();
                }
                _SentenceString = value;
            }
        }
        private string getDataString() 
        {
            int end = _SentenceString.IndexOf('*');
            int start = _SentenceString.IndexOf('$');
            return _SentenceString.Substring(start + 1, end - start - 1);
        }
        private string getCheckSum(string str)
        {
            throw new Exception();
        }
        public Byte CheckSum 
        {
            get 
            {
                
                throw new Exception();
            } 
        }

        //public static byte getNEMAcheckSum(String data, int startPos, int endPos)
        //{
        //    byte checkSum = 0;
        //    for (int i = startPos; i < endPos; i++)
        //    {
        //        checkSum ^= (byte)data.charAt(i);
        //    }
        //    return checkSum;
        //}
    }
}
