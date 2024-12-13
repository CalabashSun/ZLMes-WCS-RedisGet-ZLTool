using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.AGV
{
    /// <summary>
    /// taskCode、agvCode、wbCode 和 podCode 四个只填一个，填哪个需要与任务模板配置的 触发类型一致，优先推荐 taskCode
    /// </summary>
    public class ContinueTaskRequest
    {
        /// <summary>
        /// 请求编号，每个请求都要一个唯一
        /// 编号， 同一个请求重复提交， 使
        /// 用同一编号。
        /// </summary>
        public string reqCode { get; set; }
        /// <summary>
        /// 请求时间截 格式: “yyyy-MM-dd HH:mm:ss”。
        /// </summary>
        public string reqTime { get; set; }
        /// <summary>
        /// 客户端编号，如 PDA，HCWMS 等
        /// </summary>
        public string clientCode { get; set; }
        /// <summary>
        /// 令牌号, 由调度系统颁发。
        /// </summary>
        public string tokenCode { get; set; }
        /// <summary>
        /// 工作位，一般为机台或工作台位置，与 RCS-2000 端配置的位置名称一致, 工作位名称为字母\数字\或组合, 不超过 32 位。
        /// </summary>
        public string wbCode { get; set; }
        /// <summary>
        /// 货架编号，不指定货架可以为空
        /// </summary>
        public string podCode { get; set; }
        /// <summary>
        /// AGV 编号，填写表示指定某一编号的 AGV 执行该任务
        /// </summary>
        public string agvCode { get; set; }

        /// <summary>
        /// 任务单号,选填, 不填系统自动生成，UUID 小于等于 64 位
        /// </summary>
        public string taskCode { get; set; }

        /// <summary>
        /// 下一个子任务的序列，指定第几个子任务开始执行，校验子任务执行是否正确。不填默认执行下一个子任务
        /// </summary>
        public string taskSeq { get; set; }

        /// <summary>
        /// 下一个位置信息，在任务类型中配置外部设置时需要传入，否则不需要设置。待现场地图部署、配置完成后可获取
        /// </summary>
        public List<NextPositionCode> nextPositionCodes { get; set; } = new List<NextPositionCode>();
    }

    /// <summary>
    /// 下一个位置信息，在任务类型中配置外部设置时需要传入，否则不需要设置。待现场地图部署、配置完成后可获取
    /// </summary>
    public class NextPositionCode
    {
        /// <summary>
        /// 位置编号
        /// </summary>
        public string positionCode { get; set; }
        /// <summary>
        /// 对象类型定义00:代表 nextPositionCode 是一个位置02:代表 nextPositionCode 是一个策略
        /// </summary>
        public string type { get; set; }
    }
}
