﻿using UnityEngine;

namespace Assets.Models.Commands
{
    public class MoveCommand : Command
    {
        public MoveCommand(string dir)
        {
            Direction = dir;
        }
        public override void Do(RobotArmController robotArm)
        {
            if (Direction == "right")
            {
                robotArm.StartLerping(Vector3.right, 1);
                robotArm.text.text = "Moving to the right.";
            }
            else if (Direction == "left")
            {
                robotArm.StartLerping(Vector3.left, 1);
                robotArm.text.text = "Moving to the left.";
            }
        }

        public string Direction { get; set; }
    }
}