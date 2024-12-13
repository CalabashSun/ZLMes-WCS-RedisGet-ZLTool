using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OperateHSL.Web.Controllers
{
    /// <summary>
    /// 立体库接口交互
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class StereoscopicController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}