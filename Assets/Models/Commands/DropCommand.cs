using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class DropCommand : Command
    {
        public override void Do(RobotArm robotArm)
        {
            world.Drop();
            robotArm.Drop();

            IsDone = true;
        }
    }
}