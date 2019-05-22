namespace ClimbingApp.ImageRecognition.Services
{
    internal class CreateProductSetsOptions
    {
        public string ProjectID { get; internal set; }
        public string ComputeRegion { get; internal set; }
        public string ProductSetId { get; internal set; }
        public string ProductSetDisplayName { get; internal set; }
    }
}