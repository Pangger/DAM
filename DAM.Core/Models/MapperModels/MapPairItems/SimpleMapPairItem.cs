using System;
using System.Collections.Generic;
using System.Text;
using DAM.Core.Models.SolutionModels;

namespace DAM.Core.Models.MapperModels.MapPairItems
{
    public class SimpleMapPairItem : MapPairItemBase
    {
        public Property SourceProperty { get; set; }
        public Property DestinationProperty { get; set; }
    }
}
