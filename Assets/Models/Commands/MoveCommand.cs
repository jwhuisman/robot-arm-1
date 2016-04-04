using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class MoveCommand : Command
    {
        public MoveCommand(string dir)
        {
            Direction = dir;
        }

        public override void Do(RobotArm robotArm, SpeedMeter speedMeter, StatsCounter statsCounter)
        {
            if (Direction == "right")
            {
                statsCounter.MovesRight++;
                world.MoveRight();
                robotArm.MoveRight();
            }
            else if (Direction == "left")
            {
                statsCounter.MovesLeft++;
                world.MoveLeft();
                robotArm.MoveLeft();
            }
        }

        public string Direction { get; set; }
    }
}