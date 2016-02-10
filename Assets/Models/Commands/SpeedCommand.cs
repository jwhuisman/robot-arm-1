namespace Assets.Models.Commands
{
    public class SpeedCommand : Command
    {
        public SpeedCommand(int speed)
        {
            Speed = speed;
        }
        public override void Do(RobotArmController robotArm)
        {
            robotArm.SetSpeed(Speed);

            IsDone = true;
            networkListener.ReturnMessage("ok");
        }

        public int Speed { get; set; }
    }
}