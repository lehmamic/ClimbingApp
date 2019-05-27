using System;
using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class QueryRequest
    {
        [Required]
        public Image Image { get; set; }
    }
}
