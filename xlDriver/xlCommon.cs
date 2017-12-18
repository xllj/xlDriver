using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xlTools
{
    public class xlCommon
    {
        public static bool BIT(UInt16 X, UInt16 Y)
        {
            return ((X & Y) == Y);
        }

        public static UInt16 BIT(UInt16 X)
        {
            return (UInt16)(1 << X);
        }
        /*****************
         * 
         * 
         * bit : 所在位数
         **/
        public static bool BITx(UInt16 X, UInt16 bit)
        {
            ushort t = (UInt16)(1 << bit);
            return ((X & t) == t);
        }


    }
}
