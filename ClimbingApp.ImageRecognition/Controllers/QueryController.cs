using System;
using System.Threading.Tasks;
using AutoMapper;
using ClimbingApp.ImageRecognition.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClimbingApp.ImageRecognition.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly IImageRecognitionService imageRecognition;
        private readonly IMapper mapper;

        public QueryController(IImageRecognitionService imageRecognition, IMapper mapper)
        {
            this.imageRecognition = imageRecognition ?? throw new ArgumentNullException(nameof(imageRecognition));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> QueryTargets([FromBody]QueryRequest query)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            byte[] imageBinaries = Convert.FromBase64String(query.Image.Base64);
            TargetSearchResults results = await this.imageRecognition.QuerySimilarTargets(imageBinaries);
            QueryResponse response = this.mapper.Map<QueryResponse>(results);

            return Ok(response);
        }
    }
}
