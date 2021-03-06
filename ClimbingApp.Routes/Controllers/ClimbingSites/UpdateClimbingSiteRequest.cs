﻿using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.Routes.Controllers.ClimbingSites
{
    public class UpdateClimbingSiteRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
