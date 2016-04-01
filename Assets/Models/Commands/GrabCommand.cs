using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class GrabCommand : Command
    {
        public override void Do(RobotArm robotArm, SpeedMeter speedMeter)
        {
            if (world.RobotArm.Holding)
            {
                robotArm.PretendGrab();
            }
            else
            {
                world.Grab();

                if (world.RobotArm.Holding)
                {
                    robotArm.Grab();
                }
                else
                {
                    robotArm.PretendGrab();
                }
            }
        }
    }
}