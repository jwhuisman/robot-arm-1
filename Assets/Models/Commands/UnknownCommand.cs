using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class UnknownCommand : Command
    {
        public override void Do(RobotArm robotArm)
        {
            message = "What is this I dont even";
            IsDone = true;
        }
    }
}