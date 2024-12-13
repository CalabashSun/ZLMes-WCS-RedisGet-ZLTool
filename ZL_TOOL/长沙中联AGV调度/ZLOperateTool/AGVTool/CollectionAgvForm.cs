using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZLOperateTool.Helper;
using ZLOperateTool.Models;
using ZLOperateTool.Services;

namespace ZLOperateTool.AGVTool
{
    public partial class CollectionAgvForm : Form
    {
        public CollectionAgvForm()
        {
            InitializeComponent();
            var agvService = new AgvPositionService();
            var data1Result = agvService.GetAgvPosition(1);
            var waitPolish = agvService.GetAgvPositionWherePolishCompleted();
            var dataSoruce=new List<string>();
            foreach (var item in data1Result)
            {
                var name1 = item.PositionName + "-" + item.PositionCode;
                dataSoruce.Add(name1);
            }
           
            var dataSourcePolish = new List<string>();
            foreach (var item2 in waitPolish)
            {
                var name1 = item2.PositionName + "-" + item2.PositionCode+"-"+item2.PositionRemark;
                dataSourcePolish.Add(name1);
            }
            var dataSource1 = dataSoruce.ToList();
            var dataSource2 = dataSoruce.ToList();
            this.collectionEmptyCmb.DataSource = dataSource1;
            this.collectionEndCmb.DataSource = dataSource2;
            this.waitPolishCMX.DataSource = dataSourcePolish;
        }
      
        private void collectionCallEmptyBtn_Click(object sender, EventArgs e)
        {           
            var comboxText = this.collectionEmptyCmb.Text;
            DialogResult result = MessageBox.Show("确认要呼叫空托盘吗？点位为：" + comboxText, "确认框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //判断该点位是否有正在执行的任务
                var taskService = new AgvTaskService();
                //获取点位
                var positionCode = comboxText.Split('-')[1];
                var dataCount = taskService.TaskOn(positionCode);
                if (dataCount > 0)
                {
                    MessageBox.Show("该点位存在正在执行的任务，请等待执行完成后重试");
                }
                else
                {
                    //寻找一个空托盘                                                                               
                    var positionService=new AgvPositionService();
                    var emptyTray = positionService.CallCollectionEmptyTray();
                    if (emptyTray == null)
                    {
                        MessageBox.Show("暂无可用空托盘");
                        return;
                    }
                    var reqCode = "ToolCallEmpty" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    var orderNo="ToolOrder"+ DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    //生成AGV任务单
                    var task2 = new third_agvtask();
                    task2.AgvCode = "";
                    task2.DataSource = 1;
                    task2.EndStation = positionCode;
                    task2.ReportTicketCode = "";//暂时先放着
                    task2.ReqCode = reqCode;
                    task2.StartStation = emptyTray.PositionCode;
                    task2.Status = 0;
                    task2.TaskNo = orderNo;
                    task2.TaskStatus = 0;
                    task2.TaskStep = 1;
                    task2.TaskType = "下料线呼叫空托盘";
                    task2.PlaceHex = -1;
                    task2.CreateDt = DateTime.Now;
                    
                    //执行AGV命令
                    var agvTask = new SchedulingTaskRquest();
                    agvTask.reqCode = reqCode;
                    agvTask.taskCode = orderNo;
                    agvTask.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    agvTask.taskTyp = "ZL2";
                    string[] agvValue = { task2.StartStation, task2.EndStation };
                    agvTask.userCallCodePath = agvValue;
                    var postAgvResult = PostTaskAgv.PostToAgv(agvTask);
                    if (postAgvResult == null)
                    {
                        task2.IsDeleted = 1;
                        task2.TaskType = "AGV服务器未响应请稍后重试";
                        MessageBox.Show("AGV服务器未响应请稍后重试");
                    }
                    else
                    {
                        if (postAgvResult != null && postAgvResult.message.Contains("成功"))
                        {
                            task2.Status = 1;
                            //冻结起始还有终点点位数据
                            var startPosition = positionService.GetPosition(task2.StartStation);
                            startPosition.PositionState = 3;
                            startPosition.TrayData = 0;
                            startPosition.TrayType = 0;
                            startPosition.TrayOrder = orderNo;
                            startPosition.UpdateDt = DateTime.Now;
                            startPosition.ReqCode = reqCode;
                            positionService.UpdatePosition(startPosition);  
                            var endPosition=positionService.GetPosition(task2.EndStation);
                            endPosition.PositionState = 3;
                            endPosition.TrayData = 2;
                            endPosition.TrayType = 1;
                            endPosition.UpdateDt = DateTime.Now;
                            endPosition.ReqCode = reqCode;
                            positionService.UpdatePosition(endPosition);
                        }
                    }

                    //记录请求数据
                    //记录日志
                    var logModel = new third_apirecord
                    {
                        DataInfo = JsonConvert.SerializeObject(agvTask),
                        RequestType = "2",
                        TaskNo = reqCode,
                        DataType = "1",
                        DataResult = JsonConvert.SerializeObject(postAgvResult),
                        CreateDt = DateTime.Now,
                        UpdateDt = DateTime.Now
                    };
                    var recordService = new ApiRecordService();
                    recordService.AddApiRecord(logModel);
                    taskService.CreatAgvTask(task2);
                    MessageBox.Show("SUCCESS");
                }
            }
            else
            {
                return;
            }
        }

