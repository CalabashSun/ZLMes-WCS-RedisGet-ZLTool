using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.PLC
{
    public class PLCPipeCacheData
    {

        /// <summary>
        /// 1号切割机缓存矩管型号
        /// </summary>
        public int SpliceOneCacheProduct { get; set; }
        /// <summary>
        /// 1号切割机缓存矩管数量
        /// </summary>
        public int SpliceOneCacheNumber { get; set; }

        #region 2，3，4，5，6
        public int SpliceTwoCacheProduct { get; set; }
        public int SpliceTwoCacheNumber { get; set; }
        public int SpliceThreeCacheProduct { get; set; }
        public int SpliceThreeCacheNumber { get; set; }
        public int SpliceFourCacheProduct { get; set; }
        public int SpliceFourCacheNumber { get; set; }
        public int SpliceFiveCacheProduct { get; set; }
        public int SpliceFiveCacheNumber { get; set; }
        public int SpliceSixCacheProduct { get; set; }
        public int SpliceSixCacheNumber { get; set; }
        #endregion

        /// <summary>
        /// 立库1号缓存区缓存的物料数量
        /// </summary>
        public int BankOneCacheNumber { get; set; }
        /// <summary>
        /// 立库2号缓存区缓存的物料数量
        /// </summary>
        public int BankTwoCacheNumber { get; set; }
    }
}
