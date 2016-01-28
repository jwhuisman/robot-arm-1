using System.Collections.Generic;

namespace Assets.Models
{
    public class Section
    {
        public Section(int id)
        {
            Id = id;
        }

        public List<CubeStack> Stacks { get; set; }
        public int Id { get; set; }
    }
}
