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
            float time = robotArm.UpdateSpeed(Speed);

            IsDone = true;
            networkListener.ReturnMessage(string.Format("Speed set to: {0} seconds (per animation)", time));
        }

        public float Speed { get; set; }
    }
}