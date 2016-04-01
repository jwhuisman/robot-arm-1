﻿using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class ScanCommand : Command
    {
        public override void Do(RobotArm robotArm, SpeedMeter speedMeter)
        {
            message = world.Scan();
            robotArm.Scan();
        }
    }
}