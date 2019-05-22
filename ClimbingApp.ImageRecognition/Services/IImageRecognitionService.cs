using System;
using System.Threading.Tasks;

namespace ClimbingApp.ImageRecognition.Services
{
    public interface IImageRecognitionService
    {
        Task CreateTargetSet(string targetSetId, string displayName);

        Task CreateTarget(string targetId, string displayName, byte[] referenceImage);

        Task<QueryResult> QuerySimilarTargets(byte[] image);
    }
}
