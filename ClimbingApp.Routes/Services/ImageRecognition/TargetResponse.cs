﻿using System.Collections.Generic;

namespace ClimbingApp.Routes.Services.ImageRecognition
{
    public class TargetResponse
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public IReadOnlyDictionary<string, string> Labels { get; set; }
    }
}
