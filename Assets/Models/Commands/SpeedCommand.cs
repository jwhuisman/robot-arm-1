﻿namespace Assets.Models.Commands
{
    public class SpeedCommand : Command
    {
        public SpeedCommand(int speed)
        {
            Speed = speed;
        }
        public override bool Do(RobotArmController robotArm)
        {
            bool result = false;
            if (Speed <= 100 && Speed >= 0)
            {
                float time = (100f - Speed) / 100f;
                robotArm.text.text = "Speed of the robot arm has been changed to: " + time + " seconds.";
                robotArm.speedText.text = "Speed: " + time + " seconds";
                robotArm.timeTakenDuringLerp = time;

                result = true;
                nListener.ReturnMessage(string.Format("Speed set to: {0} seconds (per animation)", time));
            }
            else
            {
                robotArm.text.text = "Speed can't go lower than 0 or higher than 100.";

                result = true;
                nListener.ReturnMessage("Speed can't go lower than 0 or higher than 100");
            }

            return result;
        }

        public float Speed { get; set; }
    }
}