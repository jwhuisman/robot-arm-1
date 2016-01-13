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
            Vector3 vec = new Vector3();
            bool result = false;
            string s = "";
            if (Direction == "right")
            {
                vec = Vector3.right;
                s = "Moved right";
            }
            else if (Direction == "left")
            {
                vec = Vector3.left;
                s = "Moved left";
            }
            else if (Direction == "up")
            {
                vec = Vector3.up;
                s = "Moved up";
            }

            robotArm.StartLerping(vec, 1);

            robotArm.text.text = s;
            networkListener.ReturnMessage(s);

            // should return true after the robotArm animation is finished 
            // (result = await robotArm.StartLerping(vec, 1);)
            result = true;
            return result;
        }

        public string Direction { get; set; }
    }
}