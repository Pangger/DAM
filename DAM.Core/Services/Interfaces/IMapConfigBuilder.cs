using System;
using System.Collections.Generic;
using System.Text;
using DAM.Core.Models.MapperModels;
using DAM.Core.Models.SolutionModels;

namespace DAM.Core.Services.Interfaces
{
    public interface IMapConfigBuilder
    {
        IMapConfigBuilder AddMapPairs(List<MapPair> mapPairs);
        string BuildMapConfig();
    }
}
