using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ClimbingApp.ImageRecognition.Services;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Google.Cloud.Vision.V1;
using Grpc.Auth;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace ClimbingApp.ImageRecognition.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageRecognitionController : ControllerBase
    {
        private readonly IImageRecognitionService imageRecognition;

        public ImageRecognitionController(IImageRecognitionService imageRecognition)
        {
            this.imageRecognition = imageRecognition ?? throw new ArgumentNullException(nameof(imageRecognition));
        }

        [HttpPost("productset")]
        public async Task<ActionResult> CreateProductSet([FromBody]TargetSet targetSet)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            //var options = new CreateProductSetsOptions
            //{
            //    ProjectID = "climbingapp-241211",
            //    ComputeRegion = "europe-west1",
            //    ProductSetId = "climbing-routes-1",
            //    ProductSetDisplayName = "ClimbingRoutes",
            //};
            await this.imageRecognition.CreateTargetSet(targetSet.Id, targetSet.DisplayName);
            return Ok();
        }
    }
}
