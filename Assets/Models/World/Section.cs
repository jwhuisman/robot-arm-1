using System.Collections.Generic;

namespace Assets.Models.World
{
    public class Section
    {
        public Section(int id)
        {
            Stacks = new List<CubeStack>();
            Id = id;
        }
        public Section(int id, List<CubeStack> stacks)
        {
            Stacks = stacks;
            Id = id;
        }


        public List<CubeStack> Stacks { get; set; }
        public int Id { get; set; }
    }
}
