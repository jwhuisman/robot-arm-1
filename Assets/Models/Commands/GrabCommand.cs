namespace Assets.Models.Commands
{
    public class GrabCommand : Command
    {
        public GrabCommand(string action)
        {
            Action = action;
        }
        public override bool Do(RobotArmController robotArm)
        {
            bool result = false, grab = false;
            string s = "";
            if (Action == "grab")
            {
                grab = true;
                s = "Grab a block";
            } else if (Action == "put")
            {
                grab = false;
                s = "Put a block down";
            }

            robotArm.StartPickUpPutDown(grab);

            robotArm.text.text = s;
            networkListener.ReturnMessage(s);


            // should return true after the robotArm animation is finished 
            // (result = await robotArm.StartPickUpPutDown(grab);)
            result = true;
            return result;
        }

        public string Action { get; set; }
    }
}