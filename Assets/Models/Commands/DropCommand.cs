using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class DropCommand : Command
    {
        public override void Do(RobotArm robotArm, SpeedMeter speedMeter, StatsCounter statsCounter)
        {
            if (world.RobotArm.Holding)
            {
                statsCounter.Drops++;
                world.Drop();
                robotArm.Drop();
            }
            else
            {
                statsCounter.PretendDrops++;
                robotArm.PretendDrop();
            }
        }
    }
}
