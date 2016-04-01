using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class DropCommand : Command
    {
        public override void Do(RobotArm robotArm, SpeedMeter speedMeter)
        {
            if (world.RobotArm.Holding)
            {
                world.Drop();
                robotArm.Drop();
            }
            else
            {
                robotArm.PretendDrop();
            }
        }
    }
}
