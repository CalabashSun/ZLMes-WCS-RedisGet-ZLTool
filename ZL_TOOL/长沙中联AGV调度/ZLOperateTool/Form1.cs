using Newtonsoft.Json;
using RestSharp;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZLOperateTool.AGVTool;
using ZLOperateTool.BankTool;
using ZLOperateTool.Models.TaskOrder;
using ZLOperateTool.OrderTool;
using ZLOperateTool.Services;

namespace ZLOperateTool
{
    public partial class Form1 : UIForm2
    {
        public Form1()
        {
            this.ShowProcessForm();
            CheckForUpdates();
            this.HideProcessForm();

            InitializeComponent();
            // 禁用最大化按钮和最小化按钮
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.deviceId.Text= ConfigurationManager.AppSettings["ipcCode"];
        }


        #region check update
        private void CheckForUpdates()
        {
            try
            {
                string latestVersion = VersionService.ReadVersionConfig("currentVersion"); // 最新版本号

                var mesVeriosn = ConfigurationManager.AppSettings["mesUrl"] + $"/FormMesOrder/LatestVersion?versionCode={latestVersion}";
                var request = new RestRequest();
                request.Timeout = 4000;
                RestClient client = new RestClient(mesVeriosn);
                var result = client.Get(request);
                var resultData = JsonConvert.DeserializeObject<MesMessage>(result.Content);
                if (resultData.Status != 200 && resultData.Msg != null && resultData.Msg.Contains("."))
                {
                    var fileUrl = ConfigurationManager.AppSettings["mesUrl"] + $"/FormMesOrder/UploadZipFile?versionCode={resultData.Msg}";
                    string zipName = "Update.zip";
                    var uploadRequest = new RestRequest();
                    //下载文件
                    var uploadClient = new RestClient(fileUrl);
                    var zipResult = uploadClient.Get(uploadRequest);
                    if (zipResult.IsSuccessful)
                    {
                        var bytes = new MemoryStream(zipResult.RawBytes);
                        string path = AppDomain.CurrentDomain.BaseDirectory + "/" + zipName;
                        using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            bytes.CopyTo(fs);
                        }
                        bytes.Close();
                    }
                    string dominPath = AppDomain.CurrentDomain.BaseDirectory + "//upload" + "//UpdateToolControl.exe";
                    Process process = new Process() { StartInfo = new ProcessStartInfo(dominPath) };
                    process.Start();
                    Environment.Exit(0);
                }

            }
            catch(Exception ex)
            {

            }
            
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            CollectionAgvForm collectionAgvForm = new CollectionAgvForm();
            collectionAgvForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FlitchAgvForm flitchAgvForm=new FlitchAgvForm();
            flitchAgvForm.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            BankOutForm outForm= new BankOutForm();
            outForm.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TestAgvCallback testAgv = new TestAgvCallback();
            testAgv.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TeamUpAgvForm teamUpAgvForm=new TeamUpAgvForm();
            teamUpAgvForm.ShowDialog();
        }

        private void teamOrderBtn_Click(object sender, EventArgs e)
        {
            this.ShowProcessForm();
            TeamUpOrder orderForm = new TeamUpOrder();
            orderForm.ShowDialog();
        }

        private void AddReportBtn_Click(object sender, EventArgs e)
        {
            AddReportOrder reportOrder = new AddReportOrder();
            reportOrder.ShowDialog();
            this.HideProcessForm();
        }
    }
}
