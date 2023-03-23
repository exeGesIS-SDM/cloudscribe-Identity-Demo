using Microsoft.AspNetCore.Mvc;

namespace cloudscribe_identity_demo.Controllers
{
    public class PowerBIController : Controller
    {
        [HttpGet]
        [Route("powerbi")]
        [Route("{sitefolder:sitefolder}/powerbi")]
        public IActionResult Index()
        {   
            return View();
        }
    }
}
