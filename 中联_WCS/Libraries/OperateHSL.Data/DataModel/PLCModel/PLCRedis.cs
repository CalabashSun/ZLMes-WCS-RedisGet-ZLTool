using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.PLCModel
{
  public  class PLCRedis
    {
        public int ContainerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StacksMax { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StacksNow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool MaxFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Allow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Alone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Model { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Number { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string DataModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StacksNumber { get; set; }

       
    }
}
