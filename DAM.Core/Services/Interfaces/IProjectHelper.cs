using DAM.Core.Models.SolutionModels;
using System.Collections.Generic;

namespace DAM.Core.Services.Interfaces
{
    public interface IProjectHelper
    {
        List<Property> GetModelProperties(Model model);
    }
}
