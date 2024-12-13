using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using System.Xml.Linq;
using System.Configuration;
using JW.UHF;
using Newtonsoft.Json.Linq;
using ZLOperateTool.Helper;
using System.Threading;
using System.Net.NetworkInformation;
using Sunny.UI;

namespace ZLOperateTool
{
    public partial class FidWrite : UIForm2
    {

        private JWReader jwReader1 = null;

        private string rfidCode = "";
        public FidWrite()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // 禁用最大化按钮和最小化按钮
            this.MaximizeBox = false;

            this.connectStateBtn.FillColor= Color.Orange;
            this.connectStateBtn.Symbol = 61761;


            Task.Run(() =>
            {
                checkRfidSate();
            });
            Task.Run(() =>
            {
                var connectSate = StartConnect(out jwReader1);
                Invoke((Action)(() =>
                {
                    if (connectSate == "success")
                    {
                        this.connectStateBtn.FillColor = Color.SkyBlue;
                        this.connectStateBtn.Symbol = 361452;
                    }
                    else
                    {
                        this.connectStateBtn.FillColor = Color.Salmon;
                        this.connectStateBtn.Symbol = 62163;

                    }
                    this.label4.Text = connectSate;
                }));
            });
            this.ipaddressLable.Text = this.ipaddressLable.Text + ":"+ConfigurationManager.AppSettings["RFID"];
        }

        public void UpdateParameter(string rfidCode)
        {
            rfidCode= rfidCode.Trim().ToUpper();
            this.textBox1.Text = rfidCode;
        }
        public void checkRfidSate()
        {
            int i = 1;
            while (true) {
                if (i == 1)
                {
                    Thread.Sleep(300000);
                }
                else
                {
                    Thread.Sleep(10000);
                    if (jwReader1 == null || !jwReader1.IsConnected)
                    {
                        var connectSate = StartConnect(out jwReader1);
                        Invoke((Action)(() =>
                        {
                            if (connectSate == "success")
                            {
                                this.connectStateBtn.FillColor = Color.SkyBlue;
                                this.connectStateBtn.Symbol = 361452;
                            }
                            else
                            {
                                this.connectStateBtn.FillColor = Color.Salmon;
                                this.connectStateBtn.Symbol = 62163;

                            }
                            this.label4.Text = connectSate;
                        }));
                    }
                }


            }
        }


        public string StartConnect(out JWReader model)
        {
            if (jwReader1 != null && jwReader1.IsConnected)
            {
                model= jwReader1;
                return "success";
            }

            var  ip = ConfigurationManager.AppSettings["RFID"];
            model = new JWReader(ip, 9761);
            //先判断能不能ping通
            using (Ping ping = new Ping())
            {
                try
                {
                    // 发送 ping 请求
                    PingReply reply = ping.Send(ip, 1000);
                    // 检查 ping 请求的结果
                    if (reply.Status != IPStatus.Success)
                    {
                        return "请检查网线连接";
                    }
                }
                catch
                {
                    return "请检查网线连接";
                }
            }

            #region 打开模块
            Result result = model.RFID_Open();
            if (result != Result.OK)
            {
                #region 第二次尝试打开模块
                result = model.RFID_Open();
                if (result != Result.OK)
                {
                    return "失败";
                }
                #endregion
            }
            #endregion

            #region 配置模块
            RfidSetting rs = new RfidSetting();
            rs.AntennaPort_List = new List<AntennaPort>();
            AntennaPort ap = new AntennaPort();
            ap.AntennaIndex = 0;//天线0
            ap.Power = 30;//功率设为30
            rs.AntennaPort_List.Add(ap);
            rs.Inventory_Time = 0;
            rs.Region_List = RegionList.CCC;
            rs.Speed_Mode = SpeedMode.SPEED_NORMAL;

            var rsFilter = new RSSIFilter();
            rsFilter.Enable = true;
            rsFilter.RSSIValue = -60;
            rs.RSSI_Filter = rsFilter;

            rs.Tag_Group = new TagGroup();
            rs.Tag_Group.SessionTarget = SessionTarget.A;
            rs.Tag_Group.SearchMode = SearchMode.DUAL_TARGET;
            rs.Tag_Group.Session = Session.S0;
            model.RFID_Set_Config(rs);

            if (result != JW.UHF.Result.OK)
            {
                return "RFID Config Set Failure";
            }
            else
            {
                return "success";
            }
            #endregion

        }

