using DAM.Core.Models.SolutionModels;

namespace DAM.Core.Models.MapperModels.MapPairItems
{
    public class IgnoreMapPairItem : MapPairItemBase
    {
        public Property DestinationProperty { get; set; }
        public override string GetMapExpression()
            => $".ForMember(dest => dest.{DestinationProperty.Name}, opt => opt.Ignore())";
    }
}
