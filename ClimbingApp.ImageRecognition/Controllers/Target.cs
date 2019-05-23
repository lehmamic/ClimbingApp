using System;
using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class Target
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public Image ReferenceImage { get; set; }
    }
}
