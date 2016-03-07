using Assets.Scripts.View;
using System.Threading;

namespace Assets.Models.Commands
{
    public class MoveCommand : Command
    {
        public MoveCommand(string dir)
        {
            Direction = dir;
        }
        public override void Do(RobotArm robotArm)
        {
            if (Direction == "right")
            {
                world.MoveRight();
                robotArm.MoveRight();
            }
            else if (Direction == "left")
            {
                world.MoveLeft();
                robotArm.MoveLeft();
            }

            IsDone = true;

            networkListener.ReturnMessage(message);
        }

        public string Direction { get; set; }
    }
}