using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace CIPHER
{
    public class RC4 : ICipher
    {
        public event Action<long[]> ProgressChange;
        public event Action<string> secKeyBite;

        readonly byte[] S = new byte[256];

        int x = 0;
        int y = 0;

        public RC4(byte[] key)
        {
            init(key);
        }

        // Key-Scheduling Algorithm 
        // Алгоритм ключевого расписания 
        private void init(byte[] key)
        {
            int keyLength = key.Length;

            for (int i = 0; i < 256; i++)
            {
                S[i] = (byte)i;
            }

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + key[i % keyLength]) % 256;
                S.Swap(i, j);
            }
        }

        byte[] Encode(byte[] dataB, int size)
        {
            byte[] data = dataB.Take(size).ToArray();

            byte[] cipher = new byte[data.Length];

            for (int m = 0; m < data.Length; m++)
            {
                cipher[m] = (byte)(data[m] ^ keyItem());
            }

            return cipher;
        }
       byte[] Decode(byte[] dataB, int size)
        {
            return Encode(dataB, size);
        }

        // Pseudo-Random Generation Algorithm 
        // Генератор псевдослучайной последовательности 
     public  void  CipherFile(string loadFilePath, string saveFileDial, int key)
        {
            FileStream saveFileStream = null;
            FileStream openFileStream = new FileStream(loadFilePath, FileMode.Open, FileAccess.Read, FileShare.None, 100000);
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

            { saveFileStream = new FileStream(saveFileDial + extension + ".crypt", FileMode.Create, FileAccess.Write); }
            ProgressStr prsttr = new ProgressStr
            {
                massProcess = massProcess,
                openFileStream = openFileStream
            };
            TimerCallback tm = new TimerCallback(CountTimerProgress);
            System.Threading.Timer timerPr = new Timer(tm, prsttr, 0, 20);
            BinaryWriter readerSave = new BinaryWriter(saveFileStream);
            byte[] buffer = new byte[100];
            int bufferSize = buffer.Length;
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                for(int i=0; i<bufferSize; i++)
                {
                    if (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer[i] = reader.ReadByte();
                        secOriginalByte(BinCOdeWithZeroFirst(Convert.ToString(buffer[i], 2)));                      
                    }
                    else bufferSize = i;
                }
                buffer = Encode(buffer, bufferSize);

                for (int i = 0; i < bufferSize; i++)
                {
                    readerSave.Write(buffer[i]);
                    secChiperByte(BinCOdeWithZeroFirst(Convert.ToString(buffer[i], 2)));
                }
            }
            Thread.Sleep(21);
            timerPr.Dispose();
            openFileStream.Close();
            saveFileStream.Close();
        }

        struct ProgressStr
        {
            public long[] massProcess;
            public FileStream openFileStream;
        }

        void CountTimerProgress(object obj)
        {
            ProgressStr massProcess = (ProgressStr)obj;
            massProcess.massProcess[1] = massProcess.openFileStream.Position;
            ProgressChange(massProcess.massProcess);
        }

        private byte keyItem()
        {
            x = (x + 1) % 256;
            y = (y + S[x]) % 256;

            S.Swap(x, y);
            byte key= S[(S[x] + S[y]) % 256];
            secKeyBite(BinCOdeWithZeroFirst(Convert.ToString(key, 2)));
            return key;
        }

        public event Action<string> secOriginalByte;
        public event Action<string> secChiperByte;

        public string BinCOdeWithZeroFirst(string bin)
        {
            int addCount = 8 - bin.Length;
            if (bin.Length < 8) for (int i = 0; i < addCount; i++) bin = bin.Insert(0, "0");
            return bin;
        }

    }




    static class SwapExt
    {
        public static void Swap<T>(this T[] array, int index1, int index2)
        {
            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }
    }


}
