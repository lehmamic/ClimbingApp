using System.Collections.Generic;

namespace ClimbingApp.ImageRecognition.Services
{
    internal class CreateProductOptions
    {
        public string ProjectID { get; set; }

        public string ComputeRegion { get; set; }

        public string DisplayName { get; set; }

        public string ProductCategory { get; set; }

        public string ProductID { get; set; }

        public IReadOnlyDictionary<string, string> ProductLabels { get; set; }

        public string Description { get; set; }
    }
}