using System.Collections.Generic;

namespace DAM.Core.Models.SolutionModels
{
    public class Solution
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<Item> Items { get; set; }
    }
}
