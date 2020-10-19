using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Collections;

namespace CIPHER
{
    class LFSR : ICipher
    {
        delegate bool GetBitLFSR (ref BitArray[] initialState);
        GetBitLFSR getBitLFSR;
        public event Action<long[]> ProgressChange;
        public event Action<string> secOriginalByte;
        public event Action<string> secChiperByte;
        public event Action<string> secKeyBite;
        BitArray[] initialState;



        public  LFSR(BitArray[] initialState)
        {
            this.initialState = initialState;          

        }
   
        bool GetBitLFSR1(ref BitArray[] initialState)
      {
            BitArray cloneBa = (BitArray)initialState[0].Clone();
            bool outBit = initialState[0][28];
            bool newBit = initialState[0][1] ^ initialState[0][28];
            for (int i = 0; i < initialState[0].Length-1; i++)
                initialState[0][i+1] = cloneBa[i];

            initialState[0][0] = newBit;
            return outBit;
        }

        bool GetBitLFSR2(ref BitArray[] initialState)
        {
            BitArray cloneBa = (BitArray)initialState[1].Clone();
            bool outBit = initialState[1][36];
            bool newBit = initialState[1][36] ^ initialState[1][31] ^ initialState[1][32] ^ initialState[1][33] ^ initialState[1][34] ^ initialState[1][35];
            for (int i = 0; i < initialState[1].Length - 1; i++)
                initialState[1][i + 1] = cloneBa[i];

            initialState[1][0] = newBit;
            return outBit;
        }

        bool GetBitLFSR3(ref BitArray[] initialState)
        {
            BitArray cloneBa = (BitArray)initialState[2].Clone();
            bool outBit = initialState[2][26];
            bool newBit = initialState[2][26] ^ initialState[2][21] ^ initialState[2][24] ^ initialState[2][25];
            for (int i = 0; i < initialState[2].Length - 1; i++)
                initialState[2][i + 1] = cloneBa[i];

            initialState[2][0] = newBit;
            return outBit;
        }

      bool  getGeffeBit(ref BitArray[] initialState)
        {
            return GetBitLFSR1(ref initialState) ^ GetBitLFSR2(ref initialState) ^ GetBitLFSR3(ref initialState);
        }


        byte Encodebool(bool[] arr)
        {
           
            byte val = 0;
            foreach (bool b in arr)
            {
                val <<= 1;
                if (b) val |= 1;
            }
            return val;
        }

        public void CipherFile(string loadFilePath, string saveFileDial, int key)
        {
            if (initialState[1] == null)  getBitLFSR = GetBitLFSR1; 
            else  getBitLFSR = getGeffeBit;

            FileStream saveFileStream=null;
            FileStream openFileStream = new FileStream(loadFilePath, FileMode.Open, FileAccess.Read,FileShare.None,100000);
            BinaryReader reader = new BinaryReader(openFileStream);

            long length = new FileInfo(loadFilePath).Length;
            long[] massProcess = new long[3];
            massProcess[0] = openFileStream.Length;                         
            string extension = Path.GetExtension(loadFilePath);

            if (key == 2 && extension == ".crypt")
            {
                string filePathDecrypt = loadFilePath.Remove(loadFilePath.Length - 6);
                extension = filePathDecrypt.Substring(filePathDecrypt.LastIndexOf("."));
                saveFileStream = new FileStream(saveFileDial + extension, FileMode.Create, FileAccess.Write);
            }

            else

            { saveFileStream = new FileStream(saveFileDial + extension + ".crypt" , FileMode.Create, FileAccess.Write); }
            ProgressStr prsttr = new ProgressStr
            {
                massProcess = massProcess,
                openFileStream = openFileStream
            };
            TimerCallback tm = new TimerCallback(CountTimerProgress);
          System.Threading.Timer timerPr = new System.Threading.Timer(tm, prsttr, 0, 20);
            BinaryWriter readerSave = new BinaryWriter(saveFileStream);
            byte cipText = 0;
            byte origText;
            while (reader.BaseStream.Position != reader.BaseStream.Length)
                
            {
                origText = reader.ReadByte();
                secOriginalByte(BinCOdeWithZeroFirst(Convert.ToString(origText,2)));
                cipText = CipherByte(origText);
                secChiperByte(BinCOdeWithZeroFirst(Convert.ToString(cipText, 2)));
                readerSave.Write(cipText);                                                     
            }

            Thread.Sleep(21);
            timerPr.Dispose();
            openFileStream.Close();
            saveFileStream.Close();

        }

        void CountTimerProgress(object obj)
        {
            ProgressStr massProcess = (ProgressStr)obj;
            massProcess.massProcess[1] = massProcess.openFileStream.Position;
            ProgressChange(massProcess.massProcess);
        }

        struct ProgressStr
        {
            public long[] massProcess;
            public FileStream openFileStream;
        }

        byte CipherByte(byte plainByte)
        {

            bool[] oneByteKey = new bool[8];
            byte ciperByte=0;
            for (int j = 0; j < oneByteKey.Length; j++)
            {
                oneByteKey[j] = getBitLFSR(ref initialState);

            }
            byte keyPosledovat = Encodebool(oneByteKey);
            secKeyBite(BinCOdeWithZeroFirst(Convert.ToString(keyPosledovat, 2)));
            ciperByte = (byte)(plainByte ^ keyPosledovat);
            return ciperByte;

        }

        public string BinCOdeWithZeroFirst(string bin)
        {
            int addCount = 8 - bin.Length;
            if (bin.Length<8)  for (int i = 0; i < addCount; i++) bin = bin.Insert(0, "0");
            return bin;           
        }


    }
}
