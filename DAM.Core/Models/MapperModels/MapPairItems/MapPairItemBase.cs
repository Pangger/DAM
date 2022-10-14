using System;
using System.Collections.Generic;
using System.Text;

namespace DAM.Core.Models.MapperModels.MapPairItems
{
    public abstract class MapPairItemBase
    {
        public string Id { get; }

        public bool IsReverse { get; set; }

        protected MapPairItemBase()
        {
            Id = Guid.NewGuid().ToString();
            IsReverse = false;
        }

        public abstract string GetMapExpression();
    }
}
