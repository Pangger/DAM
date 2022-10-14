using DAM.Core.Models.SolutionModels;
using System.Threading.Tasks;

namespace DAM.Core.Services.Interfaces
{
    public interface ISolutionProcessorService
    {
        Task<Solution> GetSolutionProjects(string dllPath);
    }
}
