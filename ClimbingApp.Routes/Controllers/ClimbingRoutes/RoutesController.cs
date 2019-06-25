using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClimbingApp.Routes.Entities;
using ClimbingApp.Routes.Services.ImageRecognition;
using ClimbingApp.Routes.Services.Media;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;

namespace ClimbingApp.Routes.Controllers.ClimbingRoutes
{
    [Route("api/v1/sites/{siteId}/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IAsyncDocumentSession documentSession;
        private readonly IMapper mapper;
        private readonly IImageRecognitionApiClient imageRecognitionApi;
        private readonly IMediaApiClient mediaApi;

        public RoutesController(
            IAsyncDocumentSession documentSession,
            IMapper mapper,
            IImageRecognitionApiClient imageRecognitionApi,
            IMediaApiClient mediaApi)
        {
            this.documentSession = documentSession ?? throw new ArgumentNullException(nameof(documentSession));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.imageRecognitionApi = imageRecognitionApi ?? throw new ArgumentNullException(nameof(imageRecognitionApi));
            this.mediaApi = mediaApi ?? throw new ArgumentNullException(nameof(mediaApi));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ClimbingRouteResponse>>> Get([FromRoute]string siteId)
        {
            ClimbingSite site = await this.documentSession.LoadAsync<ClimbingSite>(siteId);
            if (site == null)
            {
                return NotFound();
            }

            var response = this.mapper.Map<IEnumerable<ClimbingRouteResponse>>(site.Routes);
            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetClimbingRoute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClimbingRouteResponse>> Get([FromRoute]string siteId, [FromRoute]string id)
        {
            ClimbingSite site = await this.documentSession.LoadAsync<ClimbingSite>(siteId);
            if (site == null)
            {
                return NotFound();
            }

            ClimbingRoute route = site.Routes.FirstOrDefault(r => string.Equals(r.Id, id, StringComparison.InvariantCultureIgnoreCase));
            if (route == null)
            {
                return NotFound();
            }

            var response = this.mapper.Map<ClimbingRouteResponse>(route);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClimbingRouteResponse>> Post([FromRoute]string siteId, [FromBody] CreateClimbingRouteRequest request)
        {
            ClimbingSite site = await this.documentSession.LoadAsync<ClimbingSite>(siteId);
            if (site == null)
            {
                return NotFound();
            }

            ClimbingRoute route = this.mapper.Map<ClimbingRoute>(request);
            route.ImageUri = $"api/v1/images/{route.Id}";
            site.Routes.Add(route);

            await this.documentSession.SaveChangesAsync();

            // TODO create the task with a queue or async task handler
            var labels = new Dictionary<string, string>
            {
                { ClimbingRoutesConstants.CLIMBING_ROUTE_ID_LABEL, route.Id },
            };
            await this.imageRecognitionApi.CreateTarget(request.Name, request.Description, labels, request.Image.Base64);
            await this.mediaApi.UploadImage(route.Id, request.Image.Base64);

            var response = this.mapper.Map<ClimbingRouteResponse>(route);
            return CreatedAtRoute("GetClimbingRoute", new { siteId, id = response.Id }, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put([FromRoute]string siteId, [FromRoute]string id, [FromBody] UpdateClimbingRouteRequest request)
        {
            ClimbingSite site = await this.documentSession.LoadAsync<ClimbingSite>(siteId);
            if (site == null)
            {
                return NotFound();
            }

            ClimbingRoute route = site.Routes.FirstOrDefault(r => string.Equals(r.Id, id, StringComparison.InvariantCultureIgnoreCase));
            if (route == null)
            {
                return NotFound();
            }

            this.mapper.Map(request, route);
            await this.documentSession.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute]string siteId, [FromRoute]string id)
        {
            ClimbingSite site = await this.documentSession.LoadAsync<ClimbingSite>(siteId);
            if (site == null)
            {
                return NoContent();
            }

            ClimbingRoute route = site.Routes.FirstOrDefault(r => string.Equals(r.Id, id, StringComparison.InvariantCultureIgnoreCase));
            if (route != null)
            {
                site.Routes.Remove(route);
                await this.documentSession.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}
