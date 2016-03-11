using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class SpeedCommand : Command
    {
        public SpeedCommand(int speed)
        {
            Speed = speed;
        }
        public override void Do(RobotArm robotArm)
        {
            robotArm.UpdateSpeed(Speed);
        }

        public int Speed { get; set; }
    }
}