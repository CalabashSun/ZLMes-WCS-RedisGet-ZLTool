using Newtonsoft.Json;
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
using ZLOperateTool.Helper;
using ZLOperateTool.Models;
using ZLOperateTool.Services;

namespace ZLOperateTool.AGVTool
{
    public partial class TeamUpAgvForm : Form
    {
        public TeamUpAgvForm()
        {
            InitializeComponent();
        }


        private void TeamUpAgvForm_Load(object sender, EventArgs e)
        {
            var manualTeamArea = ConfigurationManager.AppSettings["manualTeamArea"];
            var manualTeam = Convert.ToInt32(manualTeamArea);          
            var agvService = new AgvPositionService();
            var data1Result = agvService.GetFlitchAgvPosition(6, manualTeam);
            //var data1Result = agvService.GetAgvPosition(6);

            var dataSoruce = new List<string>();
            foreach (var item in data1Result)
            {
                var name1 = item.PositionName + "-" + item.PositionCode;
                dataSoruce.Add(name1);
            }
            this.pickingtrayEndCmx.DataSource = dataSoruce;
            this.PipeEndTrayCbx.DataSource = dataSoruce;
            this.TemUpSetEmpty.DataSource = dataSoruce;
            this.flitchEndCmx.DataSource = dataSoruce;
        }

        /// <summary>
        /// 组队工位选择拣选完成的物料托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            PickingTrayListForm pickingForm = new PickingTrayListForm();
            pickingForm.FormClosed += PickingListForm_FormClosed;
            pickingForm.Show();
        }

