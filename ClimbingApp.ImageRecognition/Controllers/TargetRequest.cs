using System;
using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class TargetRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public Image ReferenceImage { get; set; }
    }
}
