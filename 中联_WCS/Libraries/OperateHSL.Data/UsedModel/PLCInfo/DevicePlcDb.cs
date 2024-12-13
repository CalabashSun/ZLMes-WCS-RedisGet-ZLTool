using OperateHSL.Data.DataModel.PLCModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.UsedModel.PLCInfo
{
    /// <summary>
    /// plc设备db块
    /// </summary>
    public class DevicePlcDb
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
        /// 该plc该数据块对应的设备
        /// </summary>
        public List<DeviceInfo> plc_devices { get; set; }
    }
    /// <summary>
    /// 设备信息
    /// </summary>
    public class DeviceInfo
    {
        public string device_name { get; set; }

        public string device_code { get; set; }

        public List<DeviceDb> device_dbs { get; set; } 
    }
    /// <summary>
    /// 设备地址数据
    /// </summary>
    public class DeviceDb
    {
        public string data_code { get; set; }

        public string data_length { get; set; }

        public string data_name { get; set; }

        public decimal data_offset { get; set; }

        public string data_type { get; set; }

        public string is_use { get; set; }

        public string operate_type { get; set; }

        public string data_result { get; set; }
    }
}