        private void fidwritebtn_Click(object sender, EventArgs e)
        {
            if (jwReader1 == null || !jwReader1.IsConnected)
            {
                this.ShowErrorDialog2("请等待RFID连接成功后再执行写入操作！！！长时间连接失败请关闭RFID页面重新打开或联系管理员处理!!!");
                return;
            }
            string value = textBox1.Text.ToString();

            if (value.Length != 5&&value.Length!=20)
            {
                MessageBox.Show("请生成正确位数的产品编号");
                return;
            }
            if (this.is128bit.Checked)
            {
                value = value + DateTime.Now.ToString("mmssfff");
            }
            else if(value.Length==5)
            {
                value = value + DateTime.Now.ToString("yyyyMMddHHmm") + Util.GenerateRandomNumber(3);
            }
            
            
            if (value.Trim().Equals("") || value.Length % 4 != 0)
            {
                MessageBox.Show("输入错误,长度必须是4的倍数");
            }
            else
            {
                DialogResult result = MessageBox.Show("确认要写入Fid吗？Fid为：" + value, "确认框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var js = writeFIDNew(value);
                    MessageBox.Show(js);
                }
                else
                {
                    return;
                }
            }
        }



        /// <summary>
        /// 写入FID
        /// </summary>
        /// <param name="fidCode"></param>
        /// <returns></returns>
        public string writeFIDNew(string fidCode)
        {
            try
            {
                var  writeValue = Util.FormatHexStr(fidCode.ToString());
                if (writeValue.Trim().Equals("") || writeValue.Length % 4 != 0)
                {

                    var failResult = "输入错误,长度必须是4的倍数";
                    return failResult;
                }
                //string 类型转16进制
                writeValue = Util.StringToHexString(writeValue, Encoding.ASCII);
                JObject ac = new JObject();
                AccessParam aps = Util.AssembleWriteAccessParam(fidCode);
                if (aps == null)
                {
                    ac["Code"] = "101";
                    ac["Msg"] = "组装存取结果值错误";
                }
                DateTime starttime = DateTime.Now;
                int writeCount = 0;
                bool writeSuccess = false;
                var resultString = "";
                if (jwReader1 == null || !jwReader1.IsConnected)
                {
                    resultString = StartConnect(out jwReader1);
                    if (resultString != "success")
                    {
                        return resultString;
                    }
                }
                var result = new Result();
                while (writeCount < Constants.WRITE_COUNT)
                {
                    Result results = Result.OK;
                    result = jwReader1.RFID_Write(aps, writeValue);
                    writeCount++;
                    if (result != Result.OK)
                    {
                        continue;
                    }
                    else
                    {
                        string data = "";
                        result = jwReader1.RFID_Read(aps, out data);
                        ac["Code"] = "100";
                        ac["Msg"] = $"写入成功!写入结果为" + Util.HexStringToString(data, Encoding.ASCII); ;
                        writeSuccess = true;
                        rfidCode = "";
                        break;
                    }
                }
                if (!writeSuccess)
                {
                    ac["Code"] = "101";
                    ac["Msg"] = "写入失败";
                }
                if (writeCount == 2)
                {
                    ac["Code"] = "101";
                    ac["Msg"] = $"写入失败,失败原因{result}";

                }
                var resultMsg = Convert.ToString(ac["Msg"]);
                return resultMsg;
            }
            catch (Exception ex)
            {
                JObject ac = new JObject();
                ac["Code"] = "101";
                ac["Msg"] = $"写入失败:{ex}";
                var resultMsg = Convert.ToString(ac["Msg"]);
                return resultMsg;
            }

        }


        /// <summary>
        /// 读取FID
        /// </summary>
        /// <param name="fidCode"></param>
        /// <returns></returns>
        public void readFIDNew()
        {
            AccessParam accessParam = new AccessParam();
            accessParam.Bank = MemoryBank.EPC;
            accessParam.OffSet = 0;
            accessParam.Count = 20;
            string tagData = "";
            var result= jwReader1.RFID_Read(accessParam, out tagData);
            if (result != Result.OK)
            {
                MessageBox.Show("发生错误" + result);
            }
            else
            {
                var newresult = Util.HexStringToString(tagData, Encoding.ASCII);
                this.textBox2.Text = newresult;
            }
            
        }

