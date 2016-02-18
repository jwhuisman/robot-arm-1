using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class GrabCommand : Command
    {
        public override void Do(RobotArm robotArm)
        {
            world.Grab();
            robotArm.Grab();

            IsDone = true;

            networkListener.ReturnMessage(message);
        }
    }
}