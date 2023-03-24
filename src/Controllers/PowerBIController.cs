using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authorization;

namespace cloudscribe_identity_demo.Controllers
{
    public class PowerBIController : Controller
    {
        //[Authorize(Policy = "PowerBiAccessPolicy")]
        [HttpGet]
        [Route("powerbi")]
        [Route("{sitefolder:sitefolder}/powerbi")]
        public IActionResult Index()
        {   
            return View();
        }
    }
}
