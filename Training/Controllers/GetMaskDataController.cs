using Training.Models.V2.Domain;
using Training.Models.V2.Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Training.Controllers
{
    public class GetMaskDataController : Controller
    {
        private readonly IGetMaskDataDomain _domain;

        public GetMaskDataController(IGetMaskDataDomain domain)
        {
            _domain = domain;
        }

        [HttpGet]
        public async Task<IActionResult> GetMaskData()
        {
            var result = await _domain.GetMaskData();
            return Json(result);
        }

        [HttpGet]
        public IActionResult GetRoadData()
        {
            GetRoadDataDomain getRoadDataDomain = new GetRoadDataDomain();
            return Json(getRoadDataDomain.GetRoadData());
        }
    }
}
