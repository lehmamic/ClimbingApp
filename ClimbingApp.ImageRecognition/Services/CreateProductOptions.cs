namespace ClimbingApp.ImageRecognition.Services
{
    internal class CreateProductOptions
    {
        public string ProjectID { get; internal set; }
        public string ComputeRegion { get; internal set; }
        public string DisplayName { get; internal set; }
        public string ProductCategory { get; internal set; }
        public string ProductID { get; internal set; }
    }
}