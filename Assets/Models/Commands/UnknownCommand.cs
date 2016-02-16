using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class UnknownCommand : Command
    {
        public override void Do(RobotArm robotArm)
        {
            IsDone = true;
            message = "I dont even";
        }
    }
}