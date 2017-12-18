using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xlDriverDIO
{
    public abstract class xlDriverDioBase
    {
        public String FilePath;

        public abstract void Init();
        public abstract void Release();
        public abstract void Write(Int32 PORTx, Int32 Pin, Boolean isHigh);
        public abstract void Write(Int32 PORTx, Int32 portValue);
        public abstract Int32 Read(Int32 PORTx);
        public abstract Boolean Read(Int32 PORTx, Int32 Pin);
        public abstract void SetDir(Int32 PORTxDir, Int32 Output);
        public abstract void Reset(ushort val);
    }
}
