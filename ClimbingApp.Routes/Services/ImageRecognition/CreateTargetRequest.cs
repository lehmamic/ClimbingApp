using System.Collections.Generic;

namespace ClimbingApp.Routes.Services.ImageRecognition
{
    public class CreateTargetRequest
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public IReadOnlyDictionary<string, string> Labels { get; set; }

        public Image ReferenceImage { get; set; }
    }
}
