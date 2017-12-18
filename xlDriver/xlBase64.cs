using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xlTools
{
    public class xlBase64
    {
        public static String Encoded(Byte[]  bytes)
        {
            String base64String = Convert.ToBase64String(bytes);
            return base64String;
        }

        public static String Encoded(String orgStr)
        {
            byte[] bytes = Encoding.Default.GetBytes(orgStr);
            return Convert.ToBase64String(bytes);
        }

        public static Byte[] Decoded(String base64String)
        {
            Byte[] outputb = Convert.FromBase64String(base64String);
            //string orgStr = Encoding.Default.GetString(outputb);
            return outputb;
        }

        public static String DecodedString(String base64String)
        {
            byte[] outputb = Convert.FromBase64String(base64String);
            string orgStr = Encoding.Default.GetString(outputb);
            return orgStr;
        }

        
    }
}
