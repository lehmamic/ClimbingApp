using System;
using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class Target
    {
        [MinLength(1)]
        [MaxLength(100)]
        public string Id { get; set; }

        [MinLength(1)]
        [MaxLength(100)]
        public string DisplayName { get; set; }

        [Required]
        public Image ReferenceImage { get; set; }
    }
}