        /// <summary>
        /// 写入FID
        /// </summary>
        /// <param name="fidCode"></param>
        /// <returns></returns>
        public string writeFID(string fidCode)
        {

            var ip = ConfigurationManager.AppSettings["RFID"];
            var jwReader = new JWReader(ip, 9761);
            try
            {
                JObject ac = new JObject();
                String writeValue = Util.FormatHexStr(fidCode.ToString());
               
                // var key = Encoding.ASCII.GetBytes(writeValue);
                //System.Text.Encoding.ASCII.GetString(buf);
                if (writeValue.Trim().Equals("") || writeValue.Length % 4 != 0)
                {

                    var failResult = "输入错误,长度必须是4的倍数";
                    return failResult;
                }
                //string 类型转16进制
                writeValue = Util.StringToHexString(writeValue, Encoding.ASCII);


                Result result = new Result();
                //Task.Factory.StartNew(()=> {
                if (!jwReader.IsConnected)
                {
                    result = jwReader.RFID_Open();
                }

                //});





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
                rs.Inventory_Time = 0;
                rs.Region_List = RegionList.CCC;
                rs.Speed_Mode = SpeedMode.SPEED_NORMAL;

                var rsFilter = new RSSIFilter();
                rsFilter.Enable = true;
                rsFilter.RSSIValue = -60;
                rs.RSSI_Filter = rsFilter;

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
                AccessParam aps = Util.AssembleWriteAccessParam(fidCode);
                if (aps == null)
                {
                    ac["Code"] = "101";
                    ac["Msg"] = "组装存取结果值错误";
                }
                DateTime starttime = DateTime.Now;
                int writeCount = 0;
                bool writeSuccess = false;
                while (writeCount < Constants.WRITE_COUNT)
                {
                    Result results = Result.OK;
                    result = jwReader.RFID_Write(aps, writeValue);
                    writeCount++;
                    if (result != Result.OK)
                    {
                        continue;
                    }
                    else
                    {
                        string data = "";
                        result = jwReader.RFID_Read(aps, out data);
                        ac["Code"] = "100";
                        ac["Msg"] = $"写入成功!写入结果为"+ Util.HexStringToString(data, Encoding.ASCII); ;
                        writeSuccess = true;
                        break;
                    }
                }
                if (!writeSuccess)
                {
                    ac["Code"] = "101";
                    ac["Msg"] = "写入失败";
                }
                if (writeCount == 2)
                {
                    ac["Code"] = "101";
                    ac["Msg"] = $"写入失败,失败原因{result}";

                }
                


                jwReader.RFID_Close();
                var resultMsg = Convert.ToString(ac["Msg"]);
                return resultMsg;





            }
            catch (Exception ex)
            {
                JObject ac = new JObject();
                ac["Code"] = "101";
                ac["Msg"] = $"写入失败:{ex}";
                jwReader.RFID_Close();
                var resultMsg = Convert.ToString(ac["Msg"]);
                return resultMsg;
            }
            return null;

        }
        public class Constants
        {
            public static String DEFAULT_EPC_READ_WORD_COUNT = "12";
            public static String DEFAULT_EPC_READ_OFFSET = "0";

            public static String DEFAULT_TID_READ_WORD_COUNT = "6";
            public static String DEFAULT_TID_READ_OFFSET = "0";

            public static String DEFAULT_USER_READ_WORD_COUNT = "6";
            public static String DEFAULT_USER_READ_OFFSET = "0";

            public static String DEFAULT_RESERVED_READ_WORD_COUNT = "4";
            public static String DEFAULT_RESERVED_READ_OFFSET = "0";

            public static String INIT_ACCESS_PASSWORD = "00000000";

            public static int READ_COUNT = 2;//读次数
            public static int WRITE_COUNT = 2;//写次数
        }

        private void fidRead_Click(object sender, EventArgs e)
        {
            var resultString = "";
            if (jwReader1 == null || !jwReader1.IsConnected)
            {
                resultString = StartConnect(out jwReader1);
                if (resultString != "success")
                {
                    MessageBox.Show(resultString);
                }
            }
            readFIDNew();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (jwReader1 != null && jwReader1.IsConnected)
            {
                jwReader1.RFID_Close();
            }
            this.connectStateBtn.FillColor = Color.Salmon;
            this.connectStateBtn.Symbol = 62163;
            this.label4.Text = "未连接";
        }


        private void FidFormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.label4.Text != "未连接")
            {
                MessageBox.Show("请先断开RFID设备连接后再关闭窗口");
                e.Cancel= true;
            }
        }
    }
}
