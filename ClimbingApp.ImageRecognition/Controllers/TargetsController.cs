using System;
using System.Threading.Tasks;
using ClimbingApp.ImageRecognition.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClimbingApp.ImageRecognition.Controllers
{
    [Route("api/vs/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        private readonly IImageRecognitionService imageRecognition;

        public TargetsController(IImageRecognitionService imageRecognition)
        {
            this.imageRecognition = imageRecognition ?? throw new ArgumentNullException(nameof(imageRecognition));
        }

        // POST api/targets
        [HttpPost()]
        public async Task<ActionResult> CreateTarget([FromBody]Target target)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.imageRecognition.CreateTarget(
                target.Id,
                target.DisplayName,
                Convert.FromBase64String(target.ReferenceImage.Base64));

            return NoContent();
        }
    }
}