using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;

namespace ClimbingApp.Routes.Controllers
{
    [Route("api/sites/{siteId}/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IAsyncDocumentSession documentSession;

        public RoutesController(IAsyncDocumentSession documentSession)
        {
            this.documentSession = documentSession ?? throw new System.ArgumentNullException(nameof(documentSession));
        }

        [HttpGet]
        public ActionResult<IEnumerable<ClimbingRouteResponse>> Get([FromRoute]string siteId)
        {
            return new ClimbingRouteResponse[] { };
        }

        [HttpGet("{id}")]
        public ActionResult<ClimbingRouteResponse> Get([FromRoute]int id)
        {
            return null;
        }

        [HttpPost]
        public async Task Post([FromRoute]string siteId, [FromBody] CreateClimbingRouteRequest value)
        {
            await this.documentSession.StoreAsync(value);
        }

        [HttpPut("{id}")]
        public void Put([FromRoute]string siteId, [FromRoute]int id, [FromBody] ClimbingRouteResponse value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete([FromRoute]string siteId, [FromRoute]int id)
        {
        }
    }
}
