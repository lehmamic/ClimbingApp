using System;
using System.Threading.Tasks;
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

        public QueryController(IImageRecognitionService imageRecognition)
        {
            this.imageRecognition = imageRecognition ?? throw new ArgumentNullException(nameof(imageRecognition));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> QueryTargets([FromBody]Query query)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            byte[] imageBinaries = Convert.FromBase64String(query.Image.Base64);

            try
            {
                QueryResult result = await this.imageRecognition.QuerySimilarTargets(imageBinaries);

                return Ok(result);
            }
            catch(TargetNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
