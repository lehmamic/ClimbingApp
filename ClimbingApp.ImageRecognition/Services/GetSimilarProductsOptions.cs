namespace ClimbingApp.ImageRecognition.Services
{
    internal class GetSimilarProductsOptions
    {
        public string FilePath { get; internal set; }
        public string ProjectID { get; internal set; }
        public string ComputeRegion { get; internal set; }
        public string ProductSetId { get; internal set; }
        public string ProductCategory { get; internal set; }
        public string Filter { get; internal set; }
        public byte[] ImageBinaries { get; internal set; }
    }
}