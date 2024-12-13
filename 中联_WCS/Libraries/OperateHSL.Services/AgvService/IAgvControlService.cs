using OperateHSL.Data.AGV;
using OperateHSL.Data.AGVCallBack;
using OperateHSL.Data.DataModel.ThirdPartModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OperateHSL.Services.AgvService
{
    public interface IAgvControlService
    {

        /// <summary>
        /// 获取Agv任务
        /// </summary>
        /// <param name="getSql"></param>
        /// <returns></returns>
        Third_AgvTask GetAgvTask(string getSql);
        /// <summary>
        /// 调度任务新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        SchedulingTaskResponse SchedulingTask(SchedulingTaskRquest model);

        

        /// <summary>
        /// 继续任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ContinueTaskResponse ContinueTask(ContinueTaskRequest model);

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CancelTaskResponse CancelTask(CancelTaskRequest model);


        /// <summary>
        /// 新增agv任务
        /// </summary>
        Task AddAgvScheduleTask();


        /// <summary>
        /// 新增补焊房agv任务
        /// </summary>
        Task AddrequireHAgvScheduleTaskAsync();
        Task GSScheduleTaskAsync();

        /// <summary>
        /// 清空agv托盘占用
        /// </summary>
        /// <returns></returns>
        Task ClearTrayPosition();

        /// <summary>
        /// agv任务回馈
        /// </summary>
        Task<DataCallBackResponse> AgvTaskCallBack(DataCallBackRequest model, DataCallBackResponse result);



        /// <summary>
        /// 处理光栅数据
        /// </summary>
        Task<DataCallBackResponse> DoorTaskCallBack(DataHanldeDoor model, DataCallBackResponse result);

        /// <summary>
        /// 处理光栅数据
        /// </summary>
        Task DoorTaskCallBackSelf(int doorId,int openType);
        /// <summary>
        /// 完成组队任务托盘下料
        /// </summary>
        /// <param name="positionHex"></param>
        /// <param name="materialName"></param>
        /// <param name="materialCount"></param>
        /// <returns></returns>
        Task CompleteTeamUpPosition(int positionHex, string materialName, int materialCount);

        /// <summary>
        /// 行架下料更新点位
        /// </summary>
        /// <param name="position"></param>
        /// <param name="materialName"></param>
        /// <param name="materialCount"></param>
        /// <returns></returns>
        Task TrussesDownPosition(string position, string materialName, int materialCount);


        /// <summary>
        /// 关闭打磨房信号
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task CloseWeldingRoomData(int position, int type);


        /// <summary>
        /// 操作下件区信号
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type">1:</param>
        /// <returns></returns>
        Task HandleStructuralSignal(string position, int type);

        /// <summary>
        /// 添加置空托盘调度任务
        /// </summary>
        /// <returns></returns>
        Task AddTransRickingEmptyTrayTask();


        /// <summary>
        /// 下件区满托盘送到人工打磨缓存区
        /// </summary>
        /// <returns></returns>
        Task AddTransStrusFullTrayTask();
    }
}
