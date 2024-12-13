using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using ZLOperateTool.Helper;
using ZLOperateTool.Models.TaskOrder;

namespace ZLOperateTool.OrderTool
{
    public partial class TeamUpOrder : UIForm2
    {
        /// <summary>
        /// 
        /// </summary>
        private static int orderState = 2;
        private static Dictionary<string,string> materialSort=new Dictionary<string,string>();
        private static string ipcCode= ConfigurationManager.AppSettings["ipcCode"];


        public TeamUpOrder()
        {
            
            InitializeComponent();
            uiStyleManager1.Style = UIStyle.Green;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // 禁用最大化按钮和最小化按钮
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            uiDataGridView1.AutoGenerateColumns = false;
            uiDataGridView1.AllowUserToAddRows = false;
            uiDataGridView1.ReadOnly = true;
            uiDataGridView1.RowTemplate.Height = 45;
            
            uiPagination1.PageSize = 10;
            uiDataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            uiDataGridViewFooter1.DataGridView = uiDataGridView1;

            startTimePicker.Value = DateTime.Now.AddDays(-7);
            endTimePicker.Value= DateTime.Now.AddDays(7);
        }

        /// <summary>
        /// 分页控件页面切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pagingSource"></param>
        /// <param name="pageIndex"></param>
        /// <param name="count"></param>
        private void uiPagination1_PageChanged(object sender, object pagingSource, int pageIndex, int count)
        {

            uiDataGridView1.Rows.Clear();
            
            var resultPage = getPageData(pageIndex, count, orderState);
            BindPageData(resultPage,pageIndex,count);
            
        }

        public void BindPageData(PageData resultPage,int pageIndex, int count)
        {
            if (resultPage == null)
            {
                this.ShowErrorDialog2("获取分页数据时发生错误，请关闭程序后重试！！！");
                uiDataGridViewFooter1.Clear();
                uiDataGridView1.ClearSelection();
            }
            else
            {
                uiPagination1.TotalCount = resultPage.TotalCount;
                var upOrders = resultPage.DataList.ToObject<List<TaskOrderModel>>();
                for (int i = (pageIndex - 1) * count; i < pageIndex * count; i++)
                {
                    if (i >= resultPage.TotalCount) break;
                    int rowIndex = uiDataGridView1.Rows.Add();
                    var productFid = upOrders[i].FidCode;
                    if (productFid.Contains("0808"))
                    {
                        productFid = "0808";
                    }
                    else
                    {
                        productFid = "";
                    }
                    uiDataGridView1.Rows[rowIndex].Cells["Id"].Value = upOrders[i].Id;
                    uiDataGridView1.Rows[rowIndex].Cells["FidCode"].Value = upOrders[i].MesProductDesc;
                    uiDataGridView1.Rows[rowIndex].Cells["mesorder"].Value = upOrders[i].mesorder;
                    uiDataGridView1.Rows[rowIndex].Cells["ProductCode"].Value = upOrders[i].ProductCode;
                    uiDataGridView1.Rows[rowIndex].Cells["ProductName"].Value = upOrders[i].ProductName + "(" + upOrders[i].MesProductDesc + ")   " + productFid;
                    uiDataGridView1.Rows[rowIndex].Cells["uloc"].Value = upOrders[i].uloc;
                    uiDataGridView1.Rows[rowIndex].Cells["TaskCount"].Value = upOrders[i].TaskCount;
                    uiDataGridView1.Rows[rowIndex].Cells["TaskCompletedCount"].Value = upOrders[i].TaskCompletedCount;
                    uiDataGridView1.Rows[rowIndex].Cells["CreateDt"].Value = upOrders[i].CreateDt;
                    uiDataGridView1.Rows[rowIndex].Cells["TaskCompletedTime"].Value = upOrders[i].TaskCompletedTime;
                }
                uiDataGridViewFooter1.Clear();
                uiDataGridView1.ClearSelection();
            }
            
        }

        private void uiDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 确保单击的单元格是有效的单元格且不是标头
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // 选中整行
                uiDataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }

        /// <summary>
        /// 已完成工单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void completedBtn_Click(object sender, EventArgs e)
        {
            uiPagination1.ActivePage = 1;
            orderState = 1;
            uiDataGridView1.Rows.Clear();
            var mesOrder = this.orderTxt.Text;
            var productName=this.productTxt.Text;
            var startTime = startTimePicker.Text;
            var endTime = endTimePicker.Text;

            var resultPage = getPageData(1, 10, orderState);
            BindPageData(resultPage, 1, 10);
        }

        private void manualBtn_Click(object sender, EventArgs e)
        {
            OpenRFIDForm("");
        }

