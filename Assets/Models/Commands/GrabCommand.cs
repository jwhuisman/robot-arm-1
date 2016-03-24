using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class GrabCommand : Command
    {
        public override void Do(RobotArm robotArm)
        {
            if (world.RobotArm.Holding)
            {
                robotArm.PretendGrab();
            }
            else
            {
                world.Grab();
                robotArm.Grab();
            }
        }
    }
}