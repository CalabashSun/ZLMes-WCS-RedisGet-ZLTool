using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.PLC
{
    public class PLCSlicing
    {
        /// <summary>
        /// 矩管1  90*50*3.5
        /// </summary>
        public string RectangularPipeOne { get; set; }
        /// <summary>
        /// 矩管2  100*50*3.5
        /// </summary>
        public string RectangularPipeTwo { get; set; }
        /// <summary>
        /// 矩管3  125*75*3
        /// </summary>
        public string RectangularPipeThree { get; set; }
        /// <summary>
        /// 矩管4  125*75*4.5
        /// </summary>
        public string RectangularPipeFour { get; set; }

        /// <summary>
        /// 矩管1叫料  90*50*3.5
        /// </summary>
        public string RectangularPipeOneCall { get; set; }
        /// <summary>
        /// 矩管2叫料  100*50*3.5
        /// </summary>
        public string RectangularPipeTwoCall { get; set; }
        /// <summary>
        /// 矩管3叫料  125*75*3
        /// </summary>
        public string RectangularPipeThreeCall { get; set; }
        /// <summary>
        /// 矩管4叫料  125*75*4.5
        /// </summary>
        public string RectangularPipeFourCall { get; set; }

        /// <summary>
        /// 最近一次下发的订单号
        /// </summary>
        public string LatestTaskNo { get; set; }

        /// <summary>
        /// 最近一次下发的数量
        /// </summary>
        public string LatestDownNum { get; set; }

        /// <summary>
        /// 最近一次下发的物料类型 1，2，3，4对应 4种型号矩管
        /// </summary>
        public string LatestDownType { get; set; }
        /// <summary>
        /// 最近一次下发的订单随机数
        /// </summary>
        public string LatestTaskRandom { get; set; }

        #region 第一台设备
        /// <summary>
        /// 当前机器生产的订单号
        /// </summary>
        public string FisrtCurrentTaskNo { get; set; }
        /// <summary>
        /// 缓存位1托盘里矩管数
        /// </summary>
        public string FisrtCurrentOneAddressNumber { get; set; }

        /// <summary>
        /// 缓存位2托盘里矩管数
        /// </summary>
        public string FisrtCurrentTwoAddressNumber { get; set; }
        /// <summary>
        /// 缓存位1是否下料即呼叫托盘进行空满转换
        /// </summary>
        public string FisrtCurrentOneAddressPublish { get; set; }
        /// <summary>
        /// 缓存位2是否下料即呼叫托盘进行空满转换
        /// </summary>
        public string FisrtCurrentTwoAddressPublish { get; set; }
        /// <summary>
        /// 当前设备生产的产品类型
        /// </summary>
        public string FisrtCurrentProductType { get; set; }
        /// <summary>
        /// 缓存位1是否有托盘
        /// </summary>
        public string FisrtCurrentOneAddressHasTray { get; set; }
        /// <summary>
        /// 缓存位2是否有托盘
        /// </summary>
        public string FisrtCurrentTwoAddressHasTray { get; set; }
        /// <summary>
        /// 激光切割机1实时切割矩管类型
        /// </summary>
        public string FisrtCurrentMaterialType { get; set; }
        #endregion

        #region 第二台设备
        /// <summary>
        /// 当前机器生产的订单号
        /// </summary>
        public string DoubleCurrentTaskNo { get; set; }
        /// <summary>
        /// 缓存位1托盘里矩管数
        /// </summary>
        public string DoubleCurrentOneAddressNumber { get; set; }

        /// <summary>
        /// 缓存位2托盘里矩管数
        /// </summary>
        public string DoubleCurrentTwoAddressNumber { get; set; }
        /// <summary>
        /// 缓存位1是否下料即呼叫托盘进行空满转换
        /// </summary>
        public string DoubleCurrentOneAddressPublish { get; set; }
        /// <summary>
        /// 缓存位2是否下料即呼叫托盘进行空满转换
        /// </summary>
        public string DoubleCurrentTwoAddressPublish { get; set; }
        /// <summary>
        /// 当前设备生产的产品类型
        /// </summary>
        public string DoubleCurrentProductType { get; set; }
        /// <summary>
        /// 缓存位1是否有托盘
        /// </summary>
        public string DoubleCurrentOneAddressHasTray { get; set; }
        /// <summary>
        /// 缓存位2是否有托盘
        /// </summary>
        public string DoubleCurrentTwoAddressHasTray { get; set; }
        /// <summary>
        /// 激光切割机2实时切割矩管类型
        /// </summary>
        public string DoubleCurrentMaterialType { get; set; }
        #endregion

        #region 第三台设备
        /// <summary>
        /// 当前机器生产的订单号
        /// </summary>
        public string TripleCurrentTaskNo { get; set; }
        /// <summary>
        /// 缓存位1托盘里矩管数
        /// </summary>
        public string TripleCurrentOneAddressNumber { get; set; }

        /// <summary>
        /// 缓存位2托盘里矩管数
        /// </summary>
        public string TripleCurrentTwoAddressNumber { get; set; }
        /// <summary>
        /// 缓存位1是否下料即呼叫托盘进行空满转换
        /// </summary>
        public string TripleCurrentOneAddressPublish { get; set; }
        /// <summary>
        /// 缓存位2是否下料即呼叫托盘进行空满转换
        /// </summary>
        public string TripleCurrentTwoAddressPublish { get; set; }
        /// <summary>
        /// 当前设备生产的产品类型
        /// </summary>
        public string TripleCurrentProductType { get; set; }
        /// <summary>
        /// 缓存位1是否有托盘
        /// </summary>
        public string TripleCurrentOneAddressHasTray { get; set; }
        /// <summary>
        /// 缓存位2是否有托盘
        /// </summary>
        public string TripleCurrentTwoAddressHasTray { get; set; }
        /// <summary>
        /// 激光切割机2实时切割矩管类型
        /// </summary>
        public string TripleCurrentMaterialType { get; set; }
        #endregion

        #region 第四台设备
        /// <summary>
        /// 当前机器生产的订单号
        /// </summary>
        public string QuadraCurrentTaskNo { get; set; }
        /// <summary>
        /// 缓存位1托盘里矩管数
        /// </summary>
        public string QuadraCurrentOneAddressNumber { get; set; }

        /// <summary>
        /// 缓存位2托盘里矩管数
        /// </summary>
        public string QuadraCurrentTwoAddressNumber { get; set; }
        /// <summary>
        /// 缓存位1是否下料即呼叫托盘进行空满转换
        /// </summary>
        public string QuadraCurrentOneAddressPublish { get; set; }
        /// <summary>
        /// 缓存位2是否下料即呼叫托盘进行空满转换
        /// </summary>
        public string QuadraCurrentTwoAddressPublish { get; set; }
        /// <summary>
        /// 当前设备生产的产品类型
        /// </summary>
        public string QuadraCurrentProductType { get; set; }
        /// <summary>
        /// 缓存位1是否有托盘
        /// </summary>
        public string QuadraCurrentOneAddressHasTray { get; set; }
        /// <summary>
        /// 缓存位2是否有托盘
        /// </summary>
        public string QuadraCurrentTwoAddressHasTray { get; set; }
        /// <summary>
        /// 激光切割机4实时切割矩管类型
        /// </summary>
        public string QuadraCurrentMaterialType { get; set; }
        #endregion

        #region 第五台设备
        /// <summary>
        /// 当前机器生产的订单号
        /// </summary>
        public string PentaCurrentTaskNo { get; set; }
        /// <summary>
        /// 缓存位1托盘里矩管数
        /// </summary>
        public string PentaCurrentOneAddressNumber { get; set; }

        /// <summary>
        /// 缓存位2托盘里矩管数
        /// </summary>
        public string PentaCurrentTwoAddressNumber { get; set; }
        /// <summary>
        /// 缓存位1是否下料即呼叫托盘进行空满转换
        /// </summary>
        public string PentaCurrentOneAddressPublish { get; set; }
        /// <summary>
        /// 缓存位2是否下料即呼叫托盘进行空满转换
        /// </summary>
        public string PentaCurrentTwoAddressPublish { get; set; }
        /// <summary>
        /// 当前设备生产的产品类型
        /// </summary>
        public string PentaCurrentProductType { get; set; }
        /// <summary>
        /// 缓存位1是否有托盘
        /// </summary>
        public string PentaCurrentOneAddressHasTray { get; set; }
        /// <summary>
        /// 缓存位2是否有托盘
        /// </summary>
        public string PentaCurrentTwoAddressHasTray { get; set; }
        /// <summary>
        /// 激光切割机5实时切割矩管类型
        /// </summary>
        public string PentaCurrentMaterialType { get; set; }
        #endregion

        #region 第六台设备
        /// <summary>
        /// 当前机器生产的订单号
        /// </summary>
        public string HexaCurrentTaskNo { get; set; }
        /// <summary>
        /// 缓存位1托盘里矩管数
        /// </summary>
        public string HexaCurrentOneAddressNumber { get; set; }

        /// <summary>
        /// 缓存位2托盘里矩管数
        /// </summary>
        public string HexaCurrentTwoAddressNumber { get; set; }
        /// <summary>
        /// 缓存位1是否下料即呼叫托盘进行空满转换
        /// </summary>
        public string HexaCurrentOneAddressPublish { get; set; }
        /// <summary>
        /// 缓存位2是否下料即呼叫托盘进行空满转换
        /// </summary>
        public string HexaCurrentTwoAddressPublish { get; set; }
        /// <summary>
        /// 当前设备生产的产品类型
        /// </summary>
        public string HexaCurrentProductType { get; set; }
        /// <summary>
        /// 缓存位1是否有托盘
        /// </summary>
        public string HexaCurrentOneAddressHasTray { get; set; }
        /// <summary>
        /// 缓存位2是否有托盘
        /// </summary>
        public string HexaCurrentTwoAddressHasTray { get; set; }
        /// <summary>
        /// 激光切割机6实时切割矩管类型
        /// </summary>
        public string HexaCurrentMaterialType { get; set; }
        #endregion

        /// <summary>
        /// 当前切割机
        /// </summary>
        public List<CuttingMachine> CuttingMachines { get; set; } = new List<CuttingMachine>();
    }

    public class CuttingMachine
    {
        /// <summary>
        /// 切割机 1，2，3，4，5，6
        /// </summary>
        public int MachineNo { get; set; }
        /// <summary>
        /// 当前机器生产的订单号
        /// </summary>
        public string CurrentTaskNo { get; set; }
        /// <summary>
        /// 缓存位1托盘里矩管数
        /// </summary>
        public int CurrentOneAddressNumber { get; set; }

        /// <summary>
        /// 缓存位2托盘里矩管数
        /// </summary>
        public int CurrentTwoAddressNumber { get; set; }
        /// <summary>
        /// 缓存位1是否下料即呼叫托盘进行空满转换
        /// </summary>
        public bool CurrentOneAddressPublish { get; set; }
        /// <summary>
        /// 缓存位2是否下料即呼叫托盘进行空满转换
        /// </summary>
        public bool CurrentTwoAddressPublish { get; set; }
        /// <summary>
        /// 当前设备生产的产品类型
        /// </summary>
        public int CurrentProductType { get; set; }
        /// <summary>
        /// 缓存位1是否有托盘
        /// </summary>
        public bool CurrentOneAddressHasTray { get; set; }
        /// <summary>
        /// 缓存位2是否有托盘
        /// </summary>
        public bool CurrentTwoAddressHasTray { get; set; }
    }
}