        private void unBtn_Click(object sender, EventArgs e)
        {
            uiPagination1.ActivePage = 1;
            orderState = 2;
            uiDataGridView1.Rows.Clear();
            var mesOrder = this.orderTxt.Text;
            var productName = this.productTxt.Text;

            var startTime = startTimePicker.Text;
            var endTime = endTimePicker.Text;

            var resultPage = getPageData(1, 10, orderState);
            BindPageData(resultPage, 1, 10);
        }

        private void allBtn_Click(object sender, EventArgs e)
        {
            uiPagination1.ActivePage = 1;
            orderState = -1;
            uiDataGridView1.Rows.Clear();
            var resultPage = getPageData(1, 10, orderState);
            BindPageData(resultPage, 1, 10);
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            uiPagination1.ActivePage = 1;
            uiDataGridView1.Rows.Clear();
            var resultPage = getPageData(1, 10, orderState);
            BindPageData(resultPage, 1, 10);
        }
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // 确保只有一行被选中
            if (uiDataGridView1.SelectedRows.Count > 1)
            {
                foreach (DataGridViewRow row in uiDataGridView1.SelectedRows)
                {
                    if (!row.Selected)
                    {
                        row.Selected = false;
                    }
                }
            }
        }
        private void startOrder_Click(object sender, EventArgs e)
        {
            if (uiDataGridView1.SelectedRows.Count == 0)
            {
                this.ShowErrorTip("未检测到您选择的工单！！！");
                return;
            }

            // 确保只有一行被选中
            if (uiDataGridView1.SelectedRows.Count > 1)
            {
                this.ShowErrorTip("检测到您选中了多行工单请退出重新选择！！！");
                return;
            }
            //开始工单
            DataGridViewRow selectedRow = uiDataGridView1.SelectedRows[0];
            var taskId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
            var mesOrder = selectedRow.Cells["mesOrder"].Value.ToString();
            //先暂定productCode为04NC1后面实际拿到订单信息可能还要从 bominfo_materialdata表里面取
            var productName = selectedRow.Cells["ProductCode"].Value.ToString();
            var rfidCodeCell= selectedRow.Cells["FidCode"].Value.ToString();
            //判断订单是否已完工
            var completedTime= selectedRow.Cells["TaskCompletedTime"].Value.ToString();
            if (!string.IsNullOrEmpty(completedTime))
            {
                this.ShowErrorTip("该工单已完成不允许重复报工");
                return;
            }


            if (this.ShowAskDialog2("确认要完成工单:"+mesOrder+",产品:"+ rfidCodeCell + "|||"+productName + "吗?工单提交成功后请根据提示写入RFID！！！", true))
            {

                this.ShowProcessForm();
                var rfidCode = rfidCodeCell + DateTime.Now.ToString("yyyyMMddHHmm") + Util.GenerateRandomNumber(3);
                var commitResult = ReportTask(taskId, ipcCode, rfidCode);
                if (commitResult!=null&&commitResult.Status == 100)
                {
                    _ = ReportRfidStart(mesOrder, rfidCode);
                    //准备写入RFID
                    OpenRFIDForm(rfidCode);
                }
                else
                {
                    this.ShowErrorDialog2("工单提交失败，请稍后重试，或联系信息化人员处理");
                }

                uiPagination1.ActivePage = 1;
                uiDataGridView1.Rows.Clear();
                var resultPage = getPageData(1, 10, orderState);
                BindPageData(resultPage, 1, 10);
                this.HideProcessForm();
            }
        }

        /// <summary>
        /// 先报工给mes 看mes结果 在操作工单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private async void ayncStartOrder_Click(object sender, EventArgs e)
        {
            if (uiDataGridView1.SelectedRows.Count == 0)
            {
                this.ShowErrorTip("未检测到您选择的工单！！！");
                return;
            }

            // 确保只有一行被选中
            if (uiDataGridView1.SelectedRows.Count > 1)
            {
                this.ShowErrorTip("检测到您选中了多行工单请退出重新选择！！！");
                return;
            }
            //开始工单
            DataGridViewRow selectedRow = uiDataGridView1.SelectedRows[0];
            var taskId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
            var mesOrder = selectedRow.Cells["mesOrder"].Value.ToString();
            //先暂定productCode为04NC1后面实际拿到订单信息可能还要从 bominfo_materialdata表里面取
            var productName = selectedRow.Cells["ProductCode"].Value.ToString();
            var rfidCodeCell = selectedRow.Cells["FidCode"].Value.ToString();
            //判断订单是否已完工
            var completedTime = selectedRow.Cells["TaskCompletedTime"].Value.ToString();
            if (!string.IsNullOrEmpty(completedTime))
            {
                this.ShowErrorTip("该工单已完成不允许重复报工");
                return;
            }
            if (this.ShowAskDialog2("确认要完成工单:" + mesOrder + ",产品:" + rfidCodeCell + "|||" + productName + "吗?工单提交成功后请根据提示写入RFID！！！", true))
            {
                var rfidCode = rfidCodeCell + DateTime.Now.ToString("yyyyMMddHHmm") + Util.GenerateRandomNumber(3);
                var wcsResult = await ReportRfidStartSync(mesOrder, rfidCode);
                if (wcsResult == "success")
                {
                    var commitResult = ReportTask(taskId, ipcCode, rfidCode);
                    if (commitResult != null && commitResult.Status == 100)
                    {
                        //准备写入RFID
                        OpenRFIDForm(rfidCode);
                    }
                    else
                    {
                        this.ShowErrorDialog2("工单提交失败，请稍后重试，或联系信息化人员处理");
                    }
                }
                else
                {
                    this.ShowErrorDialog2("工单提交失败:" + wcsResult);
                }
                uiPagination1.ActivePage = 1;
                uiDataGridView1.Rows.Clear();
                var resultPage = getPageData(1, 10, orderState);
                BindPageData(resultPage, 1, 10);
            }


        }



        #region get Mes data
        /// <summary>
        /// 获取工单数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="taskState"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="productName"></param>
        /// <param name="mesOrder"></param>
        /// <returns></returns>
        public PageData getPageData(int page, int pageSize, int taskState)
        {
            var mesOrder = this.orderTxt.Text;
            var productName = this.productTxt.Text;
            var startTime = startTimePicker.Text;
            var endTime = endTimePicker.Text;
            var mesOrderUrl = ConfigurationManager.AppSettings["mesUrl"] + $"/FormMesOrder/TicketTaskInfo?Page={page}&PageSize={pageSize}&ipcCode={ipcCode}&ticketState={taskState}" +
                $"&productName={productName}&mesOrder={mesOrder}&startTime={startTime}&endTime={endTime}";
            var request = new RestRequest();
            request.Timeout = 5000;
            try
            {
                this.ShowProcessForm();
                RestClient client = new RestClient(mesOrderUrl);
                var result = client.Get(request);
                var resultData = JsonConvert.DeserializeObject<PageInfo>(result.Content);
                var pageInfo = resultData.Data.ToObject<PageData>();
                this.HideProcessForm();
                return pageInfo;
            }
            catch
            {
                this.HideProcessForm();
                return null;
            }
            

        }

        public MesMessage ReportTask(int taskId, string ipcCode, string rfidCode)
        {
            var mesOrderUrl = ConfigurationManager.AppSettings["mesUrl"] + $"/FormMesOrder/ReportTickets?taskId={taskId}&ipccode={ipcCode}&rfidCode={rfidCode}";
            var request = new RestRequest();
            request.Timeout = 5000;
            RestClient client = new RestClient(mesOrderUrl);
            try
            {
                this.ShowProcessForm();
                var result = client.Get(request);
                var resultData = JsonConvert.DeserializeObject<MesMessage>(result.Content);
                this.HideProcessForm();
                return resultData;
            }
            catch
            {
                this.HideProcessForm();
                return null;
            }
           
        }
        #endregion


        #region 打开RFID窗体
        public void OpenRFIDForm(string rfidCode)
        {
            FidWrite fidForm=Application.OpenForms.OfType<FidWrite>().FirstOrDefault();
            if (fidForm == null)
            {
                fidForm = new FidWrite();
                fidForm.UpdateParameter(rfidCode);
                fidForm.Show();
            }
            else
            {
                if (fidForm.WindowState == FormWindowState.Minimized)
                {
                    fidForm.WindowState = FormWindowState.Normal; // 恢复正常状态
                }
                fidForm.Activate();
                fidForm.Focus();
                fidForm.UpdateParameter(rfidCode);
            }
        }
        #endregion



        #region get wcs data
        public async Task ReportRfidStart(string mesOrder,string rfidCode)
        {
            var mesOrderUrl = ConfigurationManager.AppSettings["wcsUrl"] + $"/api/MesData/SNDataCallBack?jobNo={mesOrder}&sn={rfidCode}";
            var request = new RestRequest();
            RestClient client = new RestClient(mesOrderUrl);
            await client.GetAsync(request);
        }


        public async Task<string> ReportRfidStartSync(string mesOrder, string rfidCode)
        {
            var mesOrderUrl = ConfigurationManager.AppSettings["wcsUrl"] + $"/api/MesData/SNDataCallBackSync?jobNo={mesOrder}&sn={rfidCode}";
            var request = new RestRequest();
            RestClient client = new RestClient(mesOrderUrl);
            var result= await client.GetAsync(request);
            if (result.Content.Contains("200"))
            {
                return "success";
            }
            else
            {
                return result.Content;
            }
        }
        #endregion


        // 在父窗体中的 FormClosing 事件处理程序中检测子窗体
        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Form childForm in Application.OpenForms)
            {
                if (childForm is FidWrite)
                {
                    this.ShowErrorDialog2("RFID窗体还在运行中，请先关闭RFID窗体");
                    // 阻止父窗体关闭
                    e.Cancel = true;
                    return;
                }
            }

        }
    }
}
