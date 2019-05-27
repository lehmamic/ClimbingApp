﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ClimbingApp.ImageRecognition.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClimbingApp.ImageRecognition.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        private readonly IImageRecognitionService imageRecognition;
        private readonly IMapper mapper;

        public TargetsController(IImageRecognitionService imageRecognition, IMapper mapper)
        {
            this.imageRecognition = imageRecognition ?? throw new ArgumentNullException(nameof(imageRecognition));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TargetResponse>>> GetTargets()
        {
            IEnumerable<Target> targets = await this.imageRecognition.GetTargets("climbing-routes-1", 0, 10);
            IEnumerable<TargetResponse> response = this.mapper.Map<IEnumerable<TargetResponse>>(targets);

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TargetResponse>> GetTarget(string id)
        {
            Target target = await this.imageRecognition.GetTarget(id);
            TargetResponse response = this.mapper.Map<TargetResponse>(target);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateTarget([FromBody]TargetRequest target)
        {
            await this.imageRecognition.CreateTarget(
                target.Id,
                target.DisplayName,
                Convert.FromBase64String(target.ReferenceImage.Base64));

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteTarget(string id)
        {
            await this.imageRecognition.DeleteTarget("climbing-routes-1", id);
            return NoContent();
        }
    }
}