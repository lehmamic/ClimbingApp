using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class CreateTargetRequest
    {
        [Required]
        public string DisplayName { get; set; }

        public string Description { get; set; }

        [Required]
        public Image ReferenceImage { get; set; }

        public IReadOnlyDictionary<string, string> Labels { get; set; }
    }
}
