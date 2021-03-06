using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimbingApp.ImageRecognition.Services
{
    public interface IImageRecognitionService
    {
        Task CreateTargetSet(string targetSetId, string displayName);

        Task<IEnumerable<Target>> GetTargets(string targetSetId, int page, int pageSize);

        Task<Target> GetTarget(string targetId);

        Task<Target> CreateTarget(string displayName, string description, IReadOnlyDictionary<string, string> labels, byte[] referenceImageBinaries);

        Task DeleteTarget(string targetSetId, string targetId);

        Task<TargetSearchResults> QuerySimilarTargets(byte[] image);
    }
}
