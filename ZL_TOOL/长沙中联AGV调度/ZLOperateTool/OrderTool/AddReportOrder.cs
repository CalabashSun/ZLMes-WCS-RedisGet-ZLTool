using RestSharp;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZLOperateTool.OrderTool
{
    public partial class AddReportOrder : UIForm2
    {
        public AddReportOrder()
        {
            InitializeComponent();
        }

        private async void teamupBtn_Click(object sender, EventArgs e)
        {
            var mesOrder=mesorderTxt.Text;
            var rfidCode = rfidTxt.Text;
            if (!string.IsNullOrEmpty(mesOrder) && !string.IsNullOrEmpty(rfidCode))
            {
                if (this.ShowAskDialog2("确认要补推送组队报工:" + mesOrder + ",产品:" + rfidCode+"吗?", true))
                {
                    var result = await ReportRfidStartSync(mesOrder,rfidCode);
                    MessageBox.Show(result);
                }
            }
            else
            {
                this.ShowErrorTip("按照规定填数据");
            }
        }



        private async void uiButton1_Click(object sender, EventArgs e)
        {
            var mesOrder = mesorderTxt.Text;
            var rfidCode = rfidTxt.Text;
            if (!string.IsNullOrEmpty(mesOrder) && !string.IsNullOrEmpty(rfidCode))
            {
                if (this.ShowAskDialog2("确认要补推送自动焊报工:" + mesOrder + ",产品:" + rfidCode + "吗?", true))
                {
                    var result = await ReportAutoSync(rfidCode);
                    MessageBox.Show(result);
                }
            }
            else
            {
                this.ShowErrorTip("按照规定填数据");
            }

        }
        public async Task<string> ReportRfidStartSync(string mesOrder, string rfidCode)
        {
            var mesOrderUrl = ConfigurationManager.AppSettings["wcsUrl"] + $"/api/MesData/SNDataCallBackSync?jobNo={mesOrder}&sn={rfidCode}";
            var request = new RestRequest();
            RestClient client = new RestClient(mesOrderUrl);
            var result = await client.GetAsync(request);
            if (result.Content.Contains("200"))
            {
                return "success";
            }
            else
            {
                return result.Content;
            }
        }


        public async Task<string> ReportAutoSync(string rfidCode)
        {
            var mesOrderUrl = ConfigurationManager.AppSettings["wcsUrl"] + $"/api/MesData/AutoDataCallBak?rfidCode={rfidCode}";
            var request = new RestRequest();
            RestClient client = new RestClient(mesOrderUrl);
            var result = await client.GetAsync(request);
            if (result.Content.Contains("200"))
            {
                return "success";
            }
            else
            {
                return result.Content;
            }
        }

        
    }
}
