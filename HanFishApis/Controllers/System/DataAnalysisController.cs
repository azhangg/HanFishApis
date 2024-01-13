using HanFishApis.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Sysyem;

namespace HanFishApis.Controllers.System
{
    public class DataAnalysisController : BaseController
    {
        private readonly IDataAnalysisService _dataAnalysisService;

        public DataAnalysisController(IDataAnalysisService dataAnalysisService)
        {
            _dataAnalysisService = dataAnalysisService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDataAnalysisAsync()
        {
            var result = await _dataAnalysisService.GetDataAnalysisAsync();
            return JsonSuccess(result);
        }
    }
}
