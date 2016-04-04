using System.Linq;
using System.Collections.Generic;
using System;
using Assets.Models.WorldData;

namespace Assets.Scripts.WorldData
{
    public class World
    {
        public List<BlockStack> Stacks = new List<BlockStack>();
        public Levels levels = new Levels();
        public RobotArmData RobotArm;

        public World()
        {
            bool levelExists;
            Stacks = levels.LoadLevel("random", out levelExists);

            RobotArm = new RobotArmData(Stacks.Where(s => s.Blocks.Count > 0).Max(s => s.Blocks.Max(b => b.Y)) + 3);
        }

        public int Height
        {
            get
            {
                return Stacks.Max(s => s.Blocks.Count);
            }
        }

        public BlockStack CurrentStack
        {
            get
            {
                return Stacks.Single(s => s.Id == RobotArm.X);
            }
        }

        public bool LoadLevel(string name)
        {
            RobotArm.X = 0;
            RobotArm.Holding = false;
            RobotArm.HoldingBlock = new Block();

            bool levelExists;
            Stacks = levels.LoadLevel(name, out levelExists);

            return levelExists;
        }

        public void MoveLeft()
        {
            RobotArm.X--;
        }
        public void MoveRight()
        {
            RobotArm.X++;
        }
        public void Grab()
        {
            if (!RobotArm.Holding)
            {
                int i = Stacks.FindIndex(s => s.Id == RobotArm.X);

                if (Stacks[i].Blocks.Count() > 0)
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

                if (i != -1)
                {
                    RobotArm.HoldingBlock.Y = Stacks[i].Blocks.Count();
                    Stacks[i].Blocks.Push(RobotArm.HoldingBlock);
                        
                    RobotArm.Holding = false;
                }
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
    }
}