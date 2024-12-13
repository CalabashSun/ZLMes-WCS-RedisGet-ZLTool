using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperateHSL.Core.Tool
{
    public class BitConvert
    {


        public static byte[] HighLowExchange16(UInt16 value)
        {
            byte[] bytesResult = new byte[2];
            byte[] temp = new byte[2];

            BitConverter.GetBytes(Convert.ToUInt16(value)).CopyTo(temp, 0);

            bytesResult[0] = temp[1];
            bytesResult[1] = temp[0];

            return bytesResult;
        }


        public static byte[] HighLowExchange32(Single value)
        {
            byte[] bytesResult = new byte[4];
            byte[] temp = new byte[4];

            BitConverter.GetBytes(Convert.ToSingle(value)).CopyTo(temp, 0);

            bytesResult[0] = temp[3];
            bytesResult[1] = temp[2];
            bytesResult[2] = temp[1];
            bytesResult[3] = temp[0];

            return bytesResult;
        }


        public static string StrHex2Str(string strHex)
        {
            byte[] keyWord = new byte[strHex.Length / 2];
            string strResult = "";
            for (int i = 0; i < keyWord.Length; i++)
            {
                try
                {
                    keyWord[i] = (byte)(0xff & Convert.ToInt32(strHex.Substring(i * 2, 2), 16));
                    strResult += Convert.ToChar(keyWord[i]).ToString();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return strResult;
        }


        public static string Str2HexStr(string strSrc)
        {
            char[] chTemp = strSrc.ToCharArray();
            string strResult = "";
            foreach (var ch in chTemp)
            {
                int value = Convert.ToInt32(ch);
                strResult += String.Format("{0:X}", value);
            }
            return strResult;
        }


        public static ushort ToUInt16(byte[] value)
        {
            return (ushort)((value[0] << 8) | (value[1] & 0x00FF));
        }

        /// <summary>
        /// 指定偏移量的byte转short
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static ushort ToUInt16(byte[] value, int offset)
        {
            return (ushort)((value[offset] << 8) | (value[offset + 1] & 0x00FF));
        }



        public static int ToUInt32(byte[] value)
        {
            return ((value[0] << 24) | (value[1] << 16) | (value[2] << 8) | (value[3] & 0xFF));
        }

        /// <summary>
        /// 指定偏移量的byte转 int 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int ToUInt32(byte[] value, int offset)
        {
            return ((value[0] << 24) | (value[offset + 1] << 16) | (value[offset + 2] << 8) | (value[offset + 3] & 0xFF));
        }


        public static Int32 ToInt32(byte[] value, int offset)
        {
            byte[] values = new byte[] { value[offset + 3], value[offset + 2], value[offset + 1], value[offset] };
            return BitConverter.ToInt32(values, 0);
        }

        public static float ToSingle(byte[] value, int offset)
        {
            byte[] values = new byte[] { value[offset + 3], value[offset + 2], value[offset + 1], value[offset] };
            return BitConverter.ToSingle(values, 0);
        }

        public static bool[] ToBoolArray(byte b)
        {
            bool[] array = new bool[8];
            for (int i = 0; i <= 7; i++)
            { //对于byte的每bit进行判定
                array[i] = (b & 1) == 1;   //判定byte的最后一位是否为1，若为1，则是true；否则是false
                b = (byte)(b >> 1);       //将byte右移一位
            }
            return array;
        }

        /// <summary>
        /// byte[]转string
        /// </summary>
        /// <param name="srcs"></param>
        /// <returns></returns>
        public static string ExtBytesToStr(byte[] srcs)
        {
            string result = "";
            if (srcs != null)
            {
                int elemCount = srcs.Count();
                if (elemCount > 0)
                {
                    for (int i = 0; i < elemCount; i++)
                    {
                        bool bIsUpperCase = false;
                        bool bIsLowerCase = false;
                        bool bIsDot = false;
                        bool bIsNumber = false;
                        int temp = srcs[i];
                        bIsUpperCase = (srcs[i] >= 65 && srcs[i] <= 90);
                        bIsLowerCase = (srcs[i] >= 97 && srcs[i] <= 122);
                        bIsDot = (srcs[i] == 46);
                        bIsNumber = (srcs[i] >= 48 && srcs[i] <= 57);
                        bool bIsOK = (bIsUpperCase || bIsLowerCase || bIsDot || bIsNumber);
                        if (!bIsOK)
                        {
                            srcs[i] = 0;
                        }
                    }
                    result = Encoding.UTF8.GetString(srcs, 0, elemCount);
                }
            }
            return result;
        }

        /// <summary>
        /// byte[]转string
        /// </summary>
        /// <param name="srcs"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ExtBytesToStr(byte[] srcs, int startIndex, int length)
        {
            string result = "";
            if (srcs != null)
            {
                if (length > 0)
                {
                    for (int i = startIndex; i < startIndex + length; i++)
                    {
                        bool bIsUpperCase = false;
                        bool bIsLowerCase = false;
                        bool bIsDot = false;
                        bool bIsNumber = false;
                        int temp = srcs[i];
                        bIsUpperCase = (srcs[i] >= 65 && srcs[i] <= 90);
                        bIsLowerCase = (srcs[i] >= 97 && srcs[i] <= 122);
                        bIsDot = (srcs[i] == 46);
                        bIsNumber = (srcs[i] >= 48 && srcs[i] <= 57);
                        bool bIsOK = (bIsUpperCase || bIsLowerCase || bIsDot || bIsNumber);
                        if (!bIsOK)
                        {
                            srcs[i] = 0;
                        }
                    }
                    result = Encoding.UTF8.GetString(srcs, startIndex, length);
                }
            }
            if (!string.IsNullOrEmpty(result))
            {
                result = result.Replace("\b", "").Replace("\0", "").Replace("\u0000", "").Trim();
            }
            return result;
        }


        public static bool[] getBooleanArray(byte b)
        {
            bool[] array = new bool[8];
            for (int i = 0; i <= 7; i++)
            { array[i] = (b & 1) == 1; b = (byte)(b >> 1); }
            return array;
        }
    }
}
