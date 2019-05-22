using System;
using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class TargetSet
    {
        [MinLength(1)]
        [MaxLength(100)]
        public string Id { get; set; }

        [MinLength(1)]
        [MaxLength(100)]
        public string DisplayName { get; set; }
    }
}
