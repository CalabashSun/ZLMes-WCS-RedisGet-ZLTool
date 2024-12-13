using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JW.UHF;
using OperateHSL.Core.DbContext;
using OperateHSL.Core.Helpers;
using OperateHSL.Services.PlcInfoService;

namespace OperateHSL.Services.FIDInfoService
{
    public class FIDService : IFIDService
    {
        private static List<JWIpReader> jwReaders;
        private readonly IPlcDataService _plcDataService;
        public FIDService(IPlcDataService plcDataService)
        {
            _plcDataService = plcDataService;
            //ConnectService();
        }

        private void ConnectService()
        {
            #region 提前连接好rfid设备但是启动太慢了
            //var efidIpList = _plcDataService.GetPLCFIDIP();
            //foreach (var item in efidIpList)
            //{
            //    var ip = item.FidIpAddress;
            //    var model = new JWIpReader();
            //    model.ipAddress = item.FidIpAddress;
            //    var jwReader = new JWReader(ip, 9761);
            //    Result result = jwReader.RFID_Open();
            //    if (result != Result.OK)
            //    {
            //        #region 第二次尝试打开模块
            //        result = jwReader.RFID_Open();
            //        if (result != Result.OK)
            //        {
            //            continue;
            //        }
            //        #endregion
            //    }
            //    RfidSetting rs = new RfidSetting();
            //    rs.AntennaPort_List = new List<AntennaPort>();
            //    AntennaPort ap = new AntennaPort();
            //    ap.AntennaIndex = 0;//天线0
            //    ap.Power = 30;//功率设为30
            //    rs.AntennaPort_List.Add(ap);
            //    rs.GPIO_Config = null;
            //    rs.Inventory_Time = 0;
            //    rs.Region_List = RegionList.CCC;
            //    rs.Speed_Mode = SpeedMode.SPEED_NORMAL;
            //    rs.Tag_Group = new TagGroup();
            //    rs.Tag_Group.SessionTarget = SessionTarget.A;
            //    rs.Tag_Group.SearchMode = SearchMode.DUAL_TARGET;
            //    rs.Tag_Group.Session = Session.S0;
            //    result = jwReader.RFID_Set_Config(rs);
            //    if (result != Result.OK)
            //    {
            //        continue;
            //    }
            //    model.jwReader = jwReader;
            //    jwReaders.Add(model);
            #endregion
        }
        public string ReadIFidInfo(string ip)
        {
            try
            {
                var jwReader = new JWReader(ip, 9761);
                Result result = jwReader.RFID_Open();
                if (result != Result.OK)
                {
                    #region 第二次尝试打开模块
                    result = jwReader.RFID_Open();
                    if (result != Result.OK)
                    {
                        return "RFID Open Module Failure";
                    }
                    #endregion
                }
                #region 配置模块
                RfidSetting rs = new RfidSetting();
                rs.AntennaPort_List = new List<AntennaPort>();
                AntennaPort ap = new AntennaPort();
                ap.AntennaIndex = 0;//天线0
                ap.Power = 30;//功率设为30
                rs.AntennaPort_List.Add(ap);

                rs.GPIO_Config = null;
                rs.Inventory_Time = 0;
                rs.Region_List = RegionList.CCC;
                rs.Speed_Mode = SpeedMode.SPEED_NORMAL;
                rs.Tag_Group = new TagGroup();
                rs.Tag_Group.SessionTarget = SessionTarget.A;
                rs.Tag_Group.SearchMode = SearchMode.DUAL_TARGET;
                rs.Tag_Group.Session = Session.S0;
                result = jwReader.RFID_Set_Config(rs);
                if (result != Result.OK)
                {
                    return "RFID Config Set Failure";
                }
                #endregion
                var dataResult = "data not receive";
                #region 读取rfid数据
                if (!jwReader.IsConnected)
                {
                    return "RFID device disconnect";
                }
                int readCount = 0;
                string data = "";
                AccessParam accessP = AssembleReadAccessParam();
                if (accessP == null)
                    return "";

                while (readCount < 10)
                {

                    Result redResult = jwReader.RFID_Read(accessP, out data);
                    readCount++;
                    if (result != Result.OK && data != "")//读取失败
                        continue;
                    else
                    {
                        dataResult = Util.DisplayFormatHexStr((String)data);
                        dataResult = Util.FormatHexStr(dataResult);
                        break;
                    }
                }
                jwReader.RFID_Close();
                #endregion
                return dataResult;
            }
            catch (Exception ex)
            {
                return "RFID exception error" + ex.Message;
            }

        }

        public string ReadIFidInfoByDb(string ip)
        {
            return  _plcDataService.GetLastReadData(ip);
        }

