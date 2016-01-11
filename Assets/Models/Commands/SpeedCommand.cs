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
            if (Speed <= 100 && Speed >= 0)
            {
                float time = (100f - Speed) / 100f;
                robotArm.text.text = "Speed of the robot arm has been changed to: " + time + " seconds.";
                robotArm.speedText.text = "Speed: " + time + " seconds";
                robotArm.timeTakenDuringLerp = time;
            }
            else
            {
                robotArm.text.text = "You can't go lower than 0, or higher than 100.";
            }
        }

        public float Speed { get; set; }
    }
}