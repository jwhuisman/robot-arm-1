using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class ScanCommand : Command
    {
        public override void Do(RobotArm robotArm)
        {
            message = "Color: " + world.Scan();
            robotArm.Scan();

            IsDone = true;

            networkListener.ReturnMessage(message);
        }
    }
}