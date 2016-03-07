using System.Linq;
using System.Collections.Generic;
using System;
using Assets.Models.WorldData;

namespace Assets.Scripts.WorldData
{
    public class World
    {
        public List<BlockStack> Stacks = new List<BlockStack>();
        public RobotArmData RobotArm;

        public World()
        {
            for (int x = stackMin; x < stackMax; x++)
            {
                AddStack(x);
            }

            RobotArm = new RobotArmData(Stacks.Max(s => s.Blocks.Max(b => b.Y)) + 3);
        }

        public void AddStack(int x)
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

            Stacks.Add(stack);
        }



        public void MoveLeft()
        {
            RobotArm.X--;
        }
        public void MoveRight()
        {
            RobotArm.X++;
        }
        public void MoveUp()
        {
            RobotArm.Y++;
        }
        public void Grab()
        {
            if (!RobotArm.Holding)
            {
                int i = Stacks.FindIndex(s => s.Id == RobotArm.X);

                if (Stacks[i].Blocks.Count != 0)
                {
                    RobotArm.HoldingBlock = Stacks[i].Blocks.Pop();
                    RobotArm.Holding = true;
                }
            }
        }
        public void Drop()
        {
            if (RobotArm.Holding)
            {
                int i = Stacks.FindIndex(s => s.Id == RobotArm.X);

                RobotArm.HoldingBlock.Y = Stacks[i].Blocks.Count();
                Stacks[i].Blocks.Push(RobotArm.HoldingBlock);

                RobotArm.Holding = false;
            }
        }
        public string Scan()
        {
            if (RobotArm.Holding)
            {
                return RobotArm.HoldingBlock.Color;
            }

            return "none";
        }

        private Random rnd = new Random();
        private int stackMin = -10000;
        private int stackMax = 10000;
        private int minCubes = 1;
        private int maxCubes = 6;
    }
}