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

        public void LoadLevel(string name)
        {
            RobotArm.X = 0;
            RobotArm.Holding = false;
            RobotArm.HoldingBlock = new Block();

            if (name == "")
            {
                Stacks = levels.GenerateRandomLevel();
            }
            else
            {
                Stacks = levels.LoadLevel(name);
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

                if (1 != -1 && Stacks[i].Blocks.Count != 0)
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