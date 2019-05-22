namespace ClimbingApp.ImageRecognition.Services
{
    internal class CreateReferenceImageOptions
    {
        public string ProjectID { get; internal set; }
        public string ComputeRegion { get; internal set; }
        public string ProductID { get; internal set; }
        public string ReferenceImageID { get; internal set; }
        public string ReferenceImageURI { get; internal set; }
    }
}