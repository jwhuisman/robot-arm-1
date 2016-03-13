using Assets.Scripts.View;

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
            }
            else if (Direction == "left")
            {
                world.MoveLeft();
            }

            robotArm.HorizontalMovement(Direction);
        }

        public string Direction { get; set; }
    }
}