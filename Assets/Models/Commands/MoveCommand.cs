using UnityEngine;

namespace Assets.Models.Commands
{
    public class MoveCommand : Command
    {
        public MoveCommand(string dir)
        {
            Direction = dir;
        }
        public override void Do(RobotArmController robotArm)
        {
            Vector3 vec = new Vector3();
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

            robotArm.StartLerping(vec, 1.2f);

            

            // should set 'command.IsDone = true' after the robotArm transform/animation is finished 
            // and networkListener.ReturnMessage(s) should be after the 'command.IsDone'
            IsDone = true;
            robotArm.text.text = s;
            networkListener.ReturnMessage("ok");
        }

        public string Direction { get; set; }
    }
}