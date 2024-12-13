using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZLOperateTool.Helper;
using ZLOperateTool.Models;
using ZLOperateTool.Services;

namespace ZLOperateTool.AGVTool
{
    public partial class FlitchAgvForm : Form
    {
        public FlitchAgvForm()
        {
            InitializeComponent();
            var agvService = new AgvPositionService();
            var part2area= ConfigurationManager.AppSettings["flitchArea"];
            var part2IntArea = Convert.ToInt32(part2area);
            var data1Result = agvService.GetFlitchAgvPosition(4, part2IntArea);
            var dataSoruce = new List<string>();
            foreach (var item in data1Result)
            {
                var name1 = item.PositionName + "-" + item.PositionCode;
                dataSoruce.Add(name1);
            }
            var dataSource1 = dataSoruce.ToList();
            var dataSource2 = dataSoruce.ToList();
            var dataSource3 = dataSoruce.ToList();
            var dataSource4 = dataSoruce.ToList();
            var dataSource5 = dataSoruce.ToList();
            this.FlitchEmptyCmb.DataSource = dataSource1;
            this.FlitEndCmb.DataSource = dataSource2;
            this.FlitSetEmpty.DataSource = dataSource3;
            this.PipeEndTrayCbx.DataSource = dataSource4;
            this.pickingtrayEndCmx.DataSource = dataSource5;
        }


        private void FlitchEmptyCmb_Change(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PickingTrayListForm pickingForm = new PickingTrayListForm();
            pickingForm.FormClosed += PickingListForm_FormClosed;
            pickingForm.Show();
        }


        private void choosePipe_Click(object sender, EventArgs e)
        {
            PipeTrayListForm pipeForm = new PipeTrayListForm();
            pipeForm.FormClosed += PipeTrayListForm_FormClosed;
            pipeForm.Show();
        }


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
        /// 贴板焊接下料呼叫AGV执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void collectionCallEndBtn_Click(object sender, EventArgs e)
        {
            var comboxText = this.FlitEndCmb.Text;
            var materialName = this.collectionMN.Text;
            var materilCout = this.collectionMC.Text;
            if (string.IsNullOrEmpty(materialName) || string.IsNullOrEmpty(materilCout))
            {
                MessageBox.Show("请填写正确物料名称和数量");
                return;
            }
            DialogResult result = MessageBox.Show("确认要下料吗？点位为：" + comboxText + "," + "物料为：" + materialName + "," + "数量为：" + materilCout, "确认框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                    var emptyPosition = positionService.CallCollectionEmptyPosition(5);
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
                    task2.TaskType = "贴板焊接线送走实物托盘:" + "物料/" + materialName + ",数量/" + materilCout; ;
                    task2.PlaceHex = -1;

                    //执行AGV命令
                    var agvTask = new SchedulingTaskRquest();
                    agvTask.reqCode = reqCode;
                    agvTask.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //这边要等一下看看AGV配置多少
                    agvTask.taskTyp = "ZL3";
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
                            startPosition.TrayData =0;
                            startPosition.UpdateDt = DateTime.Now;
                            startPosition.ReqCode = reqCode;
                            positionService.UpdatePosition(startPosition);
                            var endPosition = positionService.GetPosition(task2.EndStation);
                            endPosition.PositionState = 3;
                            endPosition.TrayData = 2;
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
        
        /// <summary>
        /// 贴板焊接工位呼叫空托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pickingCallEmptyBtn_Click(object sender, EventArgs e)
        {

            var comboxText = this.FlitchEmptyCmb.Text;
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
                    var positionService = new AgvPositionService();
                    var emptyTray = positionService.CallFlitchEmptyTray();
                    if (emptyTray == null)
                    {
                        MessageBox.Show("暂无可用空托盘");
                        return;
                    }
                    var reqCode = "TFCEmpty" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    var orderNo = "ToolOrder" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
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
                    task2.TaskType = "贴板焊接位呼叫空托盘";
                    task2.PlaceHex = -1;

                    //执行AGV命令
                    var agvTask = new SchedulingTaskRquest();
                    agvTask.reqCode = reqCode;
                    agvTask.taskCode = orderNo;
                    agvTask.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    agvTask.taskTyp = "ZL3";
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
                            startPosition.PositionRemark = "贴板焊接呼叫空托盘，此位置置空";
                            positionService.UpdatePosition(startPosition);
                            var endPosition = positionService.GetPosition(task2.EndStation);
                            endPosition.PositionState = 3;
                            endPosition.TrayData =2;
                            endPosition.TrayType = 3;
                            endPosition.UpdateDt = DateTime.Now;
                            endPosition.ReqCode = reqCode;
                            endPosition.PositionRemark = "贴板焊接呼叫空托盘，此位置为贴板焊接落料托盘";
                            positionService.UpdatePosition(endPosition);
                        }
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
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 呼叫矩管托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
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
                var reqCode = "TCPT" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
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
                task2.TaskType = "呼叫带有矩管的托盘到贴板工位";
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
                        endPosition.TrayType = 1;
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
        /// 呼叫拣选料托盘
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
                task2.TaskType = "呼叫带有拣选物料的托盘到贴板工位";
                task2.PlaceHex = -1;
                task2.CreateDt = DateTime.Now;


                //执行AGV命令
                var agvTask = new SchedulingTaskRquest();
                agvTask.reqCode = reqCode;
                agvTask.taskCode = orderNo;
                agvTask.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                agvTask.taskTyp = "ZL3";
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
        /// 置空贴板点位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            var emptyPosition = this.FlitSetEmpty.Text;
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
                    task2.TaskType = "贴板焊接工位返回给下料线一个空托盘" ;
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
                            collectionStartPosition.PositionState =3;
                            collectionStartPosition.TrayData = 0;
                            collectionStartPosition.TrayType = 0;
                            collectionStartPosition.UpdateDt = DateTime.Now;
                            collectionStartPosition.ReqCode = orderNo;
                            agvPositionService.UpdatePosition(collectionStartPosition);
                            var collectionEndPosition = agvPositionService.GetPosition(task2.EndStation);
                            collectionEndPosition.PositionState = 3;
                            collectionStartPosition.TrayData = 1;
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
                else if (positionInfo.TrayType == 2)
                {
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
                    task2.TaskType = "贴板焊接工位返回给结构线缓存区一个空托盘";
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
                            flitchEndPosition.TrayData = 0;
                            flitchEndPosition.TrayType = positionInfo.TrayType;
                            flitchEndPosition.UpdateDt = DateTime.Now;
                            flitchEndPosition.ReqCode = reqCode;
                            flitchEndPosition.PositionRemark = "拣选空托盘";
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

        private void FlitchAgvForm_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // 获取项目启动路径
            string startupPath = Application.StartupPath;

            // 构建完整的图片文件路径
            string imagePath = Path.Combine(startupPath, "tieban.PNG");

            // 加载图片文件
            Image image = Image.FromFile(imagePath);

            // 创建自定义对话框窗体并显示图片
            using (ImageDialog imageDialog = new ImageDialog(image))
            {
                imageDialog.ShowDialog();
            }
        }
    }
}
