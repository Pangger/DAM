using System.Collections.Generic;

namespace DAM.Core.Models.SolutionModels
{
    public class ProjectDirectory : Item
    {
        public List<Item> Items { get; set; }

        public ProjectDirectory()
        {
            Items = new List<Item>();
        }
    }
}
