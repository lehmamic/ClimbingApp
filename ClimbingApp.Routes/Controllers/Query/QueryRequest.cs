using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.Routes.Controllers.Query
{
    public class QueryRequest
    {
        [Required]
        public Image Image { get; set; }
    }
}
