using System;
using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class TargetSet
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string DisplayName { get; set; }
    }
}