        public string ReadIFidInfoNew(string ip)
        {
            try
            {
                JWIpReader currentReader = null;
                if (jwReaders != null)
                {
                    currentReader=jwReaders.FirstOrDefault(p => p.ipAddress == ip);
                }
                 
                if (currentReader == null)
                {
                    var jwReader = new JWReader(ip, 9761);
                    Result result = jwReader.RFID_Open();
                    if (result != Result.OK)
                    {
                        #region 第二次尝试打开模块
                        result = jwReader.RFID_Open();
                        if (result != Result.OK)
                        {
                            return "RFID Open Module Failure";
                        }
                        #endregion
                    }
                    #region 配置模块
                    RfidSetting rs = new RfidSetting();
                    rs.AntennaPort_List = new List<AntennaPort>();
                    AntennaPort ap = new AntennaPort();
                    ap.AntennaIndex = 0;//天线0
                    ap.Power = 30;//功率设为30
                    rs.AntennaPort_List.Add(ap);

                    rs.GPIO_Config = null;
                    rs.Inventory_Time = 0;
                    rs.Region_List = RegionList.CCC;
                    rs.Speed_Mode = SpeedMode.SPEED_NORMAL;
                    rs.Tag_Group = new TagGroup();
                    rs.Tag_Group.SessionTarget = SessionTarget.A;
                    rs.Tag_Group.SearchMode = SearchMode.DUAL_TARGET;
                    rs.Tag_Group.Session = Session.S0;
                    result = jwReader.RFID_Set_Config(rs);
                    if (result != Result.OK)
                    {
                        return "RFID Config Set Failure";
                    }
                    #endregion
                    var dataResult = "data not receive";
                    #region 读取rfid数据
                    if (!jwReader.IsConnected)
                    {
                        return "RFID device disconnect";
                    }
                    int readCount = 0;
                    string data = "";
                    AccessParam accessP = AssembleReadAccessParam();
                    if (accessP == null)
                        return "";

                    while (readCount < 10)
                    {

                        Result redResult = jwReader.RFID_Read(accessP, out data);
                        readCount++;
                        if (result != Result.OK && data != "")//读取失败
                            continue;
                        else
                        {
                            dataResult = Util.DisplayFormatHexStr((String)data);
                            dataResult = Util.FormatHexStr(dataResult);
                            break;
                        }
                    }
                    #endregion
                    var newRFIdInfo = new JWIpReader { jwReader = jwReader, ipAddress = ip };
                    if (jwReaders == null)
                    {
                        jwReaders = new List<JWIpReader>();
                    }
                    jwReaders.Add(newRFIdInfo);
                    return dataResult;
                }
                else
                {
                    //var jwReader = currentReader.jwReader;
                    var jwReader = ChargeConnect(currentReader.jwReader,ip);
                    var dataResult = "data not receive";
                    #region 读取rfid数据
                    if (!jwReader.IsConnected)
                    {
                        return "RFID device disconnect";
                    }
                    int readCount = 0;
                    string data = "";
                    AccessParam accessP = AssembleReadAccessParam();
                    if (accessP == null)
                        return "";
                    while (readCount < 10)
                    {

                        Result redResult = jwReader.RFID_Read(accessP, out data);
                        readCount++;
                        if (redResult != Result.OK && data != "")//读取失败
                            continue;
                        else
                        {
                            dataResult = Util.DisplayFormatHexStr((String)data);
                            dataResult = Util.FormatHexStr(dataResult);
                            break;
                        }
                    }
                    #endregion
                    
                    return dataResult;
                }
            }
            catch(Exception ex)
            {
                return "RFID exception error"+ex.Message;
            }
            
        }


        private JWReader ChargeConnect(JWReader reader,string ip)
        {
            if (!reader.IsConnected)
            {
                //重新尝试连接
                reader = new JWReader(ip, 9761);
                Result result = reader.RFID_Open();
                if (result != Result.OK)
                {
                    #region 第二次尝试打开模块
                    result = reader.RFID_Open();
                    if (result != Result.OK)
                    {
                        return null;
                    }
                    #endregion
                }
                #region 配置模块
                RfidSetting rs = new RfidSetting();
                rs.AntennaPort_List = new List<AntennaPort>();
                AntennaPort ap = new AntennaPort();
                ap.AntennaIndex = 0;//天线0
                ap.Power = 30;//功率设为30
                rs.AntennaPort_List.Add(ap);

                rs.GPIO_Config = null;
                rs.Inventory_Time = 0;
                rs.Region_List = RegionList.CCC;
                rs.Speed_Mode = SpeedMode.SPEED_NORMAL;
                rs.Tag_Group = new TagGroup();
                rs.Tag_Group.SessionTarget = SessionTarget.A;
                rs.Tag_Group.SearchMode = SearchMode.DUAL_TARGET;
                rs.Tag_Group.Session = Session.S0;
                result = reader.RFID_Set_Config(rs);
                if (result != Result.OK)
                {
                    return null;
                }
                #endregion
                var dataRemove = jwReaders.FirstOrDefault(p => p.ipAddress == ip);
                if (dataRemove != null)
                {
                    jwReaders.Remove(dataRemove);
                }
                jwReaders.Add(new JWIpReader { jwReader = reader, ipAddress = ip });
                return reader;
            }
            else
            {
                return reader;
            }
        }


        /// <summary>
        /// 组装存取参数
        /// </summary>
        /// <returns></returns>
        private AccessParam AssembleReadAccessParam()
        {
            string accessPwd = "00000000";
            accessPwd = Util.FormatHexStr(accessPwd);
            AccessParam ap = new AccessParam();
            ap.Bank = MemoryBank.EPC;
            ap.Count = 6;
            ap.OffSet = 0;
            ap.AccessPassword = accessPwd;
            return ap;
        }
    }
}
