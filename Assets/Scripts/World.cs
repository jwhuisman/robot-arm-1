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
            Stacks = levels.GenerateRandomLevel();

            RobotArm = new RobotArmData(Stacks.Where(s => s.Blocks.Count > 0).Max(s => s.Blocks.Max(b => b.Y)) + 3);
        }

        public void LoadLevel(int level)
        {
            RobotArm.X = 0;

            if (level == 0)
            {
                Stacks = levels.GenerateRandomLevel();
            }
            if (level == 1)
            {
                Stacks = levels.LoadLevel1();
            }
            if (level == 2)
            {
                Stacks = levels.LoadLevel2();
            }
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
    }
}