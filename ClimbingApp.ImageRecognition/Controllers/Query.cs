﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class Query
    {
        [Required]
        public Image Image { get; set; }
    }
}
