using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLOperateTool.Models
{
    /// <summary>
    /// wbCode 和 positionCodePath 至少填写其中一项，以确定任务中的位置信息。若任务中需要指定多个位置信息，如起点和终点信息等，请使用 positionCodePath。
    /// </summary>
    public class SchedulingTaskRquest
    {
        public string agvCode { get; set; }

        /// <summary>
        /// 客户端编号，如 PDA，HCWMS 等
        /// </summary>
        public string clientCode { get; set; }

        /// <summary>
        /// 容器编号（叉车/CTU 专用）
        /// </summary>
        public string ctnrCode { get; set; }
        public string ctnrNum { get; set; }
        /// <summary>
        /// 容器类型（叉车/CTU 专用） 叉车项目必传
        /// </summary>
        public string ctnrTyp { get; set; }
        /// <summary>
        /// AGV 编号，填写表示指定某一编号的 AGV 执行该任务
        /// </summary>

        public string data { get; set; }

        public string eFloor { get; set; }
        public string interfaceName { get; set; }
        public string mapCode { get; set; }
        public string mapShortName { get; set; }
        /// <summary>
        /// 物料批次或货架上的物料唯一编码生成任务单时,货架与物料直接绑定时使用. （通过同时传 podCode 和materialLot 来 绑 定 或 通 过wbCode 找 到 位 置 上 的 货 架 和materialLot 来绑定）
        /// </summary>
        public string materialLot { get; set; }
        public string materialType { get; set; }
        public string needReqCode { get; set; }
        /// <summary>
        /// 货架编号，不指定货架可以为空
        /// </summary>
        public string podCode { get; set; }

        /// <summary>
        /// “180”,”0”,”90”,”-90” 分别 对 应 地 图 的 ” 左 ”,” 右 ”,”上”,”下” ，不指定方向可以为空
        /// </summary>
        public string podDir { get; set; }
        /// <summary>
        /// 货架类型, 传空时表示随机找个货架找空货架传参方式如下：-1: 代表不关心货架类型, 找到空货架即可.-2: 代表从工作位获取关联货架类型, 如果未配置, 只找空货架货架类型编号: 只找该货架类型的空货架.
        /// </summary>
        public string podTyp { get; set; }
        /// <summary>
        /// 优先级，从（1~127）级，最大优
        /// </summary>
        public string priority { get; set; }
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
        public string robotCode { get; set; }
        public string sFloor { get; set; }
        /// <summary>
        /// 任务单号,选填, 不填系统自动生成，UUID 小于等于 64 位
        /// </summary>
        public string taskCode { get; set; }
        /// <summary>
        /// 任务类型，与在 RCS-2000 端配置
        /// 的主任务类型编号一致。 厂内货架搬运: F01
        /// 厂内货架空满交换: F02 辊筒搬运接驳:F03 厂内货架出库 AGV 待命:F04 旋转货架: F05 厂内电梯任务: F06
        /// 以下为叉车专用任务类型
        /// 高位货架到工作台: F11 工作台到高位货架: F12 巷道到工作台: F13 工作台到巷道: F14 高位货架到工作台(接驳) : F15 工作台到高位货架 (接驳) : F16 巷道到工作台(接驳) : F17 工作台到巷道(接驳) : F18 叉车电梯主任务: F20
        /// </summary>
        public string taskTyp { get; set; }

        //位置路径
        public string[] userCallCodePath { get; set; }


        public string userCallCode { get; set; }


        /// <summary>
        /// 工作位，一般为机台或工作台位置，与 RCS-2000 端配置的位置名称一致, 工作位名称为字母\数字\或组合, 不超过 32 位。
        /// </summary>
        // public string wbCode { get; set; }


        //public List<PositionCodePath> positionCodePath { get; set; } = new List<PositionCodePath>();
    }

    /// <summary>
    /// 位置路径：AGV 关键路径位置集合，与任务类型中模板配置的位置路径一一对应。待现场地图部署、配置
    /// 完成后可获取positionCode:位置编号, 单个编号不超过 64 位type:位置类型说明:
    /// </summary>
    public class PositionCodePath
    {
        /// <summary>
        /// 位置编号
        /// </summary>
        public string positionCode { get; set; }
        /// <summary>
        /// 位置类型说明
        /// 00 表示：位置编号01 表示：物料批次号02 表示：策略编号（含多个区域）如：第一个区域放不下, 可以放第二
        /// 03 表示：货架编号，通过货架编号个区域找到货架所在位置04 表示：区域编号，在区域中查找可用位置05 表示：仓位编号（叉车/CTU 专用）06 表示：巷道编号（叉车/CTU 专用）
        /// </summary>
        public string type { get; set; }
    }
}