        private void collectionCallEndBtn_Click(object sender, EventArgs e)
        {
            var comboxText = this.collectionEndCmb.Text;
            var materialName = this.collectionMN.Text;
            var materilCout = this.collectionMC.Text;
            if (string.IsNullOrEmpty(materialName) || string.IsNullOrEmpty(materilCout))
            {
                MessageBox.Show("请填写正确物料名称和数量");
                return;
            }

            var materialType = "";
            if (materialName.Contains("12"))
            {
                materialType = "12";
            }
            if (materialName.Contains("06"))
            {
                materialType = "06";
            }
            if (materialName.Contains("04"))
            {
                materialType = "04";
            }
            if (materialType == "")
            {
                MessageBox.Show("物料名称中必须为04，06，12中的一种");
                return;
            }
            DialogResult result = MessageBox.Show("确认要下料吗？点位为：" + comboxText+","+"物料为："+materialName+","+"数量为："+materilCout, "确认框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //判断该点位是否有正在执行的任务
                var taskService = new AgvTaskService();
                //获取点位
                var positionCode = comboxText.Split('-')[1];
                var dataCount = taskService.TaskOn(positionCode);
                if (dataCount > 0)
                {
                    MessageBox.Show("该点位存在正在执行的任务，请等待执行完成后重试");
                }
                else
                {
                    //寻找一个空托盘
                    var positionService = new AgvPositionService();
                    var emptyPosition = positionService.CallMaterialEmptyPosition(materialType);
                    if (emptyPosition == null)
                    {
                        MessageBox.Show("暂无可用空置点位");
                        return;
                    }
                    var reqCode = "ToolSendTray" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    var orderNo = "ToolOrder" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    //生成AGV任务单
                    var task2 = new third_agvtask();
                    task2.AgvCode = "";
                    task2.DataSource = 1;
                    task2.EndStation = emptyPosition.PositionCode;
                    task2.ReportTicketCode = "";//暂时先放着
                    task2.ReqCode = reqCode;
                    task2.StartStation = positionCode;
                    task2.Status = 0;
                    task2.TaskNo = orderNo;
                    task2.TaskStatus = 0;
                    task2.TaskStep = 1;
                    task2.TaskType = "下料线送走实物托盘:"+ "物料/" + materialName + ",数量/" + materilCout; ;
                    task2.PlaceHex = -1;

                    //执行AGV命令
                    var agvTask = new SchedulingTaskRquest();
                    agvTask.reqCode = reqCode;
                    agvTask.taskCode = orderNo;
                    agvTask.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    agvTask.taskTyp = "ZL2";
                    string[] agvValue = { task2.StartStation, task2.EndStation };
                    agvTask.userCallCodePath = agvValue;
                    var postAgvResult = PostTaskAgv.PostToAgv(agvTask);
                    if (postAgvResult == null)
                    {
                        task2.IsDeleted = 1;
                        task2.TaskType = "AGV服务器未响应请稍后重试";
                        MessageBox.Show("AGV服务器未响应请稍后重试");
                    }
                    else
                    {
                        if (postAgvResult != null && postAgvResult.message.Contains("成功"))
                        {
                            task2.Status = 1;
                            //冻结起始还有终点点位数据
                            var startPosition = positionService.GetPosition(task2.StartStation);
                            startPosition.PositionState = 3;
                            startPosition.TrayData = 0;
                            startPosition.TrayType = 0;

                            startPosition.UpdateDt = DateTime.Now;
                            startPosition.ReqCode = reqCode;
                            positionService.UpdatePosition(startPosition);
                            var endPosition = positionService.GetPosition(task2.EndStation);
                            endPosition.PositionState = 3;
                            endPosition.TrayData = 6;
                            endPosition.TrayType = 1;
                            endPosition.UpdateDt = DateTime.Now;
                            endPosition.ReqCode = reqCode;
                            endPosition.PositionRemark = "物料:" + materialName + ",数量:" + materilCout;
                            positionService.UpdatePosition(endPosition);
                        }
                    }

                    //记录请求数据
                    //记录日志
                    var logModel = new third_apirecord
                    {
                        DataInfo = JsonConvert.SerializeObject(agvTask),
                        RequestType = "2",
                        TaskNo = reqCode,
                        DataType = "1",
                        DataResult = JsonConvert.SerializeObject(postAgvResult),
                        CreateDt = DateTime.Now,
                        UpdateDt = DateTime.Now
                    };
                    var recordService = new ApiRecordService();
                    recordService.AddApiRecord(logModel);
                    taskService.CreatAgvTask(task2);
                    MessageBox.Show("SUCCESS");
                }
            }
            else
            {
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 获取项目启动路径
            string startupPath = Application.StartupPath;

            // 构建完整的图片文件路径
            string imagePath = Path.Combine(startupPath, "xialiaoxian.PNG");

            // 加载图片文件
            Image image = Image.FromFile(imagePath);

            // 创建自定义对话框窗体并显示图片
            using (ImageDialog imageDialog = new ImageDialog(image))
            {
                imageDialog.ShowDialog();
            }
        }

        private void CollectionAgvForm_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 打磨完工
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {            
            var comboxText = this.waitPolishCMX.Text;
            if (string.IsNullOrEmpty(comboxText))
            {
                MessageBox.Show("请选择打磨完工的AGV点位");
                return;
            }
            DialogResult result = MessageBox.Show("确认要在这个点位打磨完工？点位为：" + comboxText, "确认框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //判断该点位是否有正在执行的任务
                var taskService = new AgvTaskService();
                //获取点位
                var positionCode = comboxText.Split('-')[1];
                var dataCount = taskService.TaskOn(positionCode);
                if (dataCount > 0)
                {
                    MessageBox.Show("该点位存在正在执行的任务，请等待执行完成后重试");
                }
                else
                {
                    //寻找一个空托盘
                    var positionService = new AgvPositionService();
                    var completedPosition = positionService.GetPosition(positionCode);
                    completedPosition.PositionState =2;
                    completedPosition.TrayData = 2;
                    completedPosition.TrayType = 1;
                    completedPosition.UpdateDt = DateTime.Now;
                    positionService.UpdatePosition(completedPosition);
                    //更新数据
                    var agvService = new AgvPositionService();
                    var waitPolish = agvService.GetAgvPositionWherePolishCompleted();
                    var dataSourcePolish = new List<string>();
                    foreach (var item2 in waitPolish)
                    {
                        var name1 = item2.PositionName + "-" + item2.PositionCode + "-" + item2.PositionRemark;
                        dataSourcePolish.Add(name1);
                    }
                    this.waitPolishCMX.DataSource = dataSourcePolish;
                    MessageBox.Show("SUCCESS");
                }
            }
            else
            {
                return;
            }
        }


      

  
        private void collectionCallCompleteBtn_Click(object sender, EventArgs e)
        {
            var agvService = new AgvPositionService();
            var waitPolish = agvService.GetAgvPositionWherePolishCompleted();
            var dataSourcePolish = new List<string>();
            foreach (var item2 in waitPolish)
            {
                var name1 = item2.PositionName + "-" + item2.PositionCode;
                dataSourcePolish.Add(name1);
            }
            this.waitPolishCMX.DataSource = dataSourcePolish;
        }
    }
}
