using Assets.Models.WorldData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assets.Scripts
{
    public class Levels
    {
        // level save/load stuff
        // ----------
        // each line is a stack
        // 'red blue white' => red block at the bottom


        // stackMax is not the amount of stacks
        // it's the stacks placed before and after (x = 0)
        // when stackMax = 3 that means there are 7 stacks (-3, -2, -1, 0, 1, 2, 3)
        // so the stack amount = stackMax * 2 + 1
        public static int stackMax;

        public List<BlockStack> LoadLevel1()
        {
            return ParseFile(path + "level1.txt");
        }
        public List<BlockStack> LoadLevel2()
        {
            return ParseFile(path + "level2.txt");
        }
        public List<BlockStack> LoadLevel3()
        {
            return ParseFile(path + "level3.txt");
        }

        public List<BlockStack> GenerateRandomLevel()
        {
            stackMax = defaultStackMax;

            List<BlockStack> stacks = new List<BlockStack>();
            for (int x = -stackMax; x <= stackMax; x++)
            {
                stacks.Add(GenerateStack(x, minCubes, maxCubes));
            }

            return stacks;
        }
        public BlockStack GenerateStack(int x, int min, int max)
        {
            BlockStack stack = new BlockStack(x);

            int minS = Math.Min(min, max);
            int maxS = Math.Max(min, max);

            int cubesAmount = rnd.Next(minS, maxS + 1);

            for (int i = 0; i < cubesAmount; i++)
            {
                string id = "(" + x + "/" + i + ")";
                string color = ((ColorEnum.Colors)rnd.Next(0, 4)).ToString();
                int y = stack.Blocks.Count();

                stack.Blocks.Push(new Block(id, color, x, y));
            }

            return stack;
        }

        public List<BlockStack> ParseFile(string path)
        {
            List<BlockStack> stacks = new List<BlockStack>();
            bool needOneMoreStack = false;

            int lineCount = File.ReadAllLines(path).Length;
            stackMax = (lineCount - 1) / 2;
            if ((lineCount - 1) % 2 != 0)
            {
                needOneMoreStack = true;
                stackMax = (lineCount) / 2;
            }


            int c = -stackMax;
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    BlockStack stack = new BlockStack(c);
                    if (line != "" && !line.Contains("@random"))
                    {
                        string[] blocks = line.Split(' ');
                        int yCounter = 0;
                        foreach (string blockColor in blocks)
                        {
                            stack.Blocks.Push(new Block("("+c+"/"+yCounter+")", blockColor, c, yCounter));
                            yCounter++;
                        }
                    }
                    else if (line.Contains("@random"))
                    {
                        string[] data = line.Split(' ');
                        int min = int.Parse(data[1]);
                        int max = data.Length > 2 ? int.Parse(data[2]) : min;

                        stack = GenerateStack(c, min, max);
                    }

                    stacks.Add(stack);
                    c++;
                }
            }

            if (needOneMoreStack)
                stacks.Add(new BlockStack(c));

            return stacks;
        }


        private string path = Path.GetFullPath(".") + "\\Assets\\Levels\\";

        private Random rnd = new Random();
        private int minCubes = 1;
        private int maxCubes = 6;
        private int defaultStackMax = 10000;
    }
}
