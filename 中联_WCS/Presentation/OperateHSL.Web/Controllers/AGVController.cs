using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OperateHSL.Core.Helpers;
using OperateHSL.Data.AGV;
using OperateHSL.Data.AGVCallBack;
using OperateHSL.Data.DataModel.ThirdPartModel;
using OperateHSL.Services.AgvService;
using OperateHSL.Services.LogService;

namespace OperateHSL.Web.Controllers
{
    /// <summary>
    /// agv接口交互
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class AGVController : Controller
    {
        private readonly IAgvControlService _agvControlService;
        private readonly IApiLogService _logService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agvControlService"></param>
        /// <param name="logService"></param>
        public AGVController(IAgvControlService agvControlService, IApiLogService logService)
        {
            _agvControlService = agvControlService;
            _logService = logService;
        }

        /// <summary>
        /// AGV返回结果回告
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AgvCallBack([FromBody]DataCallBackRequest model)
        {
            var result = new DataCallBackResponse();
            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(model),
                RequestType = "2",
                TaskNo = model.reqCode,
                DataType="1"
            };
            try
            {
                DataCallBackResponse dataResult = new DataCallBackResponse();
                try
                {
                    dataResult = await _agvControlService.AgvTaskCallBack(model, result);
                }
                catch
                {
                }

                //记录日志
                logModel.CreateDt = DateTime.Now;
                logModel.UpdateDt = DateTime.Now;
                logModel.DataResult = JsonConvert.SerializeObject(dataResult);
                _logService.AddApiLogRecord(logModel);
                return Json(dataResult);
            }
            catch (Exception ex)
            {
                var dataResult = new DataCallBackResponse();
                dataResult.code = "-1";
                dataResult.message = "程序异常"+ex.Message;
                dataResult.reqCode = model.reqCode;
                return Json(dataResult);
            }
        }

        /// <summary>
        /// AGV处理光栅门数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AgvHandleDoor([FromBody]DataHanldeDoor model)
        {
            var result = new DataCallBackResponse();
            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(model),
                RequestType = "2",
                TaskNo = model.reqCode,
                DataType = "4"
            };
            try
            {
                var dataResult = await _agvControlService.DoorTaskCallBack(model, result);
                //记录日志
                logModel.CreateDt = DateTime.Now;
                logModel.UpdateDt = DateTime.Now;
                logModel.DataResult = JsonConvert.SerializeObject(dataResult);
                _logService.AddApiLogRecord(logModel);
                return Json(dataResult);
            }
            catch (Exception ex)
            {
                var dataResult = new DataCallBackResponse();
                dataResult.code = "-1";
                dataResult.message = "程序异常" + ex.Message;
                dataResult.reqCode = model.reqCode;
                return Json(dataResult);
            }
        }

    }
}