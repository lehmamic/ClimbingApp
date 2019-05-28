using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ClimbingApp.Routes.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace ClimbingApp.Routes.Controllers.ClimbingSites
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SitesController : ControllerBase
    {
        private readonly IAsyncDocumentSession documentSession;
        private readonly IMapper mapper;

        public SitesController(IAsyncDocumentSession documentSession, IMapper mapper)
        {
            this.documentSession = documentSession ?? throw new ArgumentNullException(nameof(documentSession));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ClimbingSiteResponse>>> Get()
        {
            IEnumerable<ClimbingSite> sites = await this.documentSession.Query<ClimbingSite>()
                .ToArrayAsync();

            if (sites == null)
            {
                return NotFound();
            }

            var response = this.mapper.Map<IEnumerable<ClimbingSiteResponse>>(sites);
            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetClimbingSite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClimbingSiteResponse>> Get([FromRoute]string id)
        {
            ClimbingSite site = await this.documentSession.LoadAsync<ClimbingSite>(id);
            if (site == null)
            {
                return NotFound();
            }

            var response = this.mapper.Map<ClimbingSiteResponse>(site);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClimbingSiteResponse>> Post([FromBody] CreateClimbingSiteRequest request)
        {
            ClimbingSite site = this.mapper.Map<ClimbingSite>(request);
            await this.documentSession.StoreAsync(site);
            await this.documentSession.SaveChangesAsync();

            var response = this.mapper.Map<ClimbingSiteResponse>(site);
            return CreatedAtRoute("GetClimbingSite", new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put([FromRoute]string id, [FromBody] UpdateClimbingSiteRequest request)
        {
            ClimbingSite site = await this.documentSession.LoadAsync<ClimbingSite>(id);
            if (site == null)
            {
                return NotFound();
            }

            this.mapper.Map(request, site);
            await this.documentSession.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute]string id)
        {
            ClimbingSite site = await this.documentSession.LoadAsync<ClimbingSite>(id);
            if (site == null)
            {
                return NoContent();
            }

            this.documentSession.Delete(site);
            await this.documentSession.SaveChangesAsync();

            return NoContent();
        }
    }
}
