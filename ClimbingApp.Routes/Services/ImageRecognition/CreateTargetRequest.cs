namespace ClimbingApp.Routes.Services.ImageRecognition
{
    public class CreateTargetRequest
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public Image ReferenceImage { get; set; }
    }
}
