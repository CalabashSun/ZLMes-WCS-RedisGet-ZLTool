using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.UsedModel.PLCInfo
{
    public class SlicingPlcDb
    {
        /// <summary>
        /// plc ip
        /// </summary>
        public string plc_ip { get; set; }

        /// <summary>
        /// plc端口
        /// </summary>
        public string plc_port { get; set; }
        /// <summary>
        /// plc地址
        /// </summary>
        public string plc_dbadr { get; set; }
        /// <summary>
        /// 数据长度
        /// </summary>
        public string db_length { get; set; }

        /// <summary>
        /// 该plc该数据块对应的数据
        /// </summary>
        public List<DataDb> plc_dbInfos { get; set; } = new List<DataDb>();
    }

    /// <summary>
    /// 设备地址数据
    /// </summary>
    public class DataDb
    {
        public string data_code { get; set; }

        public string data_length { get; set; }

        public string data_name { get; set; }

        public decimal data_offset { get; set; }

        public string data_type { get; set; }

        public string is_use { get; set; }

        public string operate_type { get; set; }

        public string data_result { get; set; }

        public string data_model { get; set; }
    }
}
