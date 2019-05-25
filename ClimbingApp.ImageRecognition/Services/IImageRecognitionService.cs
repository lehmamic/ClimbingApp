using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimbingApp.ImageRecognition.Services
{
    public interface IImageRecognitionService
    {
        Task CreateTargetSet(string targetSetId, string displayName);

        Task<IEnumerable<Target>> GetTargets(string targetSetId, int page, int pageSize);

        Task CreateTarget(string targetId, string displayName, byte[] referenceImage);

        Task<QueryResult> QuerySimilarTargets(byte[] image);
    }
}
