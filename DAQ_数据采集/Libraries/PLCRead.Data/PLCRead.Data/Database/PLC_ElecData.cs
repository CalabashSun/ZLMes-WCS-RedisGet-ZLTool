using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Data.Database
{
    public class PLC_ElecData
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal APhaseA { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal APhaseV { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Ass_create_user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Ass_update_user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal BPhaseA { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal BPhaseV { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal CPhaseA { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal CPhaseV { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Creatername { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int HourData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        public int PlcHex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IpDesc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IpPosition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsDeleted { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal TotalElec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal UnusefulElec { get; set; }

        public decimal UsefulElec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateDt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UpdaterID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Updatername { get; set; }
    }
}
