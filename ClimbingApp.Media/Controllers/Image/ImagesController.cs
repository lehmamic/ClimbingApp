using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;

namespace ClimbingApp.Media.Controllers.Image
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IAsyncDocumentSession documentSession;
        private readonly IMapper mapper;

        public ImagesController(IAsyncDocumentSession documentSession, IMapper mapper)
        {
            this.documentSession = documentSession ?? throw new ArgumentNullException(nameof(documentSession));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<ActionResult> CreateImage([FromBody]CreateImageRequest request)
        {
            var image = this.mapper.Map<Entities.Image>(request);
            await this.documentSession.StoreAsync(image);
            await this.documentSession.SaveChangesAsync();

            return this.CreatedAtRoute("GetImage", new { id = request.Name }, null);
        }

        [HttpGet("{id}", Name = "GetImage")]
        public async Task<ActionResult<byte[]>> GetImage(string id)
        {
            var image = await this.documentSession.LoadAsync<Entities.Image>($"{nameof(Image)}s/{id}");
            if(image == null)
            {
                return NotFound();
            }

            return File(Convert.FromBase64String(image.Base64), "image/jpeg");
        }
    }
}
