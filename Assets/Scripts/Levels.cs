using Assets.Models.WorldData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class Levels
    {
        // stackMax is not the amount of stacks
        // it's the stacks placed before and after (x = 0)
        // when stackMax = 3 that means there are 7 stacks
        // -3, -2, -1, 0, 1, 2, 3
        public static int stackMax = 1000;

        public List<BlockStack> LoadLevel1()
        {
            List<BlockStack> stacks = new List<BlockStack>();
            for (int x = -stackMax; x <= stackMax; x++)
            {
                stacks.Add(new BlockStack(x));
            }

            Block block = new Block("0", "Red", 0, 0);

            stacks[stackMax].Blocks.Push(block);

            return stacks;
        }
        public List<BlockStack> LoadLevel2()
        {
            List<BlockStack> stacks = new List<BlockStack>();
            for (int x = -stackMax; x <= stackMax; x++)
            {
                stacks.Add(new BlockStack(x));
            }

            Block block = new Block("0", "Red", 0, 0);
            Block block2 = new Block("1", "Green", 0, 1);
            Block block3 = new Block("2", "Blue", 0, 2);
            Block block4 = new Block("3", "White", 0, 3);

            stacks[stackMax + 3].Blocks.Push(block);
            stacks[stackMax + 3].Blocks.Push(block2);
            stacks[stackMax + 3].Blocks.Push(block3);
            stacks[stackMax + 3].Blocks.Push(block4);

            return stacks;
        }
        public List<BlockStack> GenerateRandomLevel()
        {
            List<BlockStack> stacks = new List<BlockStack>();
            for (int x = -stackMax; x <= stackMax; x++)
            {
                stacks.Add(GenerateStack(x));
            }

            return stacks;
        }

        public BlockStack GenerateStack(int x)
        {
            BlockStack stack = new BlockStack(x);

            int cubesAmount = rnd.Next(minCubes, maxCubes + 1);

            for (int i = 0; i < cubesAmount; i++)
            {
                string id = "(" + x + "/" + i + ")";
                string color = ((ColorEnum.Colors)rnd.Next(0, 4)).ToString();
                int y = stack.Blocks.Count();

                stack.Blocks.Push(new Block(id, color, x, y));
            }

            return stack;
        }

        private Random rnd = new Random();
        private int minCubes = 1;
        private int maxCubes = 6;
    }
}
