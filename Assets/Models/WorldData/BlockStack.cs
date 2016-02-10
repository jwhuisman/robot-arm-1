using System.Collections.Generic;

namespace Assets.Models.WorldData
{
    public class BlockStack
    {
        public BlockStack(int id)
        {
            Blocks = new Stack<Block>();
            Id = id;
        }

        public Stack<Block> Blocks { get; set; }
        public int Id { get; set; }
    }
}