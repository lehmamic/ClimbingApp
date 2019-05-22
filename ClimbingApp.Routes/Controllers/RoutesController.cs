using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ClimbingApp.Routes.Entities;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;

namespace ClimbingApp.Routes.Controllers
{
    [Route("api/sites/{siteId}/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IAsyncDocumentSession documentSession;
        private readonly IMapper mapper;

        public RoutesController(IAsyncDocumentSession documentSession, IMapper mapper)
        {
            this.documentSession = documentSession ?? throw new System.ArgumentNullException(nameof(documentSession));
            this.mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<ClimbingRouteResponse>> Get([FromRoute]string siteId)
        {
            return new ClimbingRouteResponse[] { };
        }

        [HttpGet("{id}", Name = "GetClimbingRoute")]
        public ActionResult<ClimbingRouteResponse> Get([FromRoute]string siteId, [FromRoute]int id)
        {
            return null;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromRoute]string siteId, [FromBody] CreateClimbingRouteRequest value)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            ClimbingSite site = await this.documentSession.LoadAsync<ClimbingSite>(siteId);
            if (site == null)
            {
                return NotFound();
            }

            ClimbingRoute route = this.mapper.Map<ClimbingRoute>(value);
            site.Routes.Add(route);

            await this.documentSession.SaveChangesAsync();

            return CreatedAtRoute("GetClimbingRoute", new { siteId, id = route.Id }, route);
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
