using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Core.Tool
{
    public class AnalysisPLCData
    {
        /// <summary>
        /// 解析plc数据
        /// </summary>
        /// <param name="data_offset"></param>
        /// <param name="data_type"></param>
        /// <param name="recvs"></param>
        /// <returns></returns>
        public static string GetData(decimal data_offset,string data_type,byte[] recvs)
        {
            //获取数据偏移量整数位
            int integerPart = (int)Math.Floor(data_offset); // 获取整数部分
            var data_result = string.Empty;
            switch (data_type)
            {
                case "Bool":
                    //获取数据偏移量小数位
                    decimal decimalPart = data_offset - integerPart; // 获取小数部分
                    string decimalPartString = "";
                    if (decimalPart == (decimal)0)
                    {
                        decimalPartString = "0";
                    }
                    else
                    {
                        decimalPartString = decimalPart.ToString().Substring(2);
                    }
                    var digitPart = int.Parse(decimalPartString.Substring(0, 1));
                    data_result = BitConvert.getBooleanArray(recvs[integerPart])[digitPart].ToString();
                    break;
                case "DInt":
                    data_result = BitConvert.ToInt32(recvs, integerPart).ToString();
                    break;
                case "Int":
                    data_result = BitConvert.ToUInt16(recvs, integerPart).ToString();
                    break;
                case "String[32]":
                    data_result = BitConvert.ExtBytesToStr(recvs, integerPart, 34).ToString();
                    break;
                case "String[20]":
                    data_result = BitConvert.ExtBytesToStr(recvs, integerPart, 22).ToString();
                    break;
                case "Real":
                    data_result = BitConvert.ToSingle(recvs, integerPart).ToString();
                    break;
                case "DWord":
                    data_result = BitConvert.ToUInt32(recvs, integerPart).ToString();
                    break;
                case "Word":
                    data_result = BitConvert.ToUInt16(recvs, integerPart).ToString();
                    break;
                default:
                    break;
            }
            return data_result;
        }
    }
}
