using DAM.Core.Models.MapperModels.MapPairItems;
using DAM.Core.Models.SolutionModels;
using System;
using System.Collections.Generic;

namespace DAM.Core.Models.MapperModels
{
    public class MapPair
    {
        public string Id { get; set; }
        public Model SourceModel { get; set; }
        public Model DestinationModel { get; set; }
        public List<MapPairItemBase> MapItems { get; set; }

        public MapPair()
        {
            Id = Guid.NewGuid().ToString();
            MapItems = new List<MapPairItemBase>();
        }
    }
}
