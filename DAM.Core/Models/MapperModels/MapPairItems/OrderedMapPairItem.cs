using System;
using System.Collections.Generic;
using System.Text;
using DAM.Core.Models.SolutionModels;

namespace DAM.Core.Models.MapperModels.MapPairItems
{
    public class OrderedMapPairItem : MapPairItemBase
    {
        public Property DestinationProperty { get; set; }

        public int Order { get; set; }
    }
}
