﻿namespace ClimbingApp.ImageRecognition.Services
{
    internal class AddProductToProductSetOptions
    {
        public string ProjectID { get; internal set; }
        public string ComputeRegion { get; internal set; }
        public string ProductID { get; internal set; }
        public string ProductSetId { get; internal set; }
    }
}