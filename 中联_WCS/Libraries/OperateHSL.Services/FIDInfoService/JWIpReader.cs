using JW.UHF;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Services.FIDInfoService
{
    public class JWIpReader
    {
        public JWReader jwReader { get; set; } = new JWReader();

        public string ipAddress { get; set; }
    }
}
