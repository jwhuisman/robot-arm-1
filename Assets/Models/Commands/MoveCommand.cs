using UnityEngine;

namespace Assets.Models.Commands
{
    public class MoveCommand : Command
    {
        public MoveCommand(string dir)
        {
            Direction = dir;
        }
        public override bool Do(RobotArmController robotArm)
        {
            bool result = false;
            if (Direction == "right")
            {
                robotArm.StartLerping(Vector3.right, 1);
                robotArm.text.text = "Moving to the right.";

                result = true;
                networkListener.ReturnMessage("Robot arm moved right");
            }
            else if (Direction == "left")
            {
                robotArm.StartLerping(Vector3.left, 1);
                robotArm.text.text = "Moving to the left.";

                result = true;
                networkListener.ReturnMessage("Robot arm moved left");
            }
            else if (Direction == "up")
            {
                robotArm.StartLerping(Vector3.up, 1);
                robotArm.text.text = "Moving up.";

                result = true;
                networkListener.ReturnMessage("Robot arm moved up");
            }

            return result;
        }

        public string Direction { get; set; }
    }
}