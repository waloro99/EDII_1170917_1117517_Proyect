using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pruebas.Code
{
    public class Sdes
    {
        //
        //------------------------------------------------------------Consts------------------------------------------------------------
        //

        private readonly int[] P10;
        private readonly int[] P8;
        private readonly int[] P4;
        private readonly int[] EP;
        private readonly int[] IP;
        private readonly int[] invIP;




        private readonly bool[,,] Sbox1 = { { { false, true  }, { false, false }, { true , true  }, { true , false } },
                                            { { true , true  }, { true , false }, { false, true  }, { false, false } },
                                            { { false, false }, { true , false }, { false, true  }, { true , true  } },
                                            { { true , true  }, { false, true  }, { true , true  }, { true , false } } };

        private readonly bool[,,] Sbox2 = { { { false, false }, { false, true  }, { true , false }, { true , true  } },
                                            { { true , false }, { false, false }, { false, true  }, { true , true  } },
                                            { { true , true  }, { false, false }, { false, true  }, { false, false } },
                                            { { true , false }, { false, true  }, { false, false }, { true , true  } } };

        //
        //------------------------------------------------------------------------------------------------------------------------------------



        //
        //---------------------------------------------------------------Builder---------------------------------------------------------------
        //

        public Sdes(string perPath)
        {
            string[] Permutations = File.ReadAllLines(perPath);

            P10 = Permutations[0].Split(',').Select(int.Parse).ToArray();
            P8 = Permutations[1].Split(',').Select(int.Parse).ToArray();
            P4 = Permutations[2].Split(',').Select(int.Parse).ToArray();
            EP = Permutations[3].Split(',').Select(int.Parse).ToArray();
            IP = Permutations[4].Split(',').Select(int.Parse).ToArray();

            invIP = new int[8];
            for (int i = 0; i < 8; i++) invIP[i] = Array.IndexOf(IP, i);

        }

        //
        //------------------------------------------------------------------------------------------------------------------------------------




        //
        //------------------------------------------------------------Public Funcs------------------------------------------------------------
        //

        public void Encode(string rPath, string wPath, int key)
        {
            if (key < 0 || key > 1023) return;

            bool[] k1 = new bool[8];
            bool[] k2 = new bool[8];
            bool[] BinaryKey = GetBinaryArray(key, 10);
            GenerateKeys(BinaryKey, ref k1, ref k2);


            using (FileStream Rfile = new FileStream(rPath, FileMode.Open))
            using (BinaryReader BR = new BinaryReader(Rfile))
            using (FileStream Wfile = new FileStream(wPath, FileMode.Create))
            using (BinaryWriter BW = new BinaryWriter(Wfile))
            {
                Encode(BR, BW, k1, k2);
            }
        }

        public void Decoder(string rPath, string wPath, int key)
        {
            if (key < 0 || key > 1023)
                return;
            bool[] k1 = new bool[8];
            bool[] k2 = new bool[8];
            bool[] BinaryKey = GetBinaryArray(key, 10);
            GenerateKeys(BinaryKey, ref k1, ref k2);
            using (FileStream Rfile = new FileStream(rPath, FileMode.Open))
            using (BinaryReader BR = new BinaryReader(Rfile))
            using (FileStream Wfile = new FileStream(wPath, FileMode.Create))
            using (BinaryWriter BW = new BinaryWriter(Wfile))
            {
                Uncode(BR, BW, k1, k2);
            }
        }

        //
        //------------------------------------------------------------------------------------------------------------------------------------


        //
        //------------------------------------------------------------Private Funcs------------------------------------------------------------
        //
        private void Uncode(BinaryReader br, BinaryWriter Bw, bool[] k1, bool[] k2)
        {
            byte[] Buffer = new byte[1024];
            List<byte> WriteBuffer = new List<byte>();
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                Buffer = br.ReadBytes(1024);
                WriteBuffer = new List<byte>();
                foreach (var item in Buffer)
                {
                    bool[] TempArray = GetIP(GetBinaryArray(item, 8));
                    TempArray = EncodeBit(TempArray, k2);
                    TempArray = Swap(TempArray);
                    TempArray = EncodeBit(TempArray, k1);
                    TempArray = GetinvIP(TempArray);
                    WriteBuffer.Add((byte)GetInt(TempArray));
                }
                Bw.Write(WriteBuffer.ToArray());
            }
        }

        private void GenerateKeys(bool[] binaryKey, ref bool[] k1, ref bool[] k2)
        {
            bool[] P10BinaryKey = GetP10(binaryKey);

            bool[] FirstHalfShifted = LeftShift1(P10BinaryKey.Take(5).ToArray());
            bool[] SecondHalfShifted = LeftShift1(P10BinaryKey.Skip(5).ToArray());

            k1 = GetP8(MergeArrays(FirstHalfShifted, SecondHalfShifted));

            FirstHalfShifted = LeftShift2(FirstHalfShifted);
            SecondHalfShifted = LeftShift2(SecondHalfShifted);

            k2 = GetP8(MergeArrays(FirstHalfShifted, SecondHalfShifted));
        }


        private void Encode(BinaryReader br, BinaryWriter Bw, bool[] k1, bool[] k2)
        {
            byte[] Buffer = new byte[1024];
            List<byte> WriteBuffer = new List<byte>();

            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                Buffer = br.ReadBytes(1024);
                WriteBuffer = new List<byte>();

                foreach (var item in Buffer)
                {
                    bool[] TempArray = GetIP(GetBinaryArray(item, 8));
                    TempArray = EncodeBit(TempArray, k1);
                    TempArray = Swap(TempArray);
                    TempArray = EncodeBit(TempArray, k2);
                    TempArray = GetinvIP(TempArray);

                    WriteBuffer.Add((byte)GetInt(TempArray));
                }

                Bw.Write(WriteBuffer.ToArray());
            }

        }


        //
        //------------------------------------------------------------------------------------------------------------------------------------



        //
        //------------------------------------------------------------Permutations------------------------------------------------------------
        //

        private bool[] GetP10(bool[] Array)
        {
            return new bool[] { Array[P10[0]], Array[P10[1]], Array[P10[2]], Array[P10[3]], Array[P10[4]],
                                Array[P10[5]], Array[P10[6]], Array[P10[7]], Array[P10[8]], Array[P10[9]] };
        }

        private bool[] GetP8(bool[] Array)
        {
            return new bool[] { Array[P8[0]], Array[P8[1]], Array[P8[2]], Array[P8[3]],
                                Array[P8[4]], Array[P8[5]], Array[P8[6]], Array[P8[7]] };
        }

        private bool[] GetP4(bool[] Array)
        {
            return new bool[] { Array[P4[0]], Array[P4[1]], Array[P4[2]], Array[P4[3]] };
        }

        private bool[] GetEP(bool[] Array)
        {
            return new bool[] { Array[EP[0]], Array[EP[1]], Array[EP[2]], Array[EP[3]],
                                Array[EP[4]], Array[EP[5]], Array[EP[6]], Array[EP[7]] };
        }

        private bool[] GetIP(bool[] Array)
        {
            return new bool[] { Array[IP[0]], Array[IP[1]], Array[IP[2]], Array[IP[3]],
                                Array[IP[4]], Array[IP[5]], Array[IP[6]], Array[IP[7]] };
        }

        private bool[] GetinvIP(bool[] Arr)
        {
            return new bool[] { Arr[invIP[0]], Arr[invIP[1]], Arr[invIP[2]], Arr[invIP[3]],
                                Arr[invIP[4]], Arr[invIP[5]], Arr[invIP[6]], Arr[invIP[7]] };
        }


        //
        //------------------------------------------------------------------------------------------------------------------------------------



        //
        //------------------------------------------------------------Shifts------------------------------------------------------------------
        //

        private bool[] LeftShift1(bool[] Arr)
        {
            return new bool[] { Arr[1], Arr[2], Arr[3], Arr[4], Arr[0] };
        }

        private bool[] LeftShift2(bool[] Arr)
        {
            return new bool[] { Arr[2], Arr[3], Arr[4], Arr[0], Arr[1] };
        }

        //
        //------------------------------------------------------------------------------------------------------------------------------------



        //
        //------------------------------------------------------------Sboxes------------------------------------------------------------------
        //

        private bool[] GetSbox(bool[] Arr, bool[,,] Sbox)
        {
            int Fl = GetInt(new bool[] { Arr[0], Arr[3] });
            int Cl = GetInt(new bool[] { Arr[1], Arr[2] });
            return new bool[] { Sbox[Fl, Cl, 0], Sbox[Fl, Cl, 1] };
        }


        //
        //------------------------------------------------------------------------------------------------------------------------------------

        //
        //------------------------------------------------------------Others------------------------------------------------------------------
        //

        private bool[] MergeArrays(bool[] Arr1, bool[] Arr2)
        {
            List<bool> rtrnlst = new List<bool>();
            foreach (var item in Arr1) rtrnlst.Add(item);
            foreach (var item in Arr2) rtrnlst.Add(item);
            return rtrnlst.ToArray();
        }

        private bool[] GetBinaryArray(int key, int Size)
        {
            bool[] rtrnArray = new bool[10];
            string binaryArray = Convert.ToString(key, 2);
            binaryArray = binaryArray.PadLeft(Size, '0');

            for (int i = 0; i < Size; i++)
            {
                if (binaryArray[i] == '1') rtrnArray[i] = true;
                else rtrnArray[i] = false;
            }
            return rtrnArray;
        }

        private int GetInt(bool[] BoolArray)
        {
            string strg = "";

            foreach (var item in BoolArray)
            {
                if (item) strg += "1";
                else strg += "0";
            }

            return Convert.ToInt32(strg, 2);
        }

        private bool[] EncodeBit(bool[] Binarybyte, bool[] key)
        {
            bool[] FirstHalf = Binarybyte.Take(4).ToArray();
            bool[] SecondHalf = Binarybyte.Skip(4).ToArray();

            bool[] ExpHalf = GetEP(SecondHalf.ToArray());

            ExpHalf = XOR(ExpHalf, key);

            bool[] FirstHalf_ExpHalf = ExpHalf.Take(4).ToArray();
            bool[] SecondHalf_ExpHalf = ExpHalf.Skip(4).ToArray();

            bool[] FirstSbox = GetSbox(FirstHalf_ExpHalf, Sbox1);
            bool[] SecondSbox = GetSbox(SecondHalf_ExpHalf, Sbox2);


            bool[] MergedSbox = GetP4(MergeArrays(FirstSbox, SecondSbox));

            FirstHalf = XOR(FirstHalf, MergedSbox);

            return MergeArrays(FirstHalf, SecondHalf);
        }

        private bool[] XOR(bool[] Arr1, bool[] Arr2)
        {
            if (Arr1.Length == 4) return new bool[] { (Arr1[0] ^ Arr2[0]), (Arr1[1] ^ Arr2[1]), (Arr1[2] ^ Arr2[2]), (Arr1[3] ^ Arr2[3]), };
            else
            {
                return new bool[] { (Arr1[0] ^ Arr2[0]), (Arr1[1] ^ Arr2[1]), (Arr1[2] ^ Arr2[2]), (Arr1[3] ^ Arr2[3]),
                                (Arr1[4] ^ Arr2[4]), (Arr1[5] ^ Arr2[5]), (Arr1[6] ^ Arr2[6]), (Arr1[7] ^ Arr2[7]) };
            }
        }

        private bool[] Swap(bool[] Array)
        {
            return MergeArrays(Array.Skip(4).ToArray(), Array.Take(4).ToArray());
        }

        //
        //------------------------------------------------------------------------------------------------------------------------------------



    }
}
