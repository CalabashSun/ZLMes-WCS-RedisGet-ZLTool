using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZLOperateTool.Helper;
using ZLOperateTool.Models;
using ZLOperateTool.Services;

namespace ZLOperateTool.AGVTool
{
    public partial class TestAgvCallback : Form
    {


        public string start1 = "";
        public string start2 = "";
        public string end1 = "";
        public string end2 = "";
        public string startFor = "";
        public string endFor = "";
        public bool isContinue = false;
        public string currentTask1 = "";
        public string currentTask2 = "";

        public TestAgvCallback()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var startPoint = this.textBox1.Text;
            var endPoint = this.textBox2.Text;
            var trayType = this.textBox3.Text;
            DialogResult result = MessageBox.Show("确认要运转托盘吗", "确认框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var positionService = new AgvPositionService();
                var taskService = new AgvTaskService();
                var reqCode = "ToolCallBack" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var orderNo = "ToolCallBackOrder" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
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
                task2.TaskType = "测试AGV接口回告";
                task2.PlaceHex = -1;

                //执行AGV命令
                var agvTask = new SchedulingTaskRquest();
                agvTask.reqCode = reqCode;
                agvTask.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                agvTask.taskTyp = trayType;
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
                        //冻结起始还有终点点位数据
                        var startPosition = positionService.GetPosition(task2.StartStation);
                        startPosition.PositionState = 3;
                        startPosition.UpdateDt = DateTime.Now;
                        startPosition.ReqCode = reqCode;
                        positionService.UpdatePosition(startPosition);
                        var endPosition = positionService.GetPosition(task2.EndStation);
                        endPosition.PositionState = 3;
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
            else {
                return;
            }
        }

        private void FlitchForBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确认要循环运转托盘吗", "确认框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int isFirst = 1;
                if (isFirst == 1)
                {
                    start1 = textBox4.Text;
                    start2 = textBox5.Text;
                    startFor = textBox6.Text;
                    end1 = textBox7.Text;
                    end2 = textBox8.Text;
                    endFor = textBox9.Text;
                }
                isContinue = true;
                Task.Run(() =>
                {
                    while (isContinue)
                    {
                        var taskService = new AgvTaskService();
                        var task1 = 3;
                        var task2 = 3;

                        if (currentTask1 != "" && currentTask2 != "")
                        {
                            //判断agv任务是否完成
                            task1 = taskService.AgvTaskStatus(currentTask1);
                            task2 = taskService.AgvTaskStatus(currentTask2);
                        }
                        if (task1 == 3 && task2 == 3)
                        {
                            var changeSplit = 0;
                            var resultInfo1 = "";
                            var resultInfo2 = "";
                            if (!string.IsNullOrEmpty(start1))
                            {
                                var endForData = endFor.Split(',');
                                var randomR = new Random();
                                int chooseR = randomR.Next(0, endForData.Length - 1);
                                //去终点找一个位置
                                end1 = endFor.Split(',')[chooseR];
                                changeSplit = chooseR;
                                if (end1 == "")
                                {
                                    continue;
                                }
                                resultInfo1 = GenerteAgvTask(start1, end1, 1);
                                if (resultInfo1 == "success")
                                {
                                    //终点起点位置调换
                                    start1 = end1;
                                    end1 = "";
                                }
                            }
                            if (!string.IsNullOrEmpty(start2))
                            {
                                var endForData = endFor.Split(',');
                                if (end2 == endForData[changeSplit] || string.IsNullOrEmpty(end2))
                                {
                                    if (changeSplit == 1)
                                    {
                                        changeSplit = 2;
                                    }
                                    else
                                    {
                                        if (changeSplit <= endForData.Length - 1 && changeSplit != 0)
                                        {
                                            changeSplit = changeSplit - 1;
                                        }
                                        if (changeSplit == 0)
                                        {
                                            changeSplit = changeSplit + 2;
                                        }
                                    }

                                    //去终点找一个位置
                                    end2 = endFor.Split(',')[changeSplit];
                                }
                                resultInfo2 = GenerteAgvTask(start2, end2, 2);
                                if (resultInfo2 == "success")
                                {
                                    //终点起点位置调换
                                    start2 = end2;
                                    end2 = "";
                                }
                            }
                            if (resultInfo1 == "success" && resultInfo2 == "success")
                            {
                                var changeModel = endFor;
                                endFor = startFor;
                                startFor = changeModel;
                            }
                        }
                        else if (task1 == 4 || task2 == 4)
                        {
                            isContinue = true;
                            MessageBox.Show("循环运转时,AGV任务被手动取消，循环运转退出,请重启循环程序");
                        }
                        else {
                            Thread.Sleep(1000 * 60);
                        }
                    };
                    
                });
            }
            else
            {
                return;
            }

        }



        public string GenerteAgvTask(string startPoint,string endPoint,int type)
        {
            var positionService = new AgvPositionService();
            var taskService = new AgvTaskService();
            var reqCode = "ToolCallBack" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var orderNo = "ToolCallBackOrder" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
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
            task2.TaskType = "测试AGV接口回告";
            task2.PlaceHex = -1;
            task2.CreateDt = DateTime.Now;

            //执行AGV命令
            var agvTask = new SchedulingTaskRquest();
            agvTask.reqCode = reqCode;
            agvTask.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            agvTask.taskTyp = "ZL2";
            agvTask.taskCode = orderNo;
            string[] agvValue = { task2.StartStation, task2.EndStation };
            agvTask.userCallCodePath = agvValue;
            var postAgvResult = PostTaskAgv.PostToAgv(agvTask);
            //var postAgvResult = new SchedulingTaskResponse();
            postAgvResult.message = "成功";
            if (postAgvResult == null)
            {
                task2.IsDeleted = 1;
                task2.TaskType = "AGV服务器未响应请稍后重试";
                return "AGV服务器未响应请稍后重试";
            }
            else
            {
                if (postAgvResult != null && postAgvResult.message.Contains("成功"))
                {
                    task2.Status = 1;
                    //冻结起始还有终点点位数据
                    var startPosition = positionService.GetPosition(task2.StartStation);
                    startPosition.PositionState = 3;
                    startPosition.UpdateDt = DateTime.Now;
                    startPosition.ReqCode = reqCode;
                    positionService.UpdatePosition(startPosition);
                    var endPosition = positionService.GetPosition(task2.EndStation);
                    endPosition.PositionState = 3;
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
                TaskNo = orderNo,
                DataType = "1",
                DataResult = JsonConvert.SerializeObject(postAgvResult),
                CreateDt = DateTime.Now,
                UpdateDt = DateTime.Now
            };
            var recordService = new ApiRecordService();
            recordService.AddApiRecord(logModel);
            taskService.CreatAgvTask(task2);
            if (type == 1)
            {
                currentTask1 = task2.TaskNo;
            }
            else
            {
                currentTask2= task2.TaskNo;
            }
            
            return "success";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isContinue = false;
        }
    }
}