        /// <summary>
        /// 拣选料关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PickingListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PickingTrayListForm gridForm = (PickingTrayListForm)sender;
            if (gridForm.SelectedRow != null)
            {
                if (gridForm.SelectedRow.Cells[0].Value != null)
                {
                    // 将选中行的数据设置到上一个界面的文本框
                    this.pickingTrayTxt.Text = gridForm.SelectedRow.Cells[0].Value.ToString() + ","
                        + gridForm.SelectedRow.Cells[1].Value.ToString() + ","
                        + gridForm.SelectedRow.Cells[2].Value.ToString(); // 假设选择的是第一列数据
                }

            }
        }


        /// <summary>
        /// 组队工位呼叫拣选托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button1_Click(object sender, EventArgs e)
        {
            //获取起始点位
            var chossePosition = this.pickingTrayTxt.Text;
            if (!chossePosition.Contains(","))
            {
                MessageBox.Show("请选择一个拣选完成的托盘");
            }
            var startPoint = chossePosition.Split(',')[0];
            var endSelectPosition = this.pickingtrayEndCmx.Text;
            if (!endSelectPosition.Contains("-"))
            {
                MessageBox.Show("请选择一个终点");
            }
            var endPoint = endSelectPosition.Split('-')[1];
            DialogResult result = MessageBox.Show("确认要呼叫拣选物料托盘吗", "确认框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                //呼叫AGV
                var positionService = new AgvPositionService();
                var taskService = new AgvTaskService();
                var reqCode = "TCPTray" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var orderNo = "ToolOrder" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                #region 生产AGV任务
                //生成AGV任务单
                var task2 = new third_agvtask();
                task2.AgvCode = "";
                task2.DataSource = 1;
                task2.EndStation = endPoint;
                task2.ReportTicketCode = "";//暂时先放着
                task2.ReqCode = reqCode;
                task2.StartStation = startPoint;
                task2.Status = 0;
                task2.TaskNo = orderNo;
                task2.TaskStatus = 0;
                task2.TaskStep = 1;
                task2.TaskType = "呼叫带有拣选物料的托盘到组队工位";
                task2.PlaceHex = -1;
                task2.CreateDt = DateTime.Now;


                //执行AGV命令
                var agvTask = new SchedulingTaskRquest();
                agvTask.reqCode = reqCode;
                agvTask.taskCode = orderNo;
                agvTask.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                agvTask.taskTyp = "ZL1";
                agvTask.taskCode = orderNo;
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
                        //置空起始点位
                        var startPosition = positionService.GetPosition(task2.StartStation);
                        startPosition.PositionState = 3;
                        startPosition.UpdateDt = DateTime.Now;
                        startPosition.ReqCode = orderNo;
                        startPosition.UpdateDt = DateTime.Now;
                        startPosition.TrayData = 0;
                        startPosition.TrayType = 0;
                        positionService.UpdatePosition(startPosition);

                        var endPosition = positionService.GetPosition(task2.EndStation);
                        endPosition.PositionState = 3;
                        endPosition.UpdateDt = DateTime.Now;
                        endPosition.ReqCode = orderNo;
                        endPosition.TrayData = 2;
                        endPosition.TrayType = 2;
                        endPosition.PositionRemark = startPosition.PositionRemark;
                        positionService.UpdatePosition(endPosition);
                    }
                    //记录请求数据
                    //记录日志
                    var logModel = new third_apirecord
                    {
                        DataInfo = JsonConvert.SerializeObject(agvTask),
                        RequestType = "2",
                        TaskNo = orderNo,
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
                #endregion
            }
            else
            {
                return;
            }
        }


        /// <summary>
        /// 组队工位呼叫矩管托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void callPipeTray_Click(object sender, EventArgs e)
        {
            //获取起始点位
            var chossePosition = this.pipetxt.Text;
            if (!chossePosition.Contains(","))
            {
                MessageBox.Show("请选择一种矩管");
            }
            var startPoint = chossePosition.Split(',')[0];
            var endSelectPosition = this.PipeEndTrayCbx.Text;
            if (!endSelectPosition.Contains("-"))
            {
                MessageBox.Show("请选择一个终点");
            }
            var endPoint = endSelectPosition.Split('-')[1];
            DialogResult result = MessageBox.Show("确认要呼叫矩管料吗", "确认框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                //呼叫AGV
                var positionService = new AgvPositionService();
                var taskService = new AgvTaskService();
                var reqCode = "TCallPipeTray" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var orderNo = "ToolOrder" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                #region 生产AGV任务
                //生成AGV任务单
                var task2 = new third_agvtask();
                task2.AgvCode = "";
                task2.DataSource = 1;
                task2.EndStation = endPoint;
                task2.ReportTicketCode = "";//暂时先放着
                task2.ReqCode = reqCode;
                task2.StartStation = startPoint;
                task2.Status = 0;
                task2.TaskNo = orderNo;
                task2.TaskStatus = 0;
                task2.TaskStep = 1;
                task2.TaskType = "呼叫带有矩管的托盘到组队工位";
                task2.PlaceHex = -1;
                task2.CreateDt = DateTime.Now;


                //执行AGV命令
                var agvTask = new SchedulingTaskRquest();
                agvTask.reqCode = reqCode;
                agvTask.taskCode = orderNo;
                agvTask.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                agvTask.taskTyp = "ZL2";
                agvTask.taskCode = orderNo;
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
                        var oldDesc = "";
                        //置空起始点位
                        var startPosition = positionService.GetPosition(task2.StartStation);
                        startPosition.PositionState = 3;
                        startPosition.UpdateDt = DateTime.Now;
                        startPosition.ReqCode = orderNo;
                        startPosition.UpdateDt = DateTime.Now;
                        startPosition.TrayData = 0;
                        startPosition.TrayType = 0;
                        oldDesc = startPosition.PositionRemark;
                        startPosition.PositionRemark = "最近一次任务是组队工位把矩管呼叫走";
                        positionService.UpdatePosition(startPosition);

                        var endPosition = positionService.GetPosition(task2.EndStation);
                        endPosition.PositionState = 3;
                        endPosition.UpdateDt = DateTime.Now;
                        endPosition.ReqCode = orderNo;
                        endPosition.TrayData = 2;
                        endPosition.TrayType = 1;
                        endPosition.PositionRemark = "矩管工位来料到组队工位:"+ oldDesc;
                        positionService.UpdatePosition(endPosition);
                    }
                    //记录请求数据
                    //记录日志
                    var logModel = new third_apirecord
                    {
                        DataInfo = JsonConvert.SerializeObject(agvTask),
                        RequestType = "2",
                        TaskNo = orderNo,
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
                #endregion
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 组队工位选择矩管托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void choosePipe_Click(object sender, EventArgs e)
        {
            PipeTrayListForm pipeForm = new PipeTrayListForm();
            pipeForm.FormClosed += PipeTrayListForm_FormClosed;
            pipeForm.Show();
        }
        /// <summary>
        /// 矩管托盘选择界面关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PipeTrayListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PipeTrayListForm gridForm = (PipeTrayListForm)sender;
            if (gridForm.PipeSelectedRow != null)
            {
                if (gridForm.PipeSelectedRow.Cells[0].Value != null)
                {
                    // 将选中行的数据设置到上一个界面的文本框
                    this.pipetxt.Text = gridForm.PipeSelectedRow.Cells[0].Value.ToString() + ","
                        + gridForm.PipeSelectedRow.Cells[1].Value.ToString() + ","
                        + gridForm.PipeSelectedRow.Cells[2].Value.ToString(); // 假设选择的是第一列数据
                }
            }
        }


        /// <summary>
        /// 组队工位呼叫贴板托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            FlitchTrayListForm flitchForm = new FlitchTrayListForm();
            flitchForm.FormClosed += FlitchTrayListForm_FormClosed;
            flitchForm.Show();
        }

        /// <summary>
        /// 组队工位贴板托盘选择界面关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FlitchTrayListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FlitchTrayListForm gridForm = (FlitchTrayListForm)sender;
            if (gridForm.SelectedRow != null)
            {
                if (gridForm.SelectedRow.Cells[0].Value != null)
                {
                    // 将选中行的数据设置到上一个界面的文本框
                    this.flitchTxt.Text = gridForm.SelectedRow.Cells[0].Value.ToString() + ","
                        + gridForm.SelectedRow.Cells[1].Value.ToString() + ","
                        + gridForm.SelectedRow.Cells[2].Value.ToString(); // 假设选择的是第一列数据
                }
            }
        }

        /// <summary>
        /// 呼叫贴板工位完成的托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            //获取起始点位
            var chossePosition = this.flitchTxt.Text;
            if (!chossePosition.Contains(","))
            {
                MessageBox.Show("请选择一种托盘");
            }
            var startPoint = chossePosition.Split(',')[0];
            var endSelectPosition = this.flitchEndCmx.Text;
            if (!endSelectPosition.Contains("-"))
            {
                MessageBox.Show("请选择一个终点");
            }
            var endPoint = endSelectPosition.Split('-')[1];
            DialogResult result = MessageBox.Show("确认要呼叫贴板料："+ chossePosition.Split(',')[2]+" 吗？", "确认框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                //呼叫AGV
                var positionService = new AgvPositionService();
                var taskService = new AgvTaskService();
                var reqCode = "TCFTray" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var orderNo = "ToolOrder" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                #region 生产AGV任务
                //生成AGV任务单
                var task2 = new third_agvtask();
                task2.AgvCode = "";
                task2.DataSource = 1;
                task2.EndStation = endPoint;
                task2.ReportTicketCode = "";//暂时先放着
                task2.ReqCode = reqCode;
                task2.StartStation = startPoint;
                task2.Status = 0;
                task2.TaskNo = orderNo;
                task2.TaskStatus = 0;
                task2.TaskStep = 1;
                task2.TaskType = "呼叫带有贴板的托盘到组队工位";
                task2.PlaceHex = -1;
                task2.CreateDt = DateTime.Now;


                //执行AGV命令
                var agvTask = new SchedulingTaskRquest();
                agvTask.reqCode = reqCode;
                agvTask.taskCode = orderNo;
                agvTask.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                agvTask.taskTyp = "ZL2";
                agvTask.taskCode = orderNo;
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
                        var oldDesc = "";
                        //置空起始点位
                        var startPosition = positionService.GetPosition(task2.StartStation);
                        startPosition.PositionState = 3;
                        startPosition.UpdateDt = DateTime.Now;
                        startPosition.ReqCode = orderNo;
                        startPosition.UpdateDt = DateTime.Now;
                        startPosition.TrayData = 0;
                        startPosition.TrayType = 0;
                        oldDesc = startPosition.PositionRemark;
                        startPosition.PositionRemark = "最近一次任务是组队工位把贴板呼叫走";
                        positionService.UpdatePosition(startPosition);

                        var endPosition = positionService.GetPosition(task2.EndStation);
                        endPosition.PositionState = 3;
                        endPosition.UpdateDt = DateTime.Now;
                        endPosition.ReqCode = orderNo;
                        endPosition.TrayData = 2;
                        endPosition.TrayType = 1;
                        endPosition.PositionRemark = "贴板缓存区来料到组队工位:" + oldDesc;
                        positionService.UpdatePosition(endPosition);
                    }
                    //记录请求数据
                    //记录日志
                    var logModel = new third_apirecord
                    {
                        DataInfo = JsonConvert.SerializeObject(agvTask),
                        RequestType = "2",
                        TaskNo = orderNo,
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
                #endregion
            }
            else
            {
                return;
            }
        }


        /// <summary>
        /// 置空组队工位托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            var emptyPosition = this.TemUpSetEmpty.Text;
            if (!emptyPosition.Contains("-"))
            {
                MessageBox.Show("请选择一个已经空掉的托盘点位");
            }
            var emptyPositionCode = emptyPosition.Split('-')[1];
            DialogResult result = MessageBox.Show("确定要置空点位吗，置空就托盘将被送至缓存区", "确认框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            var agvPositionService = new AgvPositionService();
            var taskService = new AgvTaskService();
            var positionInfo = agvPositionService.GetPosition(emptyPositionCode);
               if (positionInfo != null)
            {
                //获取position详情
                if (positionInfo.TrayType == 1)
                {
                    //返回至下料线缓存区
                    var collectionEmptyPostionChoose = agvPositionService.CallCollectionEmptyPosition(2);
                    if (collectionEmptyPostionChoose == null)
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
                    task2.EndStation = collectionEmptyPostionChoose.PositionCode;
                    task2.ReportTicketCode = "";//暂时先放着
                    task2.ReqCode = reqCode;
                    task2.StartStation = emptyPositionCode;
                    task2.Status = 0;
                    task2.TaskNo = orderNo;
                    task2.TaskStatus = 0;
                    task2.TaskStep = 1;
                    task2.TaskType = "贴板焊接工位返回给下料线一个空托盘";
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
                            var collectionStartPosition = agvPositionService.GetPosition(task2.StartStation);
                            collectionStartPosition.PositionState = 3;
                            collectionStartPosition.TrayData = 0;
                            collectionStartPosition.TrayType = 0;
                            collectionStartPosition.UpdateDt = DateTime.Now;
                            collectionStartPosition.ReqCode = orderNo;
                            agvPositionService.UpdatePosition(collectionStartPosition);
                            var collectionEndPosition = agvPositionService.GetPosition(task2.EndStation);
                            collectionEndPosition.PositionState = 3;
                            collectionStartPosition.TrayData =1;
                            collectionStartPosition.TrayType = 1;
                            collectionEndPosition.UpdateDt = DateTime.Now;
                            collectionEndPosition.ReqCode = reqCode;
                            collectionEndPosition.PositionRemark = "矩管空托盘";
                            agvPositionService.UpdatePosition(collectionEndPosition);
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

                }
                else if (positionInfo.TrayType == 2||positionInfo.TrayType==3)
                {
                    var trayName = positionInfo.TrayType == 3 ? "贴板焊接工位托盘" : "拣选托盘";
                    //返回至结构线缓存区
                    //返回至下料线缓存区
                    var flitchEmptyPostionChoose = agvPositionService.CallCollectionEmptyPosition(1);
                    if (flitchEmptyPostionChoose == null)
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
                    task2.EndStation = flitchEmptyPostionChoose.PositionCode;
                    task2.ReportTicketCode = "";//暂时先放着
                    task2.ReqCode = reqCode;
                    task2.StartStation = emptyPositionCode;
                    task2.Status = 0;
                    task2.TaskNo = orderNo;
                    task2.TaskStatus = 0;
                    task2.TaskStep = 1;
                    task2.TaskType = "人工组队工位返回给结构线缓存区一个空托盘，托盘类型为："+ trayName;
                    task2.PlaceHex = -1;

                    //执行AGV命令
                    var agvTask = new SchedulingTaskRquest();
                    agvTask.reqCode = reqCode;
                    agvTask.taskCode = orderNo;
                    agvTask.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    agvTask.taskTyp = "ZL1";
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
                            var flitchStartPosition = agvPositionService.GetPosition(task2.StartStation);
                            flitchStartPosition.PositionState = 3;
                            flitchStartPosition.TrayData = 0;
                            flitchStartPosition.TrayType = 0;
                            flitchStartPosition.UpdateDt = DateTime.Now;
                            flitchStartPosition.ReqCode = orderNo;
                            agvPositionService.UpdatePosition(flitchStartPosition);
                            var flitchEndPosition = agvPositionService.GetPosition(task2.EndStation);
                            flitchEndPosition.PositionState = 3;
                            flitchEndPosition.TrayData = 1;
                            flitchEndPosition.TrayType = positionInfo.TrayType;
                            flitchEndPosition.UpdateDt = DateTime.Now;
                            flitchEndPosition.ReqCode = reqCode;
                            flitchEndPosition.PositionRemark = trayName;
                            agvPositionService.UpdatePosition(flitchEndPosition);
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
                MessageBox.Show("点位信息不存在或者已经被禁用");
                return;
            }
        }

        
    }
}
