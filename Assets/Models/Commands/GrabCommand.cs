using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class GrabCommand : Command
    {
        public override void Do(RobotArm robotArm, SpeedMeter speedMeter, StatsCounter statsCounter)
        {
            if (world.RobotArm.Holding)
            {
                statsCounter.PretendGrabs++;
                robotArm.PretendGrab();
            }
            else
            {
                world.Grab();

                // world doesn't find a block after all.
                // In some scenarios we don't want to pick up a block
                // because there is no block. Example: clean assemblyline
                if (world.RobotArm.Holding)
                {
                    statsCounter.Grabs++;
                    robotArm.Grab();
                }
                else
                {
                    statsCounter.PretendGrabs++;
                    robotArm.PretendGrab();
                }
            }
        }
    }
}