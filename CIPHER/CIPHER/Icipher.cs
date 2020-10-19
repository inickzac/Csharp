using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPHER
{
    interface ICipher
    {

        event Action<long[]> ProgressChange;
        void CipherFile(string loadFilePath, string saveFileDial, int key);
        event Action<string> secOriginalByte;
        event Action<string> secChiperByte;
        event Action<string> secKeyBite;
    }
}
