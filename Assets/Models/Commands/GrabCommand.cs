namespace Assets.Models.Commands
{
    public class GrabCommand : Command
    {
        public GrabCommand(string action)
        {
            Action = action;
        }
        public override void Do(RobotArmController robotArm)
        {
            if (Action == "grab")
            {
                robotArm.StartPickUpPutDown(true);
                robotArm.text.text = "Going to pick up a block.";
            } else if (Action == "put")
            {
                robotArm.StartPickUpPutDown(false);
                robotArm.text.text = "Putting down a block.";
            }
        }

        public string Action { get; set; }
    }
}