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
            bool result = false;
            if (Action == "grab")
            {
                robotArm.StartPickUpPutDown(true);
                robotArm.text.text = "Going to pick up a block.";

                result = true;
                networkListener.ReturnMessage("Picked up a block");
            } else if (Action == "put")
            {
                robotArm.StartPickUpPutDown(false);
                robotArm.text.text = "Putting down a block.";

                result = true;
                networkListener.ReturnMessage("Put down a block");
            }

            return result;
        }

        public string Action { get; set; }
    }
}