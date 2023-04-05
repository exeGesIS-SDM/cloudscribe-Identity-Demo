using cloudscribe_identity_demo.EmbedWeb;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace cloudscribe_identity_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PowerBIEmbedController : ControllerBase
    {
        [HttpGet]
        public async Task<EmbedInfo> Get()
        {
            Guid workspaceId = Guid.Parse("2ba8e729-5be5-4d5d-bfb1-8f591e6cf1cf");  
            Guid reportId = Guid.Parse("c81a9dc0-4df1-469d-8333-9852d18360c6");

            var embedInfo = await Embedder.GetEmbedInfo(workspaceId, reportId);
            return embedInfo;
        }
    }
}
