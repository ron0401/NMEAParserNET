using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NMEAParserNET.NMEA
{

    /// <summary>
    /// NMEA Sentence Super Class.
    /// </summary>
    public abstract class Sentence
    {
        public Sentence() { }
        public Sentence(string sentense)
        {
            sentense = AddHeadTail(sentense);
            this._SentenceString = sentense;
        }

        internal static string AddHeadTail(string str) 
        {
            if (str.Substring(str.Length - 2) != "\r\n")
            {
                str += "\r\n";
            }
            if (str.Substring(0, 1) != "$")
            {
                str = "$" + str;
            }
            return str;
        }

        public enum TypeOfMessage 
        {
            GGA,Unknown
        }
        internal static Dictionary<string, TypeOfMessage> SentenceDictionary = new Dictionary<string, TypeOfMessage>()
        {
            {"GGA",TypeOfMessage.GGA }
        };

        internal void UpdateSentenceBlock(int index,string str) 
        {
            string[] sen = _SentenceString.Split("*");

            var ary = sen[0].Split(",");
            ary[index] = str;
            sen[0] = string.Join(",", ary);
            _SentenceString = string.Join("*", sen);
        }

        internal string GetBlankCsvString(int num)
        {
            string[] str = new string[num];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = "";
            }
            return string.Join(",", str);
        }

        public string Talker 
        {
            get 
            {
                return getBlockData(0).Replace("$", "").Substring(0, 2);
            }
            set
            {
                if (value.Length != 2)
                {
                    throw new Exception();
                }
                UpdateSentenceBlock(0, "$" + value + this.MessageStr);
                UpdateCheckSum();
            }
        }

        public abstract TypeOfMessage Message { get; }
        public abstract string MessageStr { get; }

        internal string _SentenceString;
        public string SentenceString
        {
            get
            {
                return _SentenceString;
            }
            //set
            //{
            //    if (value.Length < 6)
            //    {
            //        throw new Exception();
            //    }
            //    if (value.Substring(value.Length - 2) != "\r\n" || value.Substring(0, 1) != "$" || value.Substring(value.Length - 5).Substring(0,1) != "*")
            //    {
            //        throw new Exception();
            //    }
            //    _SentenceString = value;
            //}
        }

        internal string getBlockData(uint n)
        {
            var str = getDataString();
            string[] ary = str.Split(",");
            return ary[n];
        }

        internal string getDataString() 
        {
            int end = _SentenceString.IndexOf('*');
            int start = _SentenceString.IndexOf('$');
            return _SentenceString.Substring(start + 1, end - start - 1);
        }
        private string getCheckSum(string str)
        {
            throw new Exception();
        }
        public class CheckSum 
        {
            public Byte Value { get; set; }
            public static byte CalcCheckSum(string sentence) 
            {
                string str = sentence.Split("*")[0].Replace("$","");
                byte[] data = Encoding.ASCII.GetBytes(str);
                byte b = 0;
                foreach (var f in data)
                {
                    b =(byte)(((int)b) ^ f);
                }
                return b;
            }
        }
        internal void UpdateCheckSum() 
        {
            string sum = BitConverter.ToString(new byte[] { CheckSum.CalcCheckSum(this._SentenceString) });
            string str = this._SentenceString.Split("*")[0];
            str = str + "*" + sum + "\r\n";
            _SentenceString = str;
        }
        public CheckSum _CheckSum 
        {
            get 
            {
                try
                {
                    string str = _SentenceString.Split("*")[1];
                    str = str.Replace("\r\n", "");
                    if (str == "")
                    {
                        return null;
                    }
                    return new CheckSum() { Value = Convert.ToByte(str.Substring(0, 2), 16) };
                }
                catch (Exception)
                {
                    return null;
                }

            } 
        }
    }
}
