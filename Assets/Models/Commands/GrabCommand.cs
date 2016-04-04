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
                statsCounter.Grabs++;
                world.Grab();
                robotArm.Grab();
            }
        }
    }
}