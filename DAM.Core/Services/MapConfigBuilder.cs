using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DAM.Core.Models.MapperModels;
using DAM.Core.Models.MapperModels.MapPairItems;
using DAM.Core.Models.SolutionModels;
using DAM.Core.Services.Interfaces;

namespace DAM.Core.Services
{
    public class MapConfigBuilder : IMapConfigBuilder
    {
        private List<MapPair> _mapPairs;

        public IMapConfigBuilder AddMapPairs(List<MapPair> mapPairs)
        {
            _mapPairs = mapPairs;
            return this;
        }

        public string BuildMapConfig()
        {
            StringBuilder mapConfig = new StringBuilder();

            foreach (var mapPair in _mapPairs)
            {
                mapConfig.Append(GetMapPair(mapPair));
                mapConfig.Append(Environment.NewLine);
                mapConfig.Append(Environment.NewLine);
            }

            return mapConfig.ToString();
        }

        private string GetMapPair(MapPair mapPair)
        {
            StringBuilder mapConfig = new StringBuilder();
            bool isReverseMapPlaced = false;

            mapConfig.Append($"CreateMap<{Path.GetFileNameWithoutExtension(mapPair.SourceModel.Name)}, {Path.GetFileNameWithoutExtension(mapPair.DestinationModel.Name)}>()");

            foreach (var mapItem in mapPair.MapItems.OrderBy(w => w.IsReverse))
            {
                if (!isReverseMapPlaced && mapItem.IsReverse)
                {
                    mapConfig.Append(Environment.NewLine);
                    mapConfig.Append(".ReverseMap()");
                    isReverseMapPlaced = true;
                }
                mapConfig.Append(Environment.NewLine);
                mapConfig.Append(mapItem.GetMapExpression());
            }

            mapConfig.Append(";");

            return mapConfig.ToString();
        }
    }
}
