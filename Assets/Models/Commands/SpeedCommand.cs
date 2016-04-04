using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class SpeedCommand : Command
    {
        
        public SpeedCommand(int speed)
        {
            Speed = speed;
        }

        public override void Do(RobotArm robotArm, SpeedMeter speedMeter, StatsCounter statsCounter)
        {
            robotArm.UpdateSpeed(Speed);
            speedMeter.UpdateSpeed(Speed);
        }

        public int Speed { get; set; }
    }
}