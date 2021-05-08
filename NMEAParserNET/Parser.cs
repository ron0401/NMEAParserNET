using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEAParserNET
{
    public static class Parser
    {
        public static NMEA.Sentence.TypeOfMessage JudgeMsgType(string sentence)
        {
            NMEA.Sentence.TypeOfMessage t = NMEA.Sentence.TypeOfMessage.Unknown;
            sentence = NMEA.Sentence.AddHeadTail(sentence);
            try
            {
                string r = sentence.Substring(3, 3);
                t = NMEA.Sentence.SentenceDictionary[r];
                //NMEA.Sentence.SentenceDictionary.TryGetValue(sentence.Substring(3,3), out t);
            }
            catch (Exception)
            {
                return NMEA.Sentence.TypeOfMessage.Unknown;
            }
            return t;
        }
        enum stateType 
        {
            NoDollar,HavingDoller,WaitLF
        }
        public static string PurgeWithinNMEA(string FilePath) 
        {
            stateType state = stateType.NoDollar;
            var stringList = new List<string>();
            var charList = new List<byte>();
            var sr = new StreamReader(FilePath,System.Text.Encoding.ASCII);
            int c = sr.Read();
            for (; c != -1; c = sr.Read())
            {
                switch (state)
                {
                    case stateType.NoDollar:
                        if (c == (int)'$')
                        {
                            state = stateType.HavingDoller;
                            charList.Clear();
                            charList.Add((byte)c);
                        }
                        break;
                    case stateType.HavingDoller:
                        switch (c)
                        {
                            case (int)'$':
                                charList.Clear();
                                state = stateType.NoDollar;
                                break;
                            case (int)'\r':
                                state = stateType.WaitLF;
                                break;
                            default:
                                charList.Add((byte)c);
                                break;
                        }
                        break;
                    case stateType.WaitLF:
                        if ( c == (int)'\n' && charList.Count >= 7)
                        {
                            //string s = System.Text.Encoding.ASCII.GetString(charList.ToArray());
                            if (charList[6] == (byte)',')
                            {
                                stringList.Add(System.Text.Encoding.ASCII.GetString(charList.ToArray()) + "\r\n");
                            }
                        }
                        charList.Clear();
                        state = stateType.NoDollar;
                        break;
                    default:
                        break;
                }
            }
            if (charList.Count >= 7)
            {
                //string s = System.Text.Encoding.ASCII.GetString(charList.ToArray());
                if (charList[6] == (byte)',' && charList[0] == (byte)'$')
                {
                    stringList.Add(System.Text.Encoding.ASCII.GetString(charList.ToArray()) + "\r\n");
                }
            }
            return string.Join("",stringList.ToArray());
        }
    }
}
