using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PLCRead.Web.Models;
using System.Diagnostics;
using PLCRead.Core.Caching;
using PLCRead.Data.TeamUp;
using PLCRead.Data.Structural;

namespace PLCRead.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStaticCacheManager _redisManager;

        public HomeController(ILogger<HomeController> logger, IStaticCacheManager redisManager)
        {
            _logger = logger;
            _redisManager = redisManager;
        }

        public async Task<IActionResult> Index()
        {


            var completedDataRight = new List<CompletedMaterial>();
            var dataChild = new CompletedMaterial();
            dataChild.Allow = true;
            dataChild.StacksMax= 100;
            dataChild.DataModel = "12NC2";
            dataChild.StacksNumber = "1134";
            completedDataRight.Add(dataChild);
            var prefex = new string[] { "hangjialeftresult" };
            var cacheKey = new CacheKey("hangjialeftresult_001", prefex);
            await _redisManager.SetAsync<List<CompletedMaterial>>(cacheKey, completedDataRight);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Info()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetCheckData()
        {
            var prefex = new string[] { "teamupcheckdata" };
            var cacheKey = new CacheKey("teamupcheckdata_001", prefex);
            var checkData = _redisManager.GetAsync<List<string>>(cacheKey);
            var checkDataResultString = checkData.Result == null ? "" : JsonConvert.SerializeObject(checkData.Result);
            var prefexData = new string[] { "teamupproduct" };
            var cacheKeyData = new CacheKey("teamupproduct_001", prefexData);
            var checkDataData = _redisManager.GetAsync<List<MacheProduct>>(cacheKeyData);
            var checkDataResultStringData = checkDataData.Result == null ? "" : JsonConvert.SerializeObject(checkDataData.Result);


            var prefexCheck = new string[] { "structuralcheckdata" };
            var cacheCheckKey = new CacheKey("structuralcheckdata_001", prefexCheck);
            var structualcheckData2 = _redisManager.GetAsync<List<string>>(cacheCheckKey);
            var structualcheckDataResultString2 = structualcheckData2.Result == null ? "" : JsonConvert.SerializeObject(structualcheckData2.Result);


            var prefex2 = new string[] { "hangjialeftresult" };
            var cacheKey2 = new CacheKey("hangjialeftresult_001", prefex2);
            var checkData2 = _redisManager.GetAsync<List<CompletedMaterial>>(cacheKey2);
            var checkDataResultString2 = checkData2.Result == null ? "" : JsonConvert.SerializeObject(checkData2.Result);
            var prefexData2 = new string[] { "hangjiarightresult" };
            var cacheKeyData2 = new CacheKey("hangjiarightresult_002", prefexData2);
            var checkDataData2 = _redisManager.GetAsync<List<CompletedMaterial>>(cacheKeyData2);
            var checkDataResultStringData2 = checkDataData2.Result == null ? "" : JsonConvert.SerializeObject(checkDataData2.Result);


            var prefexEndCheck = new string[] { "productEndcheckdata" };
            var cacheEndCheckKey = new CacheKey("productEndcheckdata_001", prefexEndCheck);
            var structualEndcheckData2 = _redisManager.GetAsync<List<string>>(cacheEndCheckKey);
            var structualEndcheckDataResultString2 = structualEndcheckData2.Result == null ? "" : JsonConvert.SerializeObject(structualEndcheckData2.Result);


            var prefexEnd2 = new string[] { "xialiaonorthresult" };
            var cacheKeyEnd2 = new CacheKey("xialiaonorthresult_001", prefexEnd2);
            var checkDataEnd2 = _redisManager.GetAsync<ProductNorthEnd>(cacheKeyEnd2);
            var checkDataResultStringEnd2 = checkDataEnd2.Result == null ? "" : JsonConvert.SerializeObject(checkDataEnd2.Result);
            var prefexDataEnd2 = new string[] { "xialiaosouthresult" };
            var cacheKeyDataEnd2 = new CacheKey("xialiaosouthresult_001", prefexDataEnd2);
            var checkDataDataEnd2 = _redisManager.GetAsync<ProductSouthEnd>(cacheKeyDataEnd2);
            var checkDataResultStringDataEnd2 = checkDataDataEnd2.Result == null ? "" : JsonConvert.SerializeObject(checkDataDataEnd2.Result);




            var jsonResult = new JObject();
            jsonResult["s001"] = checkDataResultString;
            jsonResult["s002"] = checkDataResultStringData;
            jsonResult["s003"] = structualcheckDataResultString2;
            jsonResult["s004"] = checkDataResultString2;
            jsonResult["s005"] = checkDataResultStringData2;
            jsonResult["s006"] = structualEndcheckDataResultString2;
            jsonResult["s007"] = checkDataResultStringEnd2;
            jsonResult["s008"] = checkDataResultStringDataEnd2;
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        public async Task<IActionResult> GetCheckInfoData()
        {
            //贴板信息化读取数据
            var flitchPrefex = new string[] { "flitchcheckdata" };
            var flitchCacheKey = new CacheKey("flitchcheckdata_001", flitchPrefex);
            var flitchCheckData =await _redisManager.GetAsync<List<string>>(flitchCacheKey);
            var flitchCheckDataResultString = flitchCheckData == null ? "" : JsonConvert.SerializeObject(flitchCheckData);
            
            //组队信息化读取数据
            var teamupPrefexData = new string[] { "teamupcheckdata" };
            var teamupCacheKeyData = new CacheKey("teamupcheckdata_001", teamupPrefexData);
            var teamupCheckDataData =await _redisManager.GetAsync<List<string>>(teamupCacheKeyData);
            var teamupCheckDataResultStringData = teamupCheckDataData == null ? "" : JsonConvert.SerializeObject(teamupCheckDataData);

            //自动焊信息化读取数据
            var autoPrefexCheck = new string[] { "autocheckdata" };
            var autoCacheCheckKey = new CacheKey("autocheckdata_001", autoPrefexCheck);
            var autoStructualcheckData = _redisManager.GetAsync<List<string>>(autoCacheCheckKey);
            var autoStructualcheckDataResultString = autoStructualcheckData.Result == null ? "" : JsonConvert.SerializeObject(autoStructualcheckData.Result);

            var autoPrefexCheckError = new string[] { "autoerrordata" };
            var autoCacheCheckKeyError = new CacheKey("autoerrordata_001", autoPrefexCheckError);
            var autoStructualcheckDataError = _redisManager.GetAsync<List<string>>(autoCacheCheckKeyError);
            var autoStructualcheckDataResultStringError = autoStructualcheckDataError.Result == null ? "" : JsonConvert.SerializeObject(autoStructualcheckDataError.Result);


            //绗架信息化读取数据

            var trussPrefex2 = new string[] { "trusscheckdata" };
            var trussCacheKey2 = new CacheKey("trusscheckdata_001", trussPrefex2);
            var trussCheckData2 = _redisManager.GetAsync<List<string>>(trussCacheKey2);
            var trussCheckDataResultString2 = trussCheckData2.Result == null ? "" : JsonConvert.SerializeObject(trussCheckData2.Result);


            var jsonResult = new JObject();
            jsonResult["s001"] = flitchCheckDataResultString;
            jsonResult["s002"] = teamupCheckDataResultStringData;
            jsonResult["s003"] = autoStructualcheckDataResultString;
            jsonResult["s004"] = autoStructualcheckDataResultStringError;
            jsonResult["s005"] = trussCheckDataResultString2;

            return Content(JsonConvert.SerializeObject(jsonResult));
        }


    }
}