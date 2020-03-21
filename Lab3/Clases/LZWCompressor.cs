using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab3.Clases
{
    class LZWCompressor
    {

        //
        //---------------------------------------------------Public Funcs----------------------------------------------------------------------
        //



        public void Compress
           (string rPath, string wPath)
        {
            /*Dictionary<Char[], int>*/
            var Characters = new Dictionary<string, int>();

            /*First, reads all the text to Get the possibles chars*/
            using (FileStream FSR = new FileStream(rPath, FileMode.Open))
            using (BinaryReader BR = new BinaryReader(FSR))
            {
                Characters = GetFileCharacters(BR);
            }


            /*Second, reads again while compressing*/
            using (FileStream FSR = new FileStream(rPath, FileMode.Open))
            using (BinaryReader BR = new BinaryReader(FSR))
            using (FileStream FSW = new FileStream(wPath, FileMode.Create))
            using (BinaryWriter BW = new BinaryWriter(FSW))
            {
                CompressAlgorithm(BR, BW, Characters);
            }
        }



        

        public string GetFilesMetrics
            (string Name, string Original, string Compresed)
        {
            string Metrics = Name.Replace(".txt", "") + '|';
            double RC, FC, PR;
            using (FileStream OR = new FileStream(Original, FileMode.Open))
            using (FileStream CM = new FileStream(Compresed, FileMode.Open))
            {
                RC = Math.Round(CM.Length / (double)OR.Length, 3);
                FC = Math.Round(OR.Length / (double)CM.Length, 3);
                PR = Math.Round(((1 - RC) * 100), 2);
            }
            Metrics += RC.ToString() + "|" + FC.ToString() + "|" + PR.ToString();
            return Metrics;
        }


        //
        //---------------------------------------------------------------------------------------------------------------------




        //
        //------------------------------------------------------Private Funcs-----------------------------------------------------
        //

        private Dictionary<string, int> GetFileCharacters(BinaryReader br)
        {
            string NextByte;
            List<byte> cmpByte = new List<byte>() { 0 };
            Dictionary<string, int> Characters = new Dictionary<string, int>();
            Characters.Add("An entry to inicialize the values", 0);

            //while used to read all the file and get all the characters
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                /*Its used .ReadBytes(1) instead of readbyte because its necessary to have an array in the paramether*/
                NextByte = (char)br.ReadByte() + "";
                if (!Characters.ContainsKey(NextByte)) Characters.Add(NextByte, Characters.Last().Value + 1);
            }

            Characters.Remove("An entry to inicialize the values");
            return Characters;
        }//Untested


        private void CompressAlgorithm(BinaryReader BR, BinaryWriter BW, Dictionary<string, int> Dic)
        {
            /*Writes just the original dictionay on the compressed file*/
            foreach (var item in Dic)
            {
                BW.Write(Convert.ToByte(item.Value));
                BW.Write(Encoding.Default.GetBytes("," + item.Key + ","));
            }
            BW.Write(Encoding.Default.GetBytes("EOD"));



            string newDicValue = Encoding.Default.GetString(BR.ReadBytes(1));
            while (BR.BaseStream.Position != BR.BaseStream.Length)
            {
                //While the string of bytes is still present on the dictionary, string += next char
                while (Dic.ContainsKey(newDicValue) && (BR.BaseStream.Position != BR.BaseStream.Length)) newDicValue += (char)BR.ReadByte();

                //Adds to the dictionary the new value with the new string readed
                if (!Dic.ContainsKey(newDicValue)) Dic.Add(newDicValue, Dic.Count + 1);

                /*Writes the contained bytelist*/
                int TST_CheckValToWrite = Dic[newDicValue.Remove(newDicValue.Length - 1)];
                BW.Write(GetBytesFromInt(Dic[newDicValue.Remove(newDicValue.Length - 1)]));
                //BW.Write(Encoding.Default.GetCharCount(GetBytesFromInt(Dic[newDicValue.Remove(newDicValue.Length - 1)])));
                newDicValue = Convert.ToString(newDicValue[newDicValue.Length - 1]);
            }
        }


        private byte[] GetBytesFromInt(int number)
        {
            string IntInBinary = "";
            var rtrnLst = new List<byte>();
            if (number <= 255)
            {
                rtrnLst.Add((byte)number);
            }
            else
            {
                IntInBinary = Convert.ToString(number, 2);
                int bytesToFill = IntInBinary.Length % 8;
                string Fill = "";
                for (int i = 0; i < 8 - bytesToFill; i++) Fill += 0;
                IntInBinary = Fill + IntInBinary;

                int iterations = IntInBinary.Length / 8;
                for (int i = 0; i < iterations; i++)
                {
                    string newbyte = "";
                    for (int j = 0; j < 8; j++) newbyte += IntInBinary[(i * 8) + j];
                    rtrnLst.Add(Convert.ToByte(newbyte, 2));
                }

            }
            return rtrnLst.ToArray();
        }


       

        private static Dictionary<int, string> CreateDictionary(List<byte> table)
        {
            Dictionary<int, string> diccionary = new Dictionary<int, string>();
            char[] Values = Encoding.Default.GetChars(table.ToArray());
            int counter = 1;
            int key = default;
            char value = default;


            for (int i = 0; i < Values.Count(); i++)
            {
                if (counter % 2 == 1)
                {
                    if (Values[i] != 44 || Values[i - 1] == 44 && Values[i + 1] == 44) key = Convert.ToByte(Values[i]);
                    else counter++;
                }
                else
                {
                    if (Values[i] != 44 || Values[i - 1] == 44 && Values[i + 1] == 44) value = Values[i];
                    else { diccionary.Add(key, Convert.ToString(value)); counter++; }
                }
            }

            Dictionary<int, string> TST_newDic = new Dictionary<int, string>();
            foreach (var item in diccionary) TST_newDic.Add(item.Key, item.Value);
            return diccionary;
        }


        
      

        //
        //---------------------------------------------------------------------------------------------------------------------
    }
}
