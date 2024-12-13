using OperateHSL.Services.PlcInfoService;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Services.FIDInfoService
{
    public class FIDConntecionManager
    {
        private static FIDConntecionManager instance;

        private static volatile List<JWIpReader> jwReaders;

        private FIDConntecionManager()
        {
            // 在构造函数中进行连接设备的操作
            ConnectDevices();
        }


        public static FIDConntecionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FIDConntecionManager();
                }
                return instance;
            }
        }

        private void ConnectDevices()
        {
            jwReaders = new List<JWIpReader>();
            foreach (var item in ips)
            {
                var data = new JWIpReader();
                data.ipAddress = item;

            }
        }

        private List<string> ips = new List<string>() { "10.99.109.210"
            , "10.99.109.211","10.99.109.212","10.99.109.213","10.99.109.214"
            ,"10.99.109.215","10.99.109.216","10.99.109.217","10.99.109.218"
            ,"10.99.109.219","10.99.109.220","10.99.109.221","10.99.109.222"
            ,"10.99.109.223"
        }; 
    }
}
