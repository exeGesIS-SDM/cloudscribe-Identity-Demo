using cloudscribe_identity_demo.EmbedWeb;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;

namespace cloudscribe_identity_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PowerBIEmbedController : ControllerBase
    {
        [HttpGet]
        public async Task<EmbedInfo> Get()
        {
            Guid workspaceId = Guid.Parse("81f02fe5-c972-4a69-ab29-b13fbf0a12e3");  // officer >> open cases ?
            Guid reportId = Guid.Parse("dcdca343-2314-433e-803c-276de8eb661f");

            var embedInfo = await Embedder.GetEmbedInfo(workspaceId, reportId);
            return embedInfo;
        }
    }
}
